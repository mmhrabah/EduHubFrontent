namespace Rabah.Screens
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class MainScreenResponse : RequestModel
    {
        public List<string> feeds;
        public List<string> notifications;
    }
}