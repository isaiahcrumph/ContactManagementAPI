using Microsoft.AspNetCore.Mvc;
using ContactManagementAPI.Models.Auth;
using ContactManagementAPI.Auth;
using Microsoft.AspNetCore.Authorization;
namespace ContactManagementAPI.Controllers
{
    [ApiVersionNeutral]
    [Route("api/auth")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly JwtHandler _jwtHandler;
        private readonly IConfiguration _configuration; // Add this line

        public AuthController(JwtHandler jwtHandler, IConfiguration configuration) // Add configuration parameter
        {
            _jwtHandler = jwtHandler;
            _configuration = configuration; // Add this line
        }

        [HttpPost("login")]
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult Login([FromBody] LoginModel login)
        {
            // Validate credentials
            string role = "";
            string username = "";

            if (login.Username == "admin" && login.Password == "admin123")
            {
                username = login.Username;
                role = "Admin";
            }
            else if (login.Username == "user" && login.Password == "user123")
            {
                username = login.Username;
                role = "User";
            }
            else
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            var token = _jwtHandler.GenerateToken(username, role);

            // Check if client wants plaintext (for easy copying)
            if (Request.Headers.Accept.Any(h => h.Contains("text/plain")))
            {
                return Content(token, "text/plain");
            }

            // Otherwise return json with more details
            return Ok(new
            {
                token = token,
                username = username,
                role = role,
                expiration = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationInMinutes"]))
            });
        }

        [HttpPost("token")]
        [Consumes("application/json")]
        [Produces("text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<string> GetToken([FromBody] LoginModel login)
        {
            // Validate credentials
            if (login.Username == "admin" && login.Password == "admin123")
            {
                return Content(_jwtHandler.GenerateToken(login.Username, "Admin"), "text/plain");
            }
            if (login.Username == "user" && login.Password == "user123")
            {
                return Content(_jwtHandler.GenerateToken(login.Username, "User"), "text/plain");
            }

            return Unauthorized("Invalid username or password");
        }
    }
}
