using Rabah.Utils.Network;
using Rabah.Utils.UI;
using UnityEngine;
namespace Rabah.Screens
{
    public class AddContentScreen : ScreenSendDataToDatabase<AddContentRequest, ResponseModel<RequestModel>, RequestModel>
    {

        protected override AddContentRequest ExtractDataFromInputs()
        {
            throw new System.NotImplementedException();
        }

        protected override void FillUIElementsInputs()
        {
            throw new System.NotImplementedException();
        }
    }
}