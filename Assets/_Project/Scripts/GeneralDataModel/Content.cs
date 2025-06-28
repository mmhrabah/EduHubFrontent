using System;
using System.Collections.Generic;

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

        public static readonly Dictionary<Guid, ContentType> ContentTypeNames = new Dictionary<Guid, ContentType>
        {
            { Guid.Parse("a45c2e16-5da0-4f78-8ae1-0e49ebb5e1e9"), new ContentType { Id = Guid.Parse("a45c2e16-5da0-4f78-8ae1-0e49ebb5e1e9"), Name = "ePub" } },
            { Guid.Parse("88b9f521-6a10-42ee-9062-2705f39fc75e"), new ContentType { Id = Guid.Parse("88b9f521-6a10-42ee-9062-2705f39fc75e"), Name = "HTML" } },
        };
        public static string GetContentTypeName(Guid typeId)
        {
            return ContentTypeNames.ContainsKey(typeId) ? ContentTypeNames[typeId].Name : "Unknown Type";
        }
    }
}