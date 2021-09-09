using System;

namespace Exception
{
    [Serializable]
    public class InvalidUserFieldException : UserException
    {
        public InvalidUserFieldException()
        {
        }

        public InvalidUserFieldException(string message) : base(message)
        {
        }
    }
}