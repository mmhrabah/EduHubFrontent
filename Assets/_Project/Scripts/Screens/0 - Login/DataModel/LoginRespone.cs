using System;

namespace Rabah.Screens
{
    [Serializable]
    public class LoginResponse : RequestModel
    {
        public Guid Id;
        public string Username;
        public string Password;
    }
}