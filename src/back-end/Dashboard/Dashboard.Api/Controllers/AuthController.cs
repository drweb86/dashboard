using System;
using System.Collections.Generic;
using System.Linq;
using Dashboard.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dashboard.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public AuthResultModel Register([FromBody] AuthRegisterInputModel registerInfo)
        {
            return new AuthResultModel() { Message = "Auth OK" };
        }

        [HttpPost]
        [Route("login")]
        public string Get()
        {
            return "Fuck Yea 2";
        }
    }
}
