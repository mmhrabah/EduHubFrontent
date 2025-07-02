using System;
using Rabah.GeneralDataModel;

namespace Rabah.Screens
{
    [Serializable]
    public class CategoriesRequestModels : RequestModel
    {
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}