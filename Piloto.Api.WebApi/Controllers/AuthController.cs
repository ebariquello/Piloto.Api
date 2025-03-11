using Microsoft.AspNetCore.Mvc;
using Piloto.Api.Application.DTO.DTO;
using Piloto.Api.Application.Interfaces;


namespace Piloto.Api.WebApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IApplicationServiceAuthUser _applicationServicesUser;

        public AuthController(IApplicationServiceAuthUser applicationServicesUser)
        {
            _applicationServicesUser = applicationServicesUser;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            var resultUser = await _applicationServicesUser.RegisterUserAsync(model);
            if (resultUser.Succeeded)
            {
                return Ok(new { message = "User registered successfully" });
            }

            return BadRequest(resultUser.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var token = await _applicationServicesUser.LoginUserAsync(model);
            if (token == null) return Unauthorized();

            return Ok(new { token });
        }
    }
}
