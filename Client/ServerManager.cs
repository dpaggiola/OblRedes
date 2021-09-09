using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Common;
using ICommon;
using IServices;
using Services;

namespace Server
{
    public static class ClientManager
    {
        public static void DisplayMenu(FrameHandler frameHandler, string username)
        {
            SendRequest(frameHandler, CommandType.MU, username);
        }

        public static void RegisterUser(FrameHandler frameHandler)
        {
            Console.WriteLine("Ingresar usuario");
            var username = "";
            while (username == "") username = Console.ReadLine();
            Console.WriteLine("Ingresar contrasena");
            var password = "";
            while (password == "") password = Console.ReadLine();
            var data = username + "@" + password;
            SendRequest(frameHandler, CommandType.AU, data);
        }

        public static async Task<string> LoginUser(FrameHandler frameHandler)
        {
            Console.WriteLine("Ingresar usuario");
            var username = "";
            while (username == "") username = Console.ReadLine();
            Console.WriteLine("Ingresar contrasena");
            var password = "";
            while (password == "") password = Console.ReadLine();
            var data = username + "@" + password;
            var headerStructure = new HeaderStructure(FlagType.REQ, CommandType.SU, data.Length);
            ICodification<HeaderStructure> header = new Header(headerStructure);
            var exitFrame = header.Encode(headerStructure).Concat(Encoding.UTF8.GetBytes(data)).ToArray();
            var tarea = frameHandler.SendMessageAsync(exitFrame);

            var serverResponse = await frameHandler.ReadDataAsync().ConfigureAwait(false);
            var asciiResponse = Encoding.ASCII.GetString(serverResponse);
            if (username == asciiResponse) return asciiResponse;

            Console.WriteLine(asciiResponse);
            return "";
        }

        public static void DisplayUsers(FrameHandler frameHandler)
        {
            SendRequest(frameHandler, CommandType.LU, "");
        }

        public static async void AddPhoto(FrameHandler frameHandler, string username, NetworkStream networkStream)
        {
            var fileFunctions = new FileFunctions();
            Console.WriteLine("Ingresar ubicacion de la foto");
            var path = "";
            while (!fileFunctions.FileExists(path)) path = Console.ReadLine();
            var data = username + "@" + path;

            var headerStructure = new HeaderStructure(FlagType.REQ, CommandType.AF, data.Length);
            ICodification<HeaderStructure> header = new Header(headerStructure);
            var exitFrame = header.Encode(headerStructure).Concat(Encoding.UTF8.GetBytes(data)).ToArray();
            var tarea = frameHandler.SendMessageAsync(exitFrame);

            IFileHandler fileHandler = new FileHandler(networkStream);
            IFileService fileService = new FileService(fileFunctions, fileHandler);
            fileService.SendFile(path);
            var serverResponse = await frameHandler.ReadDataAsync().ConfigureAwait(false);
            var asciiResponse = Encoding.ASCII.GetString(serverResponse);
            Console.WriteLine(asciiResponse);
        }

        public static void DisplayPhotos(FrameHandler frameHandler, string username)
        {
            SendRequest(frameHandler, CommandType.LF, username);
        }

        public static void DisplayComments(FrameHandler frameHandler, string username)
        {
            Console.WriteLine("Ingresar nombre de la foto");
            var photo = "";
            while (photo == "") photo = Console.ReadLine();
            var data = username + "@" + photo;
            SendRequest(frameHandler, CommandType.LC, data);
        }

        public static void AddComment(FrameHandler frameHandler, string username)
        {
            Console.WriteLine("Ingresar nombre de la foto");
            var photo = "";
            while (photo == "") photo = Console.ReadLine();
            Console.WriteLine("Ingresar comentario");
            var comment = "";
            while (comment == "") comment = Console.ReadLine();
            var data = username + "@" + photo + "@" + comment;
            SendRequest(frameHandler, CommandType.AC, data);
        }

        public static void Exit(FrameHandler frameHandler)
        {
            SendRequest(frameHandler, CommandType.FF, "");
        }

        public static async void SendRequest(FrameHandler frameHandler, CommandType commandType, string data)
        {
            var headerStructure = new HeaderStructure(FlagType.REQ, commandType, data.Length);
            ICodification<HeaderStructure> header = new Header(headerStructure);
            var exitFrame = header.Encode(headerStructure).Concat(Encoding.UTF8.GetBytes(data)).ToArray();
            var tarea = frameHandler.SendMessageAsync(exitFrame);

            var serverResponse = await frameHandler.ReadDataAsync().ConfigureAwait(false);
            var asciiResponse = Encoding.ASCII.GetString(serverResponse);
            Console.WriteLine(asciiResponse);
        }
    }
}