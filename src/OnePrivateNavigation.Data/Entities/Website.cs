using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnePrivateNavigation.Data.Entities
{
    public class Website
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

        public string? Icon { get; set; } = string.Empty;

        public int DisplayOrder { get; set; } = 0;

        public bool IsVisible { get; set; } = false;

        public int CategoryId { get; set; }
    }
}