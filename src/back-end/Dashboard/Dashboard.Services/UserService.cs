using AutoMapper;
using Dashboard.Core;
using Dashboard.Dtos.Auth;
using Dashboard.Entitites;
using Dashboard.Repositories.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Services
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(AuthRegisterDto registerInfo);
        Task<AuthDto> AuthenticateAsync(AuthLoginDto loginInfo);
    }

    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(
            IOptions<AppSettings> appSettings,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _appSettings = appSettings.Value;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AuthDto> AuthenticateAsync(AuthLoginDto loginInfo)
        {
            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Username == loginInfo.Username && x.Password == loginInfo.Password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.TokenEncryptionPassword);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
    
            var result = new AuthDto();
            result.Token = tokenHandler.WriteToken(token);
            return result;
        }

        public async Task<bool> RegisterAsync(AuthRegisterDto registerInfo)
        {
            if (await _unitOfWork.UserRepository.FirstOrDefaultAsync(
                u => u.Username.ToLower() == registerInfo.Username.ToLower()) != null)
                return false;

            var user = _mapper.Map<AuthRegisterDto, User>(registerInfo);
            
            _unitOfWork.UserRepository.Add(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
