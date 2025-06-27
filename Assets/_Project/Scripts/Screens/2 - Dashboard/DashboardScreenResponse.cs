namespace Rabah.Screens
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class DashboardScreenResponse : RequestModel
    {
        public List<string> feeds;
        public List<string> notifications;
    }
}