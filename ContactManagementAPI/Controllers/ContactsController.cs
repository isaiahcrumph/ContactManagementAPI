using Microsoft.AspNetCore.Mvc;
using ContactManagementAPI.Services;
using ContactManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using ContactManagementAPI.Models.Pagination;
using Microsoft.AspNetCore.Authorization;

namespace ContactManagementAPI.Controllers
{
    /// <summary>
    /// Controller for managing contacts in API version 1.0
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/contacts")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactsController"/> class
        /// </summary>
        /// <param name="contactService">The service for contact operations</param>
        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        /// <summary>
        /// Gets all contacts with optional filtering and sorting
        /// </summary>
        /// <param name="name">Filter by contact name</param>
        /// <param name="city">Filter by city</param>
        /// <param name="state">Filter by state (2 letter code)</param>
        /// <param name="sortBy">Sort by field (name, city, state)</param>
        /// <param name="order">Sort order (asc or desc)</param>
        /// <returns>A list of contacts matching the specified criteria</returns>
        /// <response code="200">Returns the list of contacts</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user doesn't have permission</response>
        /// <response code="500">If there was an internal server error</response>
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

        /// <summary>
        /// Gets a specific contact by ID
        /// </summary>
        /// <param name="id">The ID of the contact to retrieve</param>
        /// <returns>The contact details</returns>
        /// <response code="200">Returns the contact</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user doesn't have permission</response>
        /// <response code="404">If the contact doesn't exist</response>
        /// <response code="500">If there was an internal server error</response>
        [Authorize(Policy = "UserPolicy")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

        /// <summary>
        /// Gets a paged list of contacts with optional filtering and sorting
        /// </summary>
        /// <param name="name">Filter by contact name</param>
        /// <param name="city">Filter by city</param>
        /// <param name="state">Filter by state (2 letter code)</param>
        /// <param name="sortBy">Sort by field (name, city, state)</param>
        /// <param name="order">Sort order (asc or desc)</param>
        /// <param name="pageNumber">Page number (starts from 1)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <returns>A paged result containing contacts and pagination metadata</returns>
        /// <response code="200">Returns the paged list of contacts</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user doesn't have permission</response>
        /// <response code="500">If there was an internal server error</response>
        [Authorize(Policy = "UserPolicy")]
        [HttpGet("paged")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

        /// <summary>
        /// Creates a new contact
        /// </summary>
        /// <param name="contact">The contact data to create</param>
        /// <returns>The created contact with its assigned ID</returns>
        /// <response code="201">Returns the newly created contact</response>
        /// <response code="400">If the contact data is invalid</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user doesn't have permission</response>
        /// <response code="500">If there was an internal server error</response>
        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
                return CreatedAtAction(nameof(GetContact), new { id = createdContact.Id }, createdContact);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error occurred while creating the contact");
            }
        }

        /// <summary>
        /// Updates an existing contact
        /// </summary>
        /// <param name="id">The ID of the contact to update</param>
        /// <param name="contact">The updated contact data</param>
        /// <returns>No content if successful</returns>
        /// <response code="204">If the contact was successfully updated</response>
        /// <response code="400">If the contact data is invalid or ID mismatch</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user doesn't have permission</response>
        /// <response code="404">If the contact doesn't exist</response>
        /// <response code="500">If there was an internal server error</response>
        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Deletes a contact
        /// </summary>
        /// <param name="id">The ID of the contact to delete</param>
        /// <returns>No content if successful</returns>
        /// <response code="204">If the contact was successfully deleted</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user doesn't have permission</response>
        /// <response code="404">If the contact doesn't exist</response>
        /// <response code="500">If there was an internal server error</response>
        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

        /// <summary>
        /// Partially updates a contact
        /// </summary>
        /// <param name="id">The ID of the contact to update</param>
        /// <param name="patchValues">The values to update</param>
        /// <returns>No content if successful</returns>
        /// <response code="204">If the contact was successfully updated</response>
        /// <response code="400">If the patch data is invalid</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user doesn't have permission</response>
        /// <response code="404">If the contact doesn't exist</response>
        /// <response code="500">If there was an internal server error</response>
        [Authorize(Policy = "AdminPolicy")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
    }
}