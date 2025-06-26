using Rabah.Utils.Network;

namespace Rabah.Utils.UI
{
    /// <summary>
    /// This class is used to manage the screen that fetches data when opened.
    /// </summary>
    public abstract class ScreenWithFetchDataOnOpen<T, S> : Screen where T : ResponseModel<S>
    {
        public override void OnOpen(ScreenData data)
        {
            base.OnOpen(data);
            FetchData();
        }

        protected virtual void FetchData()
        {
            APIManager.Instance.Get<T>(ScreenSetupData.mainEndpoint,
                (response) =>
                {
                    OnDataFetched(response);
                    UIManager.Instance.HideLoading();
                },
                (error) =>
                {
                    OnErrorReceived(error);
                    UIManager.Instance.HideLoading();
                },
                fixResponse: true
                );
        }

        protected abstract void OnErrorReceived(string error);
        protected abstract void OnDataFetched(T response);
    }
}