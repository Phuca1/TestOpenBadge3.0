namespace EricTest.DTO
{
    public class CreateIssuerProfileRequest
    {
        public string Name { get; set; }
        public string Domain { get; set; }
        public string PathPrefix { get; set; } = "issuers";
        public string? Url { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
    }
}
