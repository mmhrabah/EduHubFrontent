namespace Rabah.Screens
{
    using System;
    using System.Collections.Generic;
    using Rabah.GeneralDataModel;

    [Serializable]
    public class DashboardScreenResponse : RequestModel
    {
        public int totalContentItems;
        public int activeSubscriptions;
        public int contentAddedThisMonth;
        public List<Content> recentlyAddedContent;
    }
}