using EricTest.Models;
using EricTest.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EricTest.Controllers
{
    public class AdminController : Controller
    {
        private readonly AchievementService _achievementService;

        public AdminController(AchievementService achievementService)
        {
            _achievementService = achievementService;
        }

        // GET: /Admin/Achievements
        public IActionResult Achievements()
        {
            var achievements = _achievementService.GetAllAchievements();
            return View(achievements);
        }

        // GET: /Admin/CreateAchievement
        public IActionResult CreateAchievement()
        {
            return View(new CreateAchievementViewModel
            {
                IssuerId = "did:web:openbadges.local:issuers:main"
            });
        }

        // GET: /Admin/ViewAchievement/{id}
        public IActionResult ViewAchievement(string id)
        {
            var achievement = _achievementService.GetAchievement(id);
            if (achievement == null)
            {
                return NotFound();
            }

            return View(achievement);
        }

        //// POST: /Admin/CreateAchievement
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult CreateAchievement(CreateAchievementViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var achievement = _achievementService.CreateAchievement(
        //            model.Name,
        //            model.Description,
        //            model.ImageUrl,
        //            new Criteria { Narrative = model.CriteriaNarrative },
        //            model.IssuerDid);

        //        return RedirectToAction("Achievements");
        //    }

        //    return View(model);
        //}


    }
}
