using System.Text.Json.Serialization;

namespace EricTest.Models
{
    public class Image
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }  // url to image
        [JsonPropertyName("type")]
        public string Type { get; set; } = "Image";
        [JsonPropertyName("caption")]
        public string Caption { get; set; }

    }
}
