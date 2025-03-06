using Microsoft.AspNetCore.Mvc;
using ContactManagementAPI.Models.Auth;
using ContactManagementAPI.Auth;
using Microsoft.AspNetCore.Authorization;
namespace ContactManagementAPI.Controllers
{
    /// <summary>
    /// Controller for authentication operations
    /// </summary>
    [ApiVersionNeutral]
    [Route("api/auth")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly JwtHandler _jwtHandler;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class
        /// </summary>
        /// <param name="jwtHandler">The JWT handler service</param>
        /// <param name="configuration">The application configuration</param>
        public AuthController(JwtHandler jwtHandler, IConfiguration configuration)
        {
            _jwtHandler = jwtHandler;
            _configuration = configuration;
        }

        /// <summary>
        /// Authenticates a user and generates a JWT token
        /// </summary>
        /// <param name="login">The login credentials</param>
        /// <returns>The JWT token and user information if authentication is successful</returns>
        /// <response code="200">Returns the JWT token and user information</response>
        /// <response code="401">If authentication fails</response>
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

        /// <summary>
        /// Gets a plain text JWT token for a user
        /// </summary>
        /// <param name="login">The login credentials</param>
        /// <returns>A JWT token as plain text if authentication is successful</returns>
        /// <response code="200">Returns the JWT token as plain text</response>
        /// <response code="401">If authentication fails</response>
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