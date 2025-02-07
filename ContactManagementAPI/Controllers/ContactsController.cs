using Microsoft.AspNetCore.Mvc;
using ContactManagementAPI.Services;
using ContactManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using ContactManagementAPI.Models.Pagination;
using Microsoft.AspNetCore.Authorization;

namespace ContactManagementAPI.Controllers
{
    [Authorize]
    [Route("api/contacts")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;
        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<object>> GetContacts(
    [FromQuery] string? name = null,
    [FromQuery] string? city = null,
    [FromQuery] string? state = null,
    [FromQuery] string? sortBy = null,
    [FromQuery] string? order = null)
        {
            try
            {
                var contacts = await _contactService.GetFilteredContacts(name, city, state, sortBy, order);
                var result = new
                {
                    Count = contacts.Count(),
                    Contacts = contacts
                };
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error occurred while retrieving contacts");
            }
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            try
            {
                var contact = await _contactService.GetContact(id);
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

       // /// <summary>
       // /// Gets a paged list of contacts with optional filtering and sorting
       // /// </summary>
       // /// <param name="name">Filter by contact name</param>
       // /// <param name="city">Filter by city</param>
       // /// <param name="state">Filter by state (2 letter code)</param>
       // /// <param name="sortBy">Sort by field (name, city, state)</param>
       // /// <param name="order">Sort order (asc or desc)</param>
       // /// <param name="pageNumber">Page number (starts from 1)</param>
       // /// <param name="pageSize">Number of items per page</param>

        [HttpGet("paged")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<Contact>>> GetContactsPaged(
    [FromQuery] string? name = null,
    [FromQuery] string? city = null,
    [FromQuery] string? state = null,
    [FromQuery] string? sortBy = null,
    [FromQuery] string? order = null,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
        {
            try
            {
                var contacts = await _contactService.GetFilteredContactsPaged(
                    name, city, state, sortBy, order, pageNumber, pageSize);
                return Ok(contacts);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error occurred while retrieving contacts");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Contact>> PostContact(Contact contact)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var createdContact = await _contactService.CreateContact(contact);
                return CreatedAtAction(nameof(GetContact), new { id = createdContact.Id }, createdContact);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error occurred while creating the contact");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
            try
            {
                if (!_contactService.ContactExists(id))
                {
                    return NotFound($"Contact with ID {id} not found");
                }
                await _contactService.UpdateContact(contact);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, $"Internal server error occurred while updating contact {id}");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteContact(int id)
        {
            try
            {
                if (!_contactService.ContactExists(id))
                {
                    return NotFound($"Contact with ID {id} not found");
                }
                await _contactService.DeleteContact(id);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, $"Internal server error occurred while deleting contact {id}");
            }
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchContact(int id, [FromBody] Dictionary<string, object> patchValues)
        {
            try
            {
                await _contactService.UpdateContactPartial(id, patchValues);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Contact with ID {id} not found");
            }
            catch (Exception)
            {
                return StatusCode(500, $"Internal server error occurred while updating contact {id}");
            }
        }
    }
}