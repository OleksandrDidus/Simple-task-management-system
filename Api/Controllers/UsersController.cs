using Api.Data.DTO;
using Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _userService.RegisterUserAsync(registerDto);
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var response = await _userService.LoginUserAsync(loginDto);
            var token = response.Data;
            Response.Cookies.Append("jwt", token);

            if (!response.Success)
            {
                return Unauthorized(response.Message);
            }

            return Ok(new { token = response.Data });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok(new { message = "Logged out successfully" });
        }
    }
}
