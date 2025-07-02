using System.Collections.Generic;
using Rabah.GeneralDataModel;
using Rabah.Utils.Network;
using Rabah.Utils.UI;
using UnityEngine;

namespace Rabah.Screens
{
    public class CategoriesAndTypesScreen :
                 ScreenFetchAndSendData<ResponseModel<List<Category>>, List<Category>,
                 CategoriesRequestModels, ResponseModel<List<Category>>, List<Category>>
    {
        protected override CategoriesRequestModels ExtractDataFromInputs()
        {
            throw new System.NotImplementedException();
        }

        protected override void FillUIElementsInputs()
        {
            throw new System.NotImplementedException();
        }
    }
}