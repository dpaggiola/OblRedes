using System;
using System.Runtime.Serialization;

namespace Exception
{
    [Serializable]
    public class UserDoesNotExistsException : System.Exception
    {
        public UserDoesNotExistsException()
        {
        }

        public UserDoesNotExistsException(string message) : base(message)
        {
        }

        public UserDoesNotExistsException(string message, System.Exception innerException) : base(message,
            innerException)
        {
        }

        protected UserDoesNotExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}