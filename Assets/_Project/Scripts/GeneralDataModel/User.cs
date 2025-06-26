using System;

namespace Rabah.GeneralDataModel
{
    [Serializable]
    public class User
    {
        public Guid Id;
        public string Username;
        public string Password;
        public string Email;
        public string PhoneNumber;
        public DateTime DateOfBirth;
        public string ProfilePictureUrl;
        public DateTime CreatedAt;
        public DateTime UpdatedAt;
        public string AccessToken;
    }
}