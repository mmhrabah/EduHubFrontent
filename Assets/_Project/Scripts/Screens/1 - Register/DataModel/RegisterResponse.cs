using System;

namespace Rabah.Screens
{
    [Serializable]
    public class RegisterResponse : RequestModel
    {
        public Guid Id;
        public string Username;
        public string Password;
        public string Email;
        public string ProfilePictureUrl;
        public string AccessToken;

    }
}