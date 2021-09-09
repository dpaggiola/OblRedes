using System;
using System.Runtime.Serialization;

namespace Exception
{
    [Serializable]
    public class PhotoDoesNotExistsException : System.Exception
    {
        public PhotoDoesNotExistsException()
        {
        }

        public PhotoDoesNotExistsException(string message) : base(message)
        {
        }

        public PhotoDoesNotExistsException(string message, System.Exception innerException) : base(message,
            innerException)
        {
        }

        protected PhotoDoesNotExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}