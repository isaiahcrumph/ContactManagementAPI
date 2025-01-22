using ContactManagementAPI.Data;
using ContactManagementAPI.Models;
using ContactManagementAPI.Models.Pagination;
using Microsoft.EntityFrameworkCore;

namespace ContactManagementAPI.Services
{
    public class ContactService : IContactService
    {
        private readonly ContactDbContext _context;

        public ContactService(ContactDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contact>> GetAllContacts()
        {
            return await _context.Contacts.ToListAsync();
        }

        public async Task<Contact?> GetContact(int id)
        {
            return await _context.Contacts.FindAsync(id);
        }

        public async Task<Contact> CreateContact(Contact contact)
        {
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return contact;
        }

        public async Task UpdateContact(Contact contact)
        {
            _context.Entry(contact).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
            }
        }

public async Task<IEnumerable<Contact>> GetFilteredContacts(
    string? name = null, 
    string? city = null, 
    string? state = null,
    string? sortBy = null,
    string? order = null)
{
    var query = _context.Contacts.AsQueryable();

    // Existing filtering logic
    if (!string.IsNullOrEmpty(name))
        query = query.Where(c => c.Name.Contains(name));
    if (!string.IsNullOrEmpty(city))
        query = query.Where(c => c.City.Contains(city));
    if (!string.IsNullOrEmpty(state))
        query = query.Where(c => c.State == state);

    // Add sorting
    if (!string.IsNullOrEmpty(sortBy))
    {
        query = sortBy.ToLower() switch
        {
            "name" => order?.ToLower() == "desc" ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
            "city" => order?.ToLower() == "desc" ? query.OrderByDescending(c => c.City) : query.OrderBy(c => c.City),
            "state" => order?.ToLower() == "desc" ? query.OrderByDescending(c => c.State) : query.OrderBy(c => c.State),
            _ => query.OrderBy(c => c.Id)
        };
    }


    
    return await query.ToListAsync();
}

        public async Task<PagedResult<Contact>> GetFilteredContactsPaged(
    string? name = null,
    string? city = null,
    string? state = null,
    string? sortBy = null,
    string? order = null,
    int pageNumber = 1,
    int pageSize = 10)
        {
            // Start with queryable
            var query = _context.Contacts.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(name))
                query = query.Where(c => c.Name.Contains(name));
            if (!string.IsNullOrEmpty(city))
                query = query.Where(c => c.City.Contains(city));
            if (!string.IsNullOrEmpty(state))
                query = query.Where(c => c.State == state);

            // Apply sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                query = sortBy.ToLower() switch
                {
                    "name" => order?.ToLower() == "desc" ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
                    "city" => order?.ToLower() == "desc" ? query.OrderByDescending(c => c.City) : query.OrderBy(c => c.City),
                    "state" => order?.ToLower() == "desc" ? query.OrderByDescending(c => c.State) : query.OrderBy(c => c.State),
                    _ => query.OrderBy(c => c.Id)
                };
            }

            // Get total count before paging
            var totalCount = await query.CountAsync();

            // Apply paging
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Contact>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Data = items
            };
        }

        public async Task UpdateContactPartial(int id, Dictionary<string, object> patchValues)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
                throw new KeyNotFoundException("Contact not found");

            foreach (var field in patchValues)
            {
                var property = typeof(Contact).GetProperty(field.Key);
                if (property != null)
                {
                    property.SetValue(contact, Convert.ChangeType(field.Value, property.PropertyType));
                }
            }

            await _context.SaveChangesAsync();
        }

        public bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }
    }
}