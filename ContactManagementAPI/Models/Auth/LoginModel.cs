namespace ContactManagementAPI.Models.Auth
{
    /// <summary>
    /// Model for user login credentials
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// The username for authentication
        /// </summary>
        /// <example>admin</example>
        public required string Username { get; set; }

        /// <summary>
        /// The password for authentication
        /// </summary>
        /// <example>admin123</example>
        public required string Password { get; set; }
    }
}