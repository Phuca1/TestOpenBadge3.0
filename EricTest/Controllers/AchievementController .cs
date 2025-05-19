using EricTest.DTO;
using EricTest.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

namespace EricTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AchievementController : ControllerBase
    {
        private readonly AchievementService _achievementService;
        private readonly ImageService _imageService;

        public AchievementController(AchievementService achievementService, ImageService imageService)
        {
            _achievementService = achievementService;
            _imageService = imageService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var achievements = _achievementService.GetAllAchievements();
            return Ok(achievements);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var achievement = _achievementService.GetAchievement(id);
            if (achievement == null)
                return NotFound();

            return Ok(achievement);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromForm] CreateAchievementRequest request)
        {
            try
            {
                if (request.ImageFile != null)
                {
                    var imageUrl = await _imageService.SaveImage(request.ImageFile);
                    request.ImageUrl = imageUrl;
                }

                var achievement = _achievementService.CreateAchievement(request);
                return Created(achievement.Id, achievement);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}/json")]
        public IActionResult GetAchievementJson(string id)
        {
            var achievement = _achievementService.GetAchievement(id);
            if (achievement == null)
                return NotFound();

            var options = new JsonSerializerOptions { WriteIndented = true };
            return Content(JsonSerializer.Serialize(achievement, options), "application/json");
        }
    }
}
