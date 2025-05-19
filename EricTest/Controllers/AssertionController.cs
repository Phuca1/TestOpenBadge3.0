using EricTest.DTO;
using EricTest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EricTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssertionController : ControllerBase
    {
        private readonly CredentialIssuerService _assertionService;

        public AssertionController(CredentialIssuerService assertionService)
        {
            _assertionService = assertionService;
        }

        [HttpPost]
        public IActionResult Issue([FromBody] IssueBadgeRequest request)
        {
            try
            {
                var assertion = _assertionService.IssueBadge(request);
                return Created(assertion.Id, assertion);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }
    }
}
