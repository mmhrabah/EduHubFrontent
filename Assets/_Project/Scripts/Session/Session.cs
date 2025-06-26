using System.Collections.Generic;
using Rabah.GeneralDataModel;

namespace Rabah.Utils.Session
{
    public static class Session
    {
        public static string AccessToken { get; set; } = string.Empty;
        public static string RefreshToken { get; set; } = string.Empty;
        public static User User { get; set; } = new();
    }
}