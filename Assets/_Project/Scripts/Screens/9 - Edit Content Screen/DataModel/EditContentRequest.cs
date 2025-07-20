using System;
using System.Collections.Generic;

namespace Rabah.Screens
{
    [Serializable]
    public class EditContentRequest : RequestModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public Guid TypeId { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string Link { get; set; }
        public string Version { get; set; }
        public Guid CategoryId { get; set; }
    }
}
