using ContactManagementAPI.Models;

namespace ContactManagementAPI.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ContactDbContext context)
        {
            if (context.Contacts.Any())
                return;  // DB has already been seeded

            var contacts = DataGenerator.GenerateContacts(1000);
            context.Contacts.AddRange(contacts);
            context.SaveChanges();
        }
    }
}