using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
#nullable disable
namespace ContactManagementAPI.Models
{
    /// <summary>
    /// Represents a contact in the system
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// Unique identifier for the contact
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// First name of the contact
        /// </summary>
        /// <example>John</example>
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters")]
        [RegularExpression(@"^[A-Za-z\s'-]+$", ErrorMessage = "First name should contain only letters, spaces, hyphens, and apostrophes")]
        [SwaggerSchema(Description = "First name of contact")]
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the contact
        /// </summary>
        /// <example>Doe</example>
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters")]
        [RegularExpression(@"^[A-Za-z\s'-]+$", ErrorMessage = "Last name should contain only letters, spaces, hyphens, and apostrophes")]
        [SwaggerSchema(Description = "Last name of contact")]
        public string LastName { get; set; }

        /// <summary>
        /// Full name of the contact (computed property)
        /// </summary>
        [SwaggerSchema(Description = "Full name of contact (computed from FirstName and LastName)")]
        public string FullName => $"{FirstName} {LastName}";

        // Keep the rest of your properties unchanged
        /// <summary>
        /// Email address for the contact
        /// </summary>
        /// <example>john.doe@example.com</example>
        [Required(ErrorMessage = "Email is required")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            ErrorMessage = "Please enter a valid email address")]
        [SwaggerSchema(Description = "Email address")]
        public string Email { get; set; }

        /// <summary>
        /// Phone number for the contact
        /// </summary>
        /// <example>555-123-4567</example>
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$",
            ErrorMessage = "Phone number must be in format: 555-123-4567")]
        [SwaggerSchema(Description = "Phone number in format: 555-123-4567")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Street address for the contact
        /// </summary>
        /// <example>123 Main St</example>
        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 200 characters")]
        [SwaggerSchema(Description = "Street address")]
        public string Address { get; set; }

        /// <summary>
        /// City of residence for the contact
        /// </summary>
        /// <example>Seattle</example>
        [Required(ErrorMessage = "City is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "City must be between 2 and 50 characters")]
        [SwaggerSchema(Description = "City name")]
        public string City { get; set; }

        /// <summary>
        /// Two-letter state code
        /// </summary>
        /// <example>WA</example>
        [Required(ErrorMessage = "State is required")]
        [RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "State must be 2 uppercase letters (e.g., WA)")]
        [SwaggerSchema(Description = "Two-letter state code")]
        public string State { get; set; }

        /// <summary>
        /// ZIP Code for the contact's address
        /// </summary>
        /// <example>98101</example>
        [Required(ErrorMessage = "ZIP Code is required")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "ZIP Code must be 5 digits or 5+4 digits format")]
        [SwaggerSchema(Description = "ZIP Code in 5-digit or ZIP+4 format")]
        public string ZipCode { get; set; }
    }
}