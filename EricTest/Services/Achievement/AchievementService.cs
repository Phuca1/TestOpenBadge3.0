using EricTest.DTO;
using EricTest.Models;
using System.Text.Json;
using System.Web;
using static System.Net.WebRequestMethods;

namespace EricTest.Services
{
    public class AchievementService
    {
        private readonly IWebHostEnvironment _env;
        private static List<Achievement> _achievements = new List<Achievement>();

        public AchievementService(IWebHostEnvironment env)
        {
            _env = env;
            //InitializeSampleAchievements();
        }

        private void InitializeSampleAchievements()
        {
            // Sample issuer DID (replace with your actual issuer DID)
            var issuerDid = "did:web:example.com:issuers:main";

            _achievements.Add(new Achievement
            {
                Id = $"{GetBaseUrl()}/achievements/1",
                Name = "Python Programming Fundamentals",
                Description = "Awarded for completing the Python fundamentals course",
                ImageUrl = $"{GetBaseUrl()}/badge-images/python-badge.png",
                Criteria = new Criteria
                {
                    Narrative = "Completed all course modules and passed final project"
                },
                IssuerId = issuerDid,
                Tags = new[] { "programming", "python", "beginner" },
                Alignments = new[]
                {
                new Alignment
                {
                    TargetUrl = "https://www.python.org/doc/",
                    TargetName = "Python Documentation Standards"
                }
            }
            });

            _achievements.Add(new Achievement
            {
                Id = $"{GetBaseUrl()}/achievements/2",
                Name = "Data Science Practitioner",
                Description = "Demonstrated proficiency in core data science techniques",
                ImageUrl = $"{GetBaseUrl()}/badge-images/datascience-badge.png",
                Criteria = new Criteria
                {
                    Id = $"{GetBaseUrl()}/criteria/datascience"
                },
                IssuerId = issuerDid,
                Tags = new[] { "data-science", "machine-learning", "statistics" }
            });
        }

        public Achievement CreateAchievement(CreateAchievementRequest request)
        {
            var achievement = new Achievement
            {
                Id = $"{GetBaseUrl()}/achievements/{Guid.NewGuid()}",
                Name = request.Name,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                Criteria = request.Criteria,
                IssuerId = request.IssuerId,
                Tags = request.Tags,
                Alignments = request.Alignments
            };

            _achievements.Add(achievement);
            SaveAchievementToFile(achievement);

            return achievement;
        }

        private void SaveAchievementToFile(Achievement achievement)
        {
            var filePath = Path.Combine(_env.WebRootPath, "achievements", $"{achievement.Id.Split('/').Last()}.json");
            var json = JsonSerializer.Serialize(achievement, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(filePath, json);
        }

        public IEnumerable<Achievement> GetAllAchievements()
        {
            return _achievements.AsReadOnly();
        }

        public Achievement? GetAchievement(string id)
        {
            var realIdUrl = HttpUtility.UrlDecode(id);
            return _achievements.FirstOrDefault(a => a.Id == realIdUrl);
        }

        private string GetBaseUrl() => "https://openbadges.local";


    }
}
