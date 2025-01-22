using ContactManagementAPI.Models;
using ContactManagementAPI.Models.Pagination;

namespace ContactManagementAPI.Services
{
    public interface IContactService
    {
        Task<IEnumerable<Contact>> GetAllContacts();
        Task<Contact> GetContact(int id);
        Task<Contact> CreateContact(Contact contact);
        Task UpdateContact(Contact contact);
        Task DeleteContact(int id);
        Task UpdateContactPartial(int id, Dictionary<string, object> patchValues);
        bool ContactExists(int id);

        // Original method for filtering and sorting
        Task<IEnumerable<Contact>> GetFilteredContacts(
            string? name = null,
            string? city = null,
            string? state = null,
            string? sortBy = null,
            string? order = null);

        // New method for paging
        Task<PagedResult<Contact>> GetFilteredContactsPaged(
            string? name = null,
            string? city = null,
            string? state = null,
            string? sortBy = null,
            string? order = null,
            int pageNumber = 1,
            int pageSize = 10);
    }
}