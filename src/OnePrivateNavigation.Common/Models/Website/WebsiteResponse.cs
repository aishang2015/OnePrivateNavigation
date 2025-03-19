using System.ComponentModel.DataAnnotations;

namespace OnePrivateNavigation.Common.Models.Website
{
    public class WebsiteResponse
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Icon { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsVisible { get; set; }

        public int CategoryId { get; set; }
    }
}