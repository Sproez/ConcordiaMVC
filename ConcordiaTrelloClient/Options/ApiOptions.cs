using System.ComponentModel.DataAnnotations;

namespace ConcordiaTrelloClient.Options
{
    public class ApiOptions
    {
        [Required]
        public string BaseURL { get; set; } = null!;
        [Required]
        public string ConcordiaBoardID { get; set; } = null!;
        [Required]
        public string ApiKey { get; set; } = null!;
        [Required]
        public string ApiToken { get; set; } = null!;
        [Required]
        public string HighPriorityLabelId { get; set; } = null!;
        [Required]
        public string MediumPriorityLabelId { get; set; } = null!;
        [Required]
        public string LowPriorityLabelId { get; set; } = null!;

        public string ApiAuth => $"key={ApiKey}&token={ApiToken}";
    }
}