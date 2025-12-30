using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planner.Application.interfaces;
using Planner.Application.DTOs.Auth;
using System;
using System.Threading.Tasks;

namespace Planner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserInputModel input)
        {
            var result = await _authService.LoginAsync(input);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserInputModel input)
        {
            var result = await _authService.RegisterAsync(input);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });
            return Ok(new { message = result.Message });
        }
    }
}
