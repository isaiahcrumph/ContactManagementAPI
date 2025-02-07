using Microsoft.AspNetCore.Mvc;
using ContactManagementAPI.Models.Auth;
using ContactManagementAPI.Auth;

namespace ContactManagementAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtHandler _jwtHandler;

        public AuthController(JwtHandler jwtHandler)
        {
            _jwtHandler = jwtHandler;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<string> Login([FromBody] LoginModel login)
        {
            // For demo purposes - in real world, verify against database
            if (login.Username == "admin" && login.Password == "admin123")
            {
                var token = _jwtHandler.GenerateToken(login.Username, "Admin");
                return Ok(token);
            }

            return Unauthorized("Invalid username or password");
        }
    }
}