using System.Net.Sockets;
using Common;
using DataAccess;
using Domain;
using ICommon;
using IDataAccess;
using IServices;
using Services;

namespace Server
{
    public static class ServerManager
    {
        public static User DisplayMenu(byte[] frame)
        {
            IClientDataAccess clientDataAccess = new ClientDataAccess();
            IClientService clientService = new ClientService(clientDataAccess);
            IParser parser = new Parser();

            var data = parser.GetDataObject(frame);
            if (data.Length > 0) return clientService.GetUser(data[0]);
            return null;
        }

        public static void RegisterUser(byte[] frame)
        {
            IClientDataAccess clientDataAccess = new ClientDataAccess();
            IClientService clientService = new ClientService(clientDataAccess);
            IParser parser = new Parser();

            var data = parser.GetDataObject(frame);
            clientService.RegisterUser(data[0], data[1]);
        }

        public static User LoginUser(byte[] frame)
        {
            IClientDataAccess clientDataAccess = new ClientDataAccess();
            IClientService clientService = new ClientService(clientDataAccess);
            IParser parser = new Parser();

            var data = parser.GetDataObject(frame);
            clientService.LoginUser(data[0], data[1]);
            return clientService.GetUser(data[0]);
        }

        public static string DisplayUsers()
        {
            IClientDataAccess clientDataAccess = new ClientDataAccess();
            IClientService clientService = new ClientService(clientDataAccess);

            var users = clientService.GetUsers();
            var result = "Usuarios :" + "\n";
            foreach (var user in users) result += user.Username + "\n";

            return result;
        }

        public static void AddPhoto(byte[] frame, NetworkStream networkStream)
        {
            var fileFunctions = new FileFunctions();
            IPhotoDataAccess photoDataAccess = new PhotoDataAccess();
            IPhotoService photoService = new PhotoService(photoDataAccess);
            IParser parser = new Parser();

            var data = parser.GetDataObject(frame);
            var photo = new Photo();
            photo.Name = fileFunctions.GetFileName(data[1]);
            photoService.UploadPhoto(data[0], photo);
            IFileHandler fileHandler = new FileHandler(networkStream);
            IFileService fileService = new FileService(fileFunctions, fileHandler);
            fileService.ReceiveFile();
        }


        public static string DisplayPhotos(byte[] frame)
        {
            IPhotoDataAccess photoDataAccess = new PhotoDataAccess();
            IPhotoService photoService = new PhotoService(photoDataAccess);
            IParser parser = new Parser();
            var data = parser.GetDataObject(frame);
            var list = photoService.GetPhotos(data[0]);
            var result = "Usuario : " + data[0] + "\n";
            foreach (var photo in list) result += "Nombre : " + photo.Name + "\n";
            return result;
        }

        public static string DisplayComments(byte[] frame)
        {
            ICommentDataAccess commentDataAccess = new CommentDataAccess();
            ICommentService commentService = new CommentService(commentDataAccess);
            IParser parser = new Parser();
            var data = parser.GetDataObject(frame);
            var list = commentService.GetComments(data[0], data[1]);
            var result = "Usuario : " + data[0] + "\n";
            result += "Foto : " + data[1] + "\n";
            if (list.Count == 0) result += "Sin comentarios \n";
            result += "Comentario : \n";
            foreach (var comment in list) result += comment.Content + "\n";
            return result;
        }

        public static void AddComment(byte[] frame)
        {
            ICommentDataAccess commentDataAccess = new CommentDataAccess();
            ICommentService commentService = new CommentService(commentDataAccess);
            IParser parser = new Parser();

            var data = parser.GetDataObject(frame);
            var comment = new Comment();
            comment.Content = data[2];
            commentService.UploadComment(data[0], data[1], comment);
        }
    }
}