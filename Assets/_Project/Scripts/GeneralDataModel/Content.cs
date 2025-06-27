using System;

namespace Rabah.GeneralDataModel
{
    public class Content
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string Link { get; set; }
        public string Version { get; set; } = "1.0";

        public Guid TypeId { get; set; }
        public ContentType Type { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}