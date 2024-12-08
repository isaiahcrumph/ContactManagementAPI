using System.ComponentModel.DataAnnotations;
namespace ContactManagementAPI.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; }

/*        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(ErrorMessage = "Must input email address")]*/
        //find the regular expresion for writing an email adress adn paste
        ///make sure the email format is valid
        ///

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
    ErrorMessage = "Invalid email format")]

        public string Email { get; set; }


        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [RegularExpression(@"^\d{3}-\d{4}$|^\d{3}-\d{3}-\d{4}$", ErrorMessage = "Phone number must be in format: 555-1234 or 555-123-1234")]
        public string PhoneNumber { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; }

        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters")]
        public string City { get; set; }

        [StringLength(2, MinimumLength = 2, ErrorMessage = "State must be 2 characters")]
        [RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "State must be 2 uppercase letters")]
        public string State { get; set; }

        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "ZipCode must be 5 digits or 5+4 digits")]
        public string ZipCode { get; set; }
    }
}
