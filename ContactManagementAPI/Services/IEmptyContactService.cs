using ContactManagementAPI.Models;
using ContactManagementAPI.Models.Pagination;

namespace ContactManagementAPI.Services
{
    /// <summary>
    /// Service interface for contact management operations
    /// </summary>
    public interface IEmptyContactService
    {
        /// <summary>
        /// Gets all contacts in the system
        /// </summary>
        /// <returns>A collection of all contacts</returns>
        Task<IEnumerable<EmptyContact>> GetAllContacts();

        /// <summary>
        /// Gets a specific contact by ID
        /// </summary>
        /// <param name="id">The ID of the contact to retrieve</param>
        /// <returns>The contact if found, otherwise null</returns>
        Task<EmptyContact> GetContact(int id);

        /// <summary>
        /// Creates a new contact
        /// </summary>
        /// <param name="contact">The contact data to create</param>
        /// <returns>The created contact with assigned ID</returns>
        Task<EmptyContact> CreateContact(EmptyContact contact);

        /// <summary>
        /// Updates an existing contact
        /// </summary>
        /// <param name="contact">The updated contact data</param>
        Task UpdateContact(EmptyContact contact);

        /// <summary>
        /// Deletes a contact
        /// </summary>
        /// <param name="id">The ID of the contact to delete</param>
        Task DeleteContact(int id);

        /// <summary>
        /// Updates specific properties of a contact
        /// </summary>
        /// <param name="id">The ID of the contact to update</param>
        /// <param name="patchValues">Dictionary of property names and values to update</param>
        Task UpdateContactPartial(int id, Dictionary<string, object> patchValues);

        /// <summary>
        /// Checks if a contact exists
        /// </summary>
        /// <param name="id">The ID of the contact to check</param>
        /// <returns>True if the contact exists, otherwise false</returns>
        bool ContactExists(int id);

        /// <summary>
        /// Gets filtered and sorted contacts
        /// </summary>
        /// <param name="name">Optional filter by contact name</param>
        /// <param name="city">Optional filter by city</param>
        /// <param name="state">Optional filter by state</param>
        /// <param name="sortBy">Optional field to sort by</param>
        /// <param name="order">Optional sort order (asc/desc)</param>
        /// <returns>A filtered and sorted collection of contacts</returns>
        Task<IEnumerable<EmptyContact>> GetFilteredContacts(
            string? name = null,
            string? city = null,
            string? state = null,
            string? sortBy = null,
            string? order = null);

        /// <summary>
        /// Searches contacts by name or email.
        /// </summary>
        /// <param name="query">The search query string</param>
        /// <returns>A list of matching contacts</returns>
        Task<IEnumerable<EmptyContact>> SearchContacts(string query);

        /// <summary>
        /// Gets a paged list of filtered and sorted contacts
        /// </summary>
        /// <param name="name">Optional filter by contact name</param>
        /// <param name="city">Optional filter by city</param>
        /// <param name="state">Optional filter by state</param>
        /// <param name="sortBy">Optional field to sort by</param>
        /// <param name="order">Optional sort order (asc/desc)</param>
        /// <param name="pageNumber">Page number (1-based)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <returns>A paged result of filtered and sorted contacts</returns>
        Task<PagedResult<EmptyContact>> GetFilteredContactsPaged(
            string? name = null,
            string? city = null,
            string? state = null,
            string? sortBy = null,
            string? order = null,
            int pageNumber = 1,
            int pageSize = 10);

    }
}