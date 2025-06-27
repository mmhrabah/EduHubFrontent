using System;

namespace Rabah.Screens
{
    [Serializable]
    public class LoginDataModelRequest : RequestModel
    {
        public string name;
        public string Password;
    }
}