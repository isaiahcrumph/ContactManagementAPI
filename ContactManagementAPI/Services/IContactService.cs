using ContactManagementAPI.Models;

namespace ContactManagementAPI.Services
{
    public interface IContactService
    {
        Task<IEnumerable<Contact>> GetAllContacts();
        Task<Contact> GetContact(int id);
        Task<Contact> CreateContact(Contact contact);
        Task UpdateContact(Contact contact);
        Task DeleteContact(int id);
        bool ContactExists(int id);
    }
}