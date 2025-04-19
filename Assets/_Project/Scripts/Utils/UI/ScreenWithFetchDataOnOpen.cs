namespace Rabah.Utils.UI
{
    /// <summary>
    /// This class is used to manage the screen that fetches data when opened.
    /// </summary>
    public abstract class ScreenWithFetchDataOnOpen : Screen
    {
        public override void OnOpen(ScreenData data)
        {
            base.OnOpen(data);
            FetchData();
        }

        protected abstract void FetchData();
    }
}