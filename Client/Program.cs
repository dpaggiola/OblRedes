using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Common;
using Microsoft.Extensions.Configuration;
using Server;

namespace Client
{
    internal class Program
    {
        private static IConfiguration Configuration;

        private static async Task Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            try
            {
                var tcpClient = await ClientConnectAsync().ConfigureAwait(false);
                var keepConnection = true;
                var username = "";
                using (var networkStream = tcpClient.GetStream())
                {
                    var frameHandler = new FrameHandler(networkStream);

                    // var serverResponse = await frameHandler.ReadDataAsync().ConfigureAwait(false);
                    //  var menu = Encoding.ASCII.GetString(serverResponse);
                    //  Console.WriteLine(menu);

                    ClientManager.DisplayMenu(frameHandler, username);
                    while (keepConnection)
                    {
                        var frameToBeSent = "";
                        while (frameToBeSent == "") frameToBeSent = Console.ReadLine();

                        if (username == "")
                            switch (frameToBeSent)
                            {
                                case "1":
                                    ClientManager.RegisterUser(frameHandler);
                                    break;
                                case "2":
                                    username = ClientManager.LoginUser(frameHandler).Result;
                                    ClientManager.DisplayMenu(frameHandler, username);
                                    break;
                                case "3":
                                    ClientManager.DisplayUsers(frameHandler);
                                    break;
                                case "4":
                                    ClientManager.Exit(frameHandler);
                                    keepConnection = false;
                                    break;
                                default:
                                    Console.WriteLine(frameToBeSent + " no es un comando del servidor");
                                    ClientManager.DisplayMenu(frameHandler, username);
                                    break;
                            }
                        else
                            switch (frameToBeSent)
                            {
                                case "1":
                                    ClientManager.DisplayUsers(frameHandler);
                                    break;
                                case "2":
                                    ClientManager.AddPhoto(frameHandler, username, networkStream);
                                    break;
                                case "3":
                                    ClientManager.DisplayPhotos(frameHandler, username);
                                    break;
                                case "4":
                                    ClientManager.DisplayComments(frameHandler, username);
                                    break;
                                case "5":
                                    ClientManager.AddComment(frameHandler, username);
                                    break;
                                case "6":
                                    ClientManager.Exit(frameHandler);
                                    keepConnection = false;
                                    break;
                                default:
                                    Console.WriteLine(frameToBeSent + " no es un comando del servidor");
                                    ClientManager.DisplayMenu(frameHandler, username);
                                    break;
                            }

                        /*                    
                        else
                        {
                            frameToBeSent = "REQ"+frameToBeSent;
                            var encodedFrame = Encoding.UTF8.GetBytes(frameToBeSent);
                            var tarea = frameHandler.SendMessageAsync(encodedFrame);
                            serverResponse = await frameHandler.ReadDataAsync().ConfigureAwait(false);
                            var asciiResponse = (Encoding.ASCII.GetString(serverResponse));
                            string[] separator = {"@"};
                            string[] splittedResponse =
                                asciiResponse.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            Console.WriteLine(splittedResponse[0]);
                            if (splittedResponse[0].Equals("Sending file..."))
                            {
                                IFileFunctions fileFunctions = new FileFunctions();
                                IFileHandler fileHandler = new FileHandler(networkStream);
                                IFileService fileService = new FileService(fileFunctions, fileHandler);
                                try
                                {
                                    fileService.SendFile(splittedResponse[1]);
                                    serverResponse = await frameHandler.ReadDataAsync().ConfigureAwait(false);
                                    asciiResponse = (Encoding.ASCII.GetString(serverResponse));
                                    Console.WriteLine(asciiResponse);
                                }
                                catch (System.Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }

                            if (asciiResponse.Equals("Goodbye"))
                            {
                                keepConnection = false;
                            }
                        }                         */
                    }
                }

                tcpClient.Close();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                //   "Ha ocurrido un error de conexion verifique su app config o que el servidor este levantado");
                Console.ReadLine();
            }
        }


        private static async Task<TcpClient> ClientConnectAsync()
        {
            Console.WriteLine("Client starting...");
            var clientIpAddress = Configuration.GetSection("ClientIpAddress").Value;
            var clientPort = int.Parse(Configuration.GetSection("ClientPort").Value);
            var serverIpAddress = Configuration.GetSection("ServerIpAddress").Value;
            var serverPort = int.Parse(Configuration.GetSection("ServerPort").Value);
            var clientIpEndPoint = new IPEndPoint(IPAddress.Parse(clientIpAddress), clientPort);
            var tcpClient = new TcpClient(clientIpEndPoint);
            Console.WriteLine("Trying to connect to server");

            await tcpClient.ConnectAsync(serverIpAddress, serverPort).ConfigureAwait(false);
            return tcpClient;
        }
    }
}