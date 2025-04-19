using System;

namespace Rabah.Screens
{
    [Serializable]
    public class LoginDataModelRequest : RequestModel
    {
        public string Username;
        public string Password;
    }
}