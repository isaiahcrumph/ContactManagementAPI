using Swashbuckle.AspNetCore.Filters;
using ContactManagementAPI.Models;

namespace ContactManagementAPI.Examples
{
    public class ContactExample : IExamplesProvider<Contact>
    {
        public Contact GetExamples()
        {
            return new Contact
            {
                Name = "John Doe",
                Email = "johndoe@example.com",
                PhoneNumber = "555-123-4567",
                Address = "123 Main Street",
                City = "Seattle",
                State = "WA",
                ZipCode = "98101"
            };
        }
    }
}