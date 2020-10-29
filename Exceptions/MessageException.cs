using System;
using System.Net;

namespace RouletteApi.Exceptions
{
    public class MessageException: Exception
    {
        public int StatusCode = (int) HttpStatusCode.InternalServerError;
        public string StatusName { get; }
        public MessageException(string name, string message) : base(message)
        {
            this.StatusName = name;
        }
    }
}
