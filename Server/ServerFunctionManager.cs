using System;
using DataAccess;
using Domain;
using IDataAccess;
using IServices;
using Services;

namespace Server
{
    public static class ServerFunctionManager
    {
        public static void PopulateServer()
        {
            IClientDataAccess clientDataAccess = new ClientDataAccess();
            IClientService clientService = new ClientService(clientDataAccess);
            clientService.RegisterUser("roberto", "roberto1");

            IPhotoDataAccess photoDataAccess = new PhotoDataAccess();
            IPhotoService photoService = new PhotoService(photoDataAccess);
            photoService.UploadPhoto("roberto", new Photo {Name = "foto.jpg"});

            ICommentDataAccess commentDataAccess = new CommentDataAccess();
            ICommentService commentService = new CommentService(commentDataAccess);
            commentService.UploadComment("roberto", "foto.jpg", new Comment {Content = "comentario de ejemplo"});
        }

        public static void DisplayUsers()
        {
            IClientDataAccess clientDataAccess = new ClientDataAccess();
            IClientService clientService = new ClientService(clientDataAccess);
            var clientList = clientService.GetUsersConnected();
            Console.WriteLine("Hay " + clientList.Count + " usuarios conectados");
            foreach (var client in clientList) Console.WriteLine(client.Username + " - Ultima conexion: " +client.LastConnection);
        }

        public static void RegisterUser()
        {
            IClientDataAccess clientDataAccess = new ClientDataAccess();
            IClientService clientService = new ClientService(clientDataAccess);

            Console.WriteLine("Ingresar usuario");
            var username = "";
            while (username == "") username = Console.ReadLine();

            Console.WriteLine("Ingresar contrasena");
            var password = "";
            while (password == "") password = Console.ReadLine();

            clientService.RegisterUser(username, password);
            Console.WriteLine("Registro exitoso");
        }

        public static void DeleteUser()
        {
            IClientDataAccess clientDataAccess = new ClientDataAccess();
            IClientService clientService = new ClientService(clientDataAccess);

            Console.WriteLine("Ingresar usuario");
            var username = "";
            while (username == "") username = Console.ReadLine();

            clientService.DeleteUser(username);
            Console.WriteLine("Borrado exitoso");
        }

        public static void UpdateUser()
        {
            IClientDataAccess clientDataAccess = new ClientDataAccess();
            IClientService clientService = new ClientService(clientDataAccess);

            Console.WriteLine("Ingresar usuario a modificar");
            var oldUsername = "";
            while (oldUsername == "") oldUsername = Console.ReadLine();

            Console.WriteLine("Ingresar usuario nuevo");
            var username = "";
            while (username == "") username = Console.ReadLine();

            Console.WriteLine("Ingresar contrasena");
            var password = "";
            while (password == "") password = Console.ReadLine();

            clientService.UpdateUser(oldUsername,username, password);
            Console.WriteLine("Se actualizo correctamente.");
        }

        public static void DisplayUserPhotos()
        {
            IPhotoDataAccess photoDataAccess = new PhotoDataAccess();
            IPhotoService photoService = new PhotoService(photoDataAccess);
            Console.WriteLine("Ingresar usuario");
            var username = "";
            while (username == "") username = Console.ReadLine();

            var list = photoService.GetPhotos(username);
            Console.WriteLine("Hay " + list.Count + " fotos del usuario " + username);
            foreach (var photo in list) Console.WriteLine(photo.Name);
        }

        public static void AddUserPhotos()
        {
            IPhotoDataAccess photoDataAccess = new PhotoDataAccess();
            IPhotoService photoService = new PhotoService(photoDataAccess);
            Console.WriteLine("Ingresar usuario");
            var username = "";
            while (username == "") username = Console.ReadLine();

            Console.WriteLine("Ingresar ruta de la foto");
            var photo = "";
            while (photo == "") photo = Console.ReadLine();

            var newPhoto = new Photo();
            newPhoto.Name = photo;
            photoService.UploadPhoto(username, newPhoto);
            Console.WriteLine("Registro exitoso");
        }

        public static void DisplayComments()
        {
            ICommentDataAccess commentDataAccess = new CommentDataAccess();
            ICommentService commentService = new CommentService(commentDataAccess);
            Console.WriteLine("Ingresar usuario");
            var username = "";
            while (username == "") username = Console.ReadLine();

            Console.WriteLine("Ingresar foto");
            var photo = "";
            while (photo == "") photo = Console.ReadLine();

            var list = commentService.GetComments(username, photo);
            Console.WriteLine("Hay " + list.Count + " comentarios en la foto " + photo + " del usuario " +
                              username);
            foreach (var comment in list) Console.WriteLine(comment.Content);
        }

        public static void AddUserComment()
        {
            ICommentDataAccess commentDataAccess = new CommentDataAccess();
            ICommentService commentService = new CommentService(commentDataAccess);
            Console.WriteLine("Ingresar usuario");
            var username = "";
            while (username == "") username = Console.ReadLine();

            Console.WriteLine("Ingresar foto");
            var photo = "";
            while (photo == "") photo = Console.ReadLine();

            Console.WriteLine("Ingresar comentario");
            var comment = "";
            while (comment == "") comment = Console.ReadLine();

            var newComment = new Comment();
            newComment.Content = comment;
            commentService.UploadComment(username, photo, newComment);

            Console.WriteLine("Registro exitoso");
        }
    }
}