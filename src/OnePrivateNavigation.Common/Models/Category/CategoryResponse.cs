using System.ComponentModel.DataAnnotations;

namespace OnePrivateNavigation.Common.Models.Category
{
    public class CategoryResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsVisible { get; set; }
    }
}