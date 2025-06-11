using Microsoft.AspNetCore.Mvc;
using ContactManagementAPI.Services;
using ContactManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using ContactManagementAPI.Models.Pagination;
using Microsoft.AspNetCore.Authorization;


namespace ContactManagementAPI.Controllers
{
    /// <summary>
    /// Controller for managing empty contacts in API version 1.0
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/emptyContacts")]
    [ApiController]
    public class EmptyContactsController : ControllerBase
    {
        private readonly IEmptyContactService _emptyContactService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactsController"/> class
        /// </summary>
        /// <param name="emptyContactService">The service for contact operations</param>
        public EmptyContactsController(IEmptyContactService emptyContactService)
        {
            _emptyContactService = emptyContactService;
        }

        /// <summary>
        /// Gets all empty contacts with optional filtering and sorting
        /// </summary>
        /// <param name="name">Filter by contact name</param>
        /// <param name="city">Filter by city</param>
        /// <param name="state">Filter by state (2 letter code)</param>
        /// <param name="sortBy">Sort by field (name, city, state)</param>
        /// <param name="order">Sort order (asc or desc)</param>
        /// <returns>A list of empty contacts matching the specified criteria</returns>
        /// <response code="200">Returns the list of empty contacts</response>
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
                var contacts = await _emptyContactService.GetFilteredContacts(name, city, state, sortBy, order);
                var result = new
                {
                    Count = contacts.Count(),
                    Contacts = contacts
                };
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error occurred while retrieving empty contacts");
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
        public async Task<ActionResult<EmptyContact>> GetContact(int id)
        {
            try
            {
                var emptyContact = await _emptyContactService.GetContact(id);
                if ( emptyContact == null)
                {
                    return NotFound($"Contact with ID {id} not found");
                }
                return emptyContact;
            }
            catch (Exception)
            {
                return StatusCode(500, $"Internal server error occurred while retrieving contact {id}");
            }
        }

        /// <summary>
        /// Gets a paged list of empty contacts with optional filtering and sorting
        /// </summary>
        /// <param name="name">Filter by contact name</param>
        /// <param name="city">Filter by city</param>
        /// <param name="state">Filter by state (2 letter code)</param>
        /// <param name="sortBy">Sort by field (name, city, state)</param>
        /// <param name="order">Sort order (asc or desc)</param>
        /// <param name="pageNumber">Page number (starts from 1)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <returns>A paged result containing empty contacts and pagination metadata</returns>
        /// <response code="200">Returns the paged list of empty contacts</response>
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
                var contacts = await _emptyContactService.GetFilteredContactsPaged(
                    name, city, state, sortBy, order, pageNumber, pageSize);
                return Ok(contacts);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error occurred while retrieving empty contacts");
            }
        }

        /// <summary>
        /// Searches empty contacts by name or email.
        /// </summary>
        /// <param name="query">The search query string</param>
        /// <returns>List of matching empty contacts</returns>
        /// <response code="200">Returns the list of matching empty contacts</response>
        /// <response code="400">If the search query is empty</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user doesn't have permission</response>
        /// <response code="500">If there was an internal server error</response>
        [Authorize(Policy = "UserPolicy")]
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Contact>>> SearchContacts([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query cannot be empty.");
            }

            try
            {
                var contacts = await _emptyContactService.SearchContacts(query);
                return Ok(contacts);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error occurred while searching for empty contacts.");
            }
        }


        /// <summary>
        /// Creates a new contact
        /// </summary>
        /// <param name="emptyContact">The contact data to create</param>
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
        public async Task<ActionResult<Contact>> PostContact(EmptyContact emptyContact)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var createdContact = await _emptyContactService.CreateContact(emptyContact);
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
        /// <param name="emptyContact">The updated contact data</param>
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
        public async Task<IActionResult> PutContact(int id, EmptyContact emptyContact)
        {
            if (id != emptyContact.Id)
            {
                return BadRequest("ID in URL does not match ID in data");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (!_emptyContactService.ContactExists(id))
                {
                    return NotFound($"Contact with ID {id} not found");
                }
                await _emptyContactService.UpdateContact(emptyContact);
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
                if (!_emptyContactService.ContactExists(id))
                {
                    return NotFound($"Contact with ID {id} not found");
                }
                await _emptyContactService.DeleteContact(id);
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
                await _emptyContactService.UpdateContactPartial(id, patchValues);
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