using System;

namespace Exception
{
    [Serializable]
    public class UserException : System.Exception
    {
        public UserException()
        {
        }

        public UserException(string message) : base(message)
        {
        }
    }
}