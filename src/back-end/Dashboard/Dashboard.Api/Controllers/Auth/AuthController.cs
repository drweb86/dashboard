using System.Threading.Tasks;
using AutoMapper;
using Dashboard.Dtos.Auth;
using Dashboard.Services;
using Dashboard.ViewModels.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Api.Controllers.Auth
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        // private readonly ILogger<AuthController> _logger;

        public AuthController(IMapper mapper,
            IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        // this if for test
        [HttpPost]
        [Route("test")]
        public async Task<IActionResult> Test([FromBody] AuthRegisterInputModel registerInfo)
        {
            return Ok(new AuthResultModel() { Message = "Test" });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] AuthRegisterInputModel registerInfo)
        {
            if (await _userService.RegisterAsync(
                _mapper.Map<AuthRegisterInputModel, AuthRegisterDto>(registerInfo)))
                return Ok(new AuthResultModel() { Message = "Registered" });

            return BadRequest(new AuthResultModel { Message = "Username is already taken" });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] AuthLoginInputModel loginInfo)
        {
            var authInfo = await _userService.AuthenticateAsync(_mapper.Map<AuthLoginInputModel, AuthLoginDto>(loginInfo));

            if (authInfo == null)
                return BadRequest(new AuthResultModel { Message = "Username or password is incorrect" });

            return Ok(_mapper.Map<AuthDto, AuthResultModel>(authInfo));
        }
    }
}
