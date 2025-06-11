using ContactManagementAPI.Data;
using ContactManagementAPI.Models;
using ContactManagementAPI.Models.Pagination;
using Microsoft.EntityFrameworkCore;

namespace ContactManagementAPI.Services
{
    public class EmptyContactService : IEmptyContactService
    {
        private readonly ContactDbContext _context;

        public EmptyContactService(ContactDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmptyContact>> GetAllContacts()
        {
            return await _context.EmptyContacts.ToListAsync();
        }

        public async Task<EmptyContact> GetContact(int id)
        {
            var contact = await _context.EmptyContacts.FindAsync(id);
            return contact == null ? throw new KeyNotFoundException("Contact not found") : contact;
        }

        public async Task<EmptyContact> CreateContact(EmptyContact contact)
        {
            _context.EmptyContacts.Add(contact);
            await _context.SaveChangesAsync();
            return contact;
        }

        public async Task UpdateContact(EmptyContact contact)
        {
            _context.Entry(contact).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteContact(int id)
        {
            var contact = await _context.EmptyContacts.FindAsync(id);
            if (contact != null)
            {
                _context.EmptyContacts.Remove(contact);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<EmptyContact>> SearchContacts(string query)
        {
            var searchQuery = query.ToLower();

            return await _context.EmptyContacts
                .Where(c => c.FirstName.ToLower().Contains(searchQuery) ||
                            c.LastName.ToLower().Contains(searchQuery) ||
                            c.Email.ToLower().Contains(searchQuery) ||
                            (c.FirstName + " " + c.LastName).ToLower().Contains(searchQuery))
                .ToListAsync();
        }



        public async Task<IEnumerable<EmptyContact>> GetFilteredContacts(
    string? name = null, 
    string? city = null, 
    string? state = null,
    string? sortBy = null,
    string? order = null)
{
    var query = _context.EmptyContacts.AsQueryable();

            // Existing filtering logic
            if (!string.IsNullOrEmpty(name))
                query = query.Where(c => c.FirstName.Contains(name) ||
                    c.LastName.Contains(name) ||
                    (c.FirstName + " " + c.LastName).Contains(name));
            if (!string.IsNullOrEmpty(city))
        query = query.Where(c => c.City.Contains(city));
    if (!string.IsNullOrEmpty(state))
        query = query.Where(c => c.State == state);

    // Add sorting
    if (!string.IsNullOrEmpty(sortBy))
    {
        query = sortBy.ToLower() switch
        {
            "name" => order?.ToLower() == "desc" ?
    query.OrderByDescending(c => c.LastName).ThenByDescending(c => c.FirstName) :
    query.OrderBy(c => c.LastName).ThenBy(c => c.FirstName),
            "city" => order?.ToLower() == "desc" ? query.OrderByDescending(c => c.City) : query.OrderBy(c => c.City),
            "state" => order?.ToLower() == "desc" ? query.OrderByDescending(c => c.State) : query.OrderBy(c => c.State),
            _ => query.OrderBy(c => c.Id)
        };
    }


    
    return await query.ToListAsync();
}

        public async Task<PagedResult<EmptyContact>> GetFilteredContactsPaged(
    string? name = null,
    string? city = null,
    string? state = null,
    string? sortBy = null,
    string? order = null,
    int pageNumber = 1,
    int pageSize = 10)
        {
            // Start with queryable
            var query = _context.EmptyContacts.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(name))
                query = query.Where(c => c.FirstName.Contains(name) ||
                                         c.LastName.Contains(name) ||
                                         (c.FirstName + " " + c.LastName).Contains(name));
            if (!string.IsNullOrEmpty(city))
                query = query.Where(c => c.City.Contains(city));
            if (!string.IsNullOrEmpty(state))
                query = query.Where(c => c.State == state);

            // Apply sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                query = sortBy.ToLower() switch
                {
                    "name" => order?.ToLower() == "desc" ?
    query.OrderByDescending(c => c.LastName).ThenByDescending(c => c.FirstName) :
    query.OrderBy(c => c.LastName).ThenBy(c => c.FirstName),
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

            return new PagedResult<EmptyContact>
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
                var property = typeof(EmptyContact).GetProperty(field.Key);
                if (property != null)
                {
                    property.SetValue(contact, Convert.ChangeType(field.Value, property.PropertyType));
                }
            }

            await _context.SaveChangesAsync();
        }

        public bool ContactExists(int id)
        {
            return _context.EmptyContacts.Any(e => e.Id == id);
        }
    }
}