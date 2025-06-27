using System;

namespace Rabah.Screens
{
    [Serializable]
    public class LoginResponse : RequestModel
    {
        // public Guid Id;
        // public string Username;
        // public string Password;
        // public string Email;
        // public string PhoneNumber;
        // public DateTime DateOfBirth;
        // public string ProfilePictureUrl;
        // public string AccessToken;
        public string token;
    }
}