using Microsoft.AspNetCore.Mvc;
using ContactManagementAPI.Services;
using ContactManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using ContactManagementAPI.Models.Pagination;
using Microsoft.AspNetCore.Authorization;

namespace ContactManagementAPI.Controllers
{
    [Authorize]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/contacts")]
    [ApiController]
    public class ContactsV2Controller : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsV2Controller(IContactService contactService)
        {
            _contactService = contactService;
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
                // V2 enhancement: Add version info and better metadata
                var result = new
                {
                    ApiVersion = "2.0",
                    Count = contacts.Count(),
                    Filters = new { name, city, state },
                    Sorting = new { sortBy, order },
                    Contacts = contacts
                };
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error occurred while retrieving contacts");
            }
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

        [Authorize(Policy = "UserPolicy")]
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

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
                return CreatedAtAction(nameof(GetContact), new { id = createdContact.Id, version = "2.0" }, createdContact);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error occurred while creating the contact");
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
                var exists = await Task.Run(() => _contactService.ContactExists(id));
                if (!exists)
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

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteContact(int id)
        {
            try
            {
                var exists = await Task.Run(() => _contactService.ContactExists(id));
                if (!exists)
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

        [Authorize(Policy = "AdminPolicy")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

        [HttpGet("summary")]
        [Authorize(Policy = "UserPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<object>> GetContactsSummary()
        {
            try
            {
                var contacts = await _contactService.GetAllContacts();
                var summary = new
                {
                    TotalCount = contacts.Count(),
                    ByState = contacts.GroupBy(c => c.State)
                                     .Select(g => new { State = g.Key, Count = g.Count() })
                };
                return Ok(summary);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}