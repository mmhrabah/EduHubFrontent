using System;
using System.Collections.Generic;
using System.Linq;
using Rabah.Utils.Session;

namespace Rabah.GeneralDataModel
{
    [Serializable]
    public class Content
    {
        public Guid Id;
        public string Name;
        public string Description;
        public string ImageUrl;
        public string Author;
        public string Publisher;
        public string Link;
        public string Version;
        public DateTime CreatedAt;
        public DateTime? UpdatedAt;
        public Guid CategoryId;
        public Guid TypeId;
    }

    public class ContentTypeMapping
    {
        public static string GetContentTypeName(Guid typeId)
        {
            var conentType = Session.ContentTypes.FirstOrDefault((ct) => typeId.Equals(ct.Id));
            return conentType == null ? "Unknown Type" : conentType.Name;
        }
    }
}