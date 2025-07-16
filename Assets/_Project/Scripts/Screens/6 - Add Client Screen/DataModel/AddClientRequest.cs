using System;
using System.Collections.Generic;

namespace Rabah.Screens
{
    [Serializable]
    public class AddClientRequest : RequestModel
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime SubscriptionStartDate { get; set; }
        public DateTime SubscriptionEndDate { get; set; }
        public List<string> MacAddresses { get; set; }
    }
}
