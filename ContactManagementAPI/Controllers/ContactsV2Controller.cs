using Microsoft.AspNetCore.Mvc;
using ContactManagementAPI.Services;
using ContactManagementAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace ContactManagementAPI.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/contacts")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Contacts V2")]
    [Produces("application/json")]
    public class ContactsV2Controller : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsV2Controller(IContactService contactService)
        {
            _contactService = contactService;
        }

        // Add a new endpoint specific to v2
        [HttpGet("summary")]
        [Authorize(Policy = "UserPolicy")]
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

        // You can also copy existing endpoints from v1 that you want to keep
        // We'll add one here as an example
        [Authorize(Policy = "UserPolicy")]
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
                // V2 enhancement: Add a message about the version
                var result = new
                {
                    ApiVersion = "2.0",
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
    }
}