using Dashboard.Core;
using Dashboard.Entitites;
using Dashboard.Repositories.Services;
using Dashboard.ViewModels.Auth;
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
        Task RegisterAsync(AuthRegisterInputModel registerInfo);
        Task<AuthResultModel> AuthenticateAsync(AuthLoginInputModel loginInfo);
    }

    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(
            IOptions<AppSettings> appSettings,
            IUnitOfWork unitOfWork)
        {
            _appSettings = appSettings.Value;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthResultModel> AuthenticateAsync(AuthLoginInputModel loginInfo)
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
    
            var result = new AuthResultModel(); // separate dto - viewmodels?
            result.Token = tokenHandler.WriteToken(token);
            return result;
        }

        public async Task RegisterAsync(AuthRegisterInputModel registerInfo)
        {
            // TODO: use mapping for that.


            var user = new User()
            {
                FirstName = registerInfo.FirstName,
                LastName = registerInfo.LastName,
                Username = registerInfo.Username,
                Password = registerInfo.Password,
            };

            _unitOfWork.UserRepository.Add(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
