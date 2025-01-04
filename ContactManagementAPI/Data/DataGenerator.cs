using ContactManagementAPI.Models;

namespace ContactManagementAPI.Data
{
    public static class DataGenerator
    {
        private static readonly string[] FirstNames = { "James", "Mary", "John", "Patricia", "Robert", "Jennifer", "Michael", "Linda", "William", "Elizabeth" };
        private static readonly string[] LastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez" };
        private static readonly string[] Cities = { "New York", "Los Angeles", "Chicago", "Houston", "Phoenix", "Philadelphia", "San Antonio", "San Diego", "Dallas", "San Jose" };
        private static readonly string[] States = { "NY", "CA", "IL", "TX", "AZ", "PA", "TX", "CA", "TX", "CA" };
        private static readonly Random Random = new Random();

        public static List<Contact> GenerateContacts(int count)
        {
            var contacts = new List<Contact>();

            for (int i = 0; i < count; i++)
            {
                var firstName = FirstNames[Random.Next(FirstNames.Length)];
                var lastName = LastNames[Random.Next(LastNames.Length)];
                var cityIndex = Random.Next(Cities.Length);

                contacts.Add(new Contact
                {
                    Name = $"{firstName} {lastName}",
                    Email = $"{firstName.ToLower()}.{lastName.ToLower()}@example.com",
                    PhoneNumber = $"{Random.Next(100, 999)}-{Random.Next(100, 999)}-{Random.Next(1000, 9999)}",
                    Address = $"{Random.Next(100, 9999)} {lastName} Street",
                    City = Cities[cityIndex],
                    State = States[cityIndex],
                    ZipCode = Random.Next(10000, 99999).ToString()
                });
            }

            return contacts;
        }
    }
}