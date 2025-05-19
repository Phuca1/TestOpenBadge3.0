using EricTest.DTO;
using EricTest.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
namespace EricTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuerProfileController : ControllerBase
    {
        private readonly IssuerProfileService _issuerProfileService;
        private readonly IWebHostEnvironment _env;
        public IssuerProfileController(IssuerProfileService issuerProfileService, IWebHostEnvironment env)
        {
            _issuerProfileService = issuerProfileService;
            _env = env;
        }

        [HttpPost("create")]
        public IActionResult CreateIssuerProfile([FromBody] CreateIssuerProfileRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Invalid request data.");
            }
            var profile = _issuerProfileService.CreateProfile(request);
            return Created(profile.Id, profile);
        }

        [HttpGet("{did}")]
        public IActionResult GetIssuerProfile(string did)
        {
            var result = _issuerProfileService.GetProfile(did);
            return Content(JsonSerializer.Serialize(result), "application/json");
        }

        [HttpGet(".well-known/did.json")]
        public IActionResult GetDidDocument()
        {
            try
            {
                var path = Path.Combine(
                    _env.WebRootPath,
                    ".well-known",
                    "did.json");

                if (!System.IO.File.Exists(path))
                    return NotFound();

                var didDoc = System.IO.File.ReadAllText(path);
                return Content(didDoc, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
