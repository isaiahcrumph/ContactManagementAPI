using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace ContactManagementAPI.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        [SwaggerSchema(Description = "Full name of contact")]

        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            ErrorMessage = "Please enter a valid email address")]
        [SwaggerSchema(Description = "Email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$",
            ErrorMessage = "Phone number must be in format: 555-123-4567")]
        [SwaggerSchema(Description = "Phone number in format: 555-123-4567")]

        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 200 characters")]
        [SwaggerSchema(Description = "Street address")]

        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "City must be between 2 and 50 characters")]
        [SwaggerSchema(Description = "City name")]

        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        [RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "State must be 2 uppercase letters (e.g., WA)")]
        [SwaggerSchema(Description = "Two-letter state code")]

        public string State { get; set; }

        [Required(ErrorMessage = "ZIP Code is required")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "ZIP Code must be 5 digits or 5+4 digits format")]
        [SwaggerSchema(Description = "ZIP Code in 5-digit or ZIP+4 format")]

        public string ZipCode { get; set; }
    }
}
