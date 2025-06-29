using System;

namespace Rabah.GeneralDataModel
{
    [Serializable]
    public class ContentType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FileExtension { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}