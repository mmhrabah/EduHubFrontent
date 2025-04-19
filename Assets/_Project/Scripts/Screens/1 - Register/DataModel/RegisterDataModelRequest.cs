using System;

namespace Rabah.Screens
{
    [Serializable]
    public class RegisterDataModelRequest : RequestModel
    {
        public Guid Id;
        public string Username;
        public string Password;
    }
}
