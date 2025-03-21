using System.ComponentModel.DataAnnotations;

namespace OnePrivateNavigation.Common.Models.Website
{
    public class WebsiteRequest
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Url]
        public string Url { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int DisplayOrder { get; set; } = 0;

        public bool IsVisible { get; set; } = true;

        [Required]
        public int CategoryId { get; set; }
    }
}