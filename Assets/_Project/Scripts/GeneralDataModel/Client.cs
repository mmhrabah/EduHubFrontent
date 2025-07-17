using System;
using System.Collections.Generic;

namespace Rabah.GeneralDataModel
{
    public class Client
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public SubscriptionStatus SubscriptionStatus { get; set; }
        public UserStatus Status { get; set; }
        public DateTime SubscriptionStartDate { get; set; }
        public DateTime SubscriptionEndDate { get; set; }
        public List<string> MacAddresses { get; set; } = new List<string>();

        public string GetSubscriptionStatusText()
        {
            return SubscriptionStatus switch
            {
                SubscriptionStatus.Subscribed => "Subscribed",
                SubscriptionStatus.LastMonthOfSubscription => "Last Month of Subscription",
                SubscriptionStatus.GraceMonth => "Grace Month",
                SubscriptionStatus.Unsubscribed => "Unsubscribed",
                _ => "Unknown Status"
            };
        }
    }
}