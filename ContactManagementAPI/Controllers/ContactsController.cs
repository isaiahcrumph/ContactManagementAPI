using Microsoft.AspNetCore.Mvc;
using ContactManagementAPI.Data;
using Microsoft.EntityFrameworkCore;
namespace ContactManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ContactDbContext _context;
        public ContactsController(ContactDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
        {
            try
            {
                var contacts = await _context.Contacts.ToListAsync();
                return Ok(contacts);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error occurred while retrieving contacts");
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            try
            {
                var contact = await _context.Contacts.FindAsync(id);
                if (contact == null)
                {
                    return NotFound($"Contact with ID {id} not found");
                }
                return contact;
            }
            catch (Exception)
            {
                return StatusCode(500, $"Internal server error occurred while retrieving contact {id}");
            }
        }
        [HttpPost]
        public async Task<ActionResult<Contact>> PostContact(Contact contact)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error occurred while creating the contact");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(int id, Contact contact)
        {
            if (id != contact.Id)
            {
                return BadRequest("ID in URL does not match ID in data");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Entry(contact).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
                {
                    return NotFound($"Contact with ID {id} not found");
                }
                throw;
            }
            catch (Exception)
            {
                return StatusCode(500, $"Internal server error occurred while updating contact {id}");
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            try
            {
                var contact = await _context.Contacts.FindAsync(id);
                if (contact == null)
                {
                    return NotFound($"Contact with ID {id} not found");
                }
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, $"Internal server error occurred while deleting contact {id}");
            }
        }
        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }
    }
}