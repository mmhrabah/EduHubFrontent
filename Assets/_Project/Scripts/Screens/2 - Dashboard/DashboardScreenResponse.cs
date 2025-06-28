using System;
using System.Collections.Generic;
using Rabah.GeneralDataModel;
namespace Rabah.Screens
{

    [Serializable]
    public class DashboardScreenResponse : RequestModel
    {
        public int totalContentItems;
        public int activeSubscriptions;
        public int contentAddedThisMonth;
        public List<Content> recentlyAddedContent;
    }
}