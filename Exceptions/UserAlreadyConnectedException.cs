using System;
using System.Runtime.Serialization;

namespace Exception
{
    [Serializable]
    public class UserAlreadyConnectedException : System.Exception
    {
        public UserAlreadyConnectedException()
        {
        }

        public UserAlreadyConnectedException(string message) : base(message)
        {
        }

        public UserAlreadyConnectedException(string message, System.Exception innerException) : base(message,
            innerException)
        {
        }

        protected UserAlreadyConnectedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}