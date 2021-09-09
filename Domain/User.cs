using System.Collections.Generic;
using System.Text.Json.Serialization;
using Domain.ErrorMessages;
using Exception;

namespace Domain
{
    public class User
    {
        public const int MIN_USERNAME_LENGTH = 4;
        public const int MAX_USERNAME_LENGTH = 20;
        private string _password;

        private string _username;
        [JsonIgnore]
        public virtual List<Photo> Photos { get; set; }

        public User()
        {
            Photos = new List<Photo>();
        }

        public bool IsConnected { get; set; }
        public string LastConnection { get; set; }

        public string Username
        {
            get => _username;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidUserFieldException(UserErrorMessages.EMPTY_USERNAME);
                ValidStringLength(value, UserErrorMessages.INCORRECT_USERNAME_LENGTH);
                CorrectFormat(value, UserErrorMessages.INCORRECT_FORMAT_USERNAME);
                _username = value;
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidUserFieldException(UserErrorMessages.EMPTY_PASSWORD);
                ValidStringLength(value, UserErrorMessages.INCORRECT_PASSWORD_LENGTH);
                CorrectFormat(value, UserErrorMessages.INCORRECT_FORMAT_PASSWORD);
                _password = value;
            }
        }

        private void ValidStringLength(string value, string message)
        {
            var length = value.Length;
            var isShorter = length < MIN_USERNAME_LENGTH;
            var isLonger = length > MAX_USERNAME_LENGTH;
            if (isShorter || isLonger) throw new InvalidUserFieldException(message);
        }

        private void CorrectFormat(string value, string message)
        {
            if (!HasOnlyLettersOrDigits(value)) throw new InvalidUserFieldException(message);
        }

        private bool HasOnlyLettersOrDigits(string value)
        {
            var noSpaces = true;
            foreach (var c in value)
                if (!char.IsLetterOrDigit(c))
                    noSpaces = false;
            return noSpaces;
        }
    }
}