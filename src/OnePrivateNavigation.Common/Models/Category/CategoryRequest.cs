using System.ComponentModel.DataAnnotations;

namespace OnePrivateNavigation.Common.Models.Category
{
    public class CategoryRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int DisplayOrder { get; set; } = 0;

        public bool IsVisible { get; set; } = true;
    }
}