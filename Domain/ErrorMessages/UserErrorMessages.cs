namespace Domain.ErrorMessages
{
    public class UserErrorMessages
    {
        public const string EMPTY_USERNAME = "El campo nombre de usuario no puede estar vacio";
        public const string EMPTY_PASSWORD = "El campo contrasena no puede estar vacio";

        public const string INCORRECT_USERNAME_LENGTH =
            "El nombre de usuario debe contener tener un largo de entre 4 y 20 caracteres";

        public const string INCORRECT_PASSWORD_LENGTH =
            "La contrasena debe contener tener un largo de entre 4 y 20 caracteres";

        public const string INCORRECT_FORMAT_USERNAME = "El nombre de usuario debe contener solo numeros y letras";
        public const string INCORRECT_FORMAT_PASSWORD = "La contrasena debe contener solo numeros y letras";

        public const string NO_IMAGE = "Debes seleccionar una imagen para registrarte";
    }
}