using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dashboard.Services;
using Dashboard.ViewModels.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dashboard.Api.Controllers.Auth
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger,
            IUserService userService)
        {
            _logger = logger;
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
            // This is to be improved one time further

            await _userService.RegisterAsync(registerInfo);
            return Ok(new AuthResultModel() { Message = "Registered" });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] AuthLoginInputModel loginInfo)
        {
            var user = await _userService.AuthenticateAsync(loginInfo);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }
    }
}
