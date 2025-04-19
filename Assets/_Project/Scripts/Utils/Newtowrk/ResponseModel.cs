using System;

namespace Rabah.Utils.Network
{
    /// <summary>
    /// This class is used to manage the response model for the API requests.
    /// </summary>
    [Serializable]
    public class ResponseModel<T>
    {
        public int StatusCode;
        public T Data;
    }
}