using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Common;
using DataAccess;
using Grpc.Core;
using ICommon;
using IDataAccess;
using IServices;
using Microsoft.Extensions.Configuration;
using Server.Services;
using Services;

namespace Server
{
    public class Program
    {
        private static bool isServerUp = true;
        private static IConfiguration Configuration;

        private static async Task Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();


            Console.WriteLine("Server is starting...");
            var ipEndPoint = new IPEndPoint(
                IPAddress.Parse(Configuration.GetSection("ServerIpAddress").Value),
                int.Parse(Configuration.GetSection("ServerPort").Value));
            var tcpListener = new TcpListener(ipEndPoint);

            tcpListener.Start(100);
            Console.WriteLine("Server started listening connections on ip " +
                              Configuration.GetSection("ServerIpAddress").Value + " and port " +
                              Configuration.GetSection("ServerPort").Value);
            Console.WriteLine("Server will start displaying messages from the clients");

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            //grpc
            int grpcPort = int.Parse(Configuration.GetSection("ServerGRPCPort").Value);
            Grpc.Core.Server server = new Grpc.Core.Server
            {
                Services = {Greeter.BindService(new GreeterService())},
                Ports = {new ServerPort("localhost", grpcPort, ServerCredentials.Insecure)}
            };
            server.Start();
            
            try
            {
                var serverFunctionsTask = Task.Run(() => ServerFunctions());
                while (isServerUp)
                {
                    var tcpClientSocket = await tcpListener.AcceptTcpClientAsync().ConfigureAwait(false);
                    var handleClientTask =
                        Task.Run(async () => await HandleClient(tcpClientSocket).ConfigureAwait(false));
                }
            }
            finally
            {
                server.ShutdownAsync().Wait();
            }
        }

        private static void ServerFunctions()
        {
            PrintServerCommands();
            ServerFunctionManager.PopulateServer();
            while (isServerUp)
            {
                var serverFunction = Console.ReadLine();
                Console.WriteLine("Server running: " + serverFunction);
                HandleServer(serverFunction);
            }
        }

        private static bool HandleServer(string serverFunction)
        {
            switch (serverFunction)
            {
                case "1":
                    ServerFunctionManager.DisplayUsers();
                    break;
                case "2":
                    ServerFunctionManager.RegisterUser();
                    break;
                case "3":
                    ServerFunctionManager.DeleteUser();
                    break;
                case "4":
                    ServerFunctionManager.UpdateUser();
                    break;
                case "5":
                    ServerFunctionManager.DisplayUserPhotos();
                    break;
                case "6":
                    ServerFunctionManager.AddUserPhotos();
                    break;
                case "7":
                    ServerFunctionManager.DisplayComments();
                    break;
                case "8":
                    ServerFunctionManager.AddUserComment();
                    break;
                case "9":
                    //EXIT
                    isServerUp = false;
                    Console.WriteLine("El servidor no aceptará clientes hasta que este encendido");
                    break;
                default:
                    Console.WriteLine(serverFunction + " no es un comando del servidor");
                    PrintServerCommands();
                    break;
            }

            return true;
        }

        private static void PrintServerCommands()
        {
            Console.WriteLine("----COMANDOS DEL SERVIDOR----");
            Console.WriteLine("1) Mostrar clientes conectados");
            Console.WriteLine("2) Alta de cliente");
            Console.WriteLine("3) Baja de cliente");
            Console.WriteLine("4) Modificacion de cliente");
            Console.WriteLine("5) Listado de fotos");
            Console.WriteLine("6) Carga de fotos");
            Console.WriteLine("7) Listado de comentarios");
            Console.WriteLine("8) Comentar una foto");
            Console.WriteLine("9) EXIT");
            Console.Write("\r\nEliga una opción: ");
        }

        private static async Task HandleClient(TcpClient tcpClientSocket)
        {
            var isClientConnected = true;
            try
            {
                using (var networkStream = tcpClientSocket.GetStream())
                {
                    IFrameHandler frameHandler = new FrameHandler(networkStream);
                    while (isClientConnected)
                    {
                        var frame = await frameHandler.ReadDataAsync();
                        var printiame = Encoding.ASCII.GetString(frame);
                        Console.WriteLine("Manejando: " + printiame);
                        isClientConnected = HandleRequest(frame, networkStream);
                    }
                }
            }
            catch (SocketException)
            {
                Console.WriteLine("Conexion del cliente fue interrumpida.");
            }
        }

        private static void ServerResponse(IFrameHandler frameHandler, string msg)
        {
            var bytesMsg = Encoding.UTF8.GetBytes(msg);
            frameHandler.SendMessageAsync(bytesMsg);
        }

        private static bool HandleRequest(byte[] frame, NetworkStream networkStream)
        {
           
            try
            {
                IFrameHandler frameHandler = new FrameHandler(networkStream);

                ICodification<HeaderStructure> header = new Header();


                var headerStructure = header.Decode(frame);

                if (!IsHeaderStructureOk(headerStructure, frame)) return true;

                var command = headerStructure.CommandType;

                switch (command)
                {
                    case CommandType.MU:
                        var user = ServerManager.DisplayMenu(frame);
                        SendMenu(networkStream, user != null);
                        break;
                    case CommandType.AU:
                        ServerManager.RegisterUser(frame);
                        ServerResponse(frameHandler, "Usuario Creado");
                        break;
                    case CommandType.SU:
                        var user1 = ServerManager.LoginUser(frame);
                        ServerResponse(frameHandler, user1.Username);
                        break;
                    case CommandType.LU:
                        ServerResponse(frameHandler, ServerManager.DisplayUsers());
                        break;
                    case CommandType.AF:
                        ServerManager.AddPhoto(frame, networkStream);
                        ServerResponse(frameHandler, "Foto Creada");
                        break;
                    case CommandType.LF:
                        ServerResponse(frameHandler, ServerManager.DisplayPhotos(frame));
                        break;
                    case CommandType.LC:
                        ServerResponse(frameHandler, ServerManager.DisplayComments(frame));
                        break;
                    case CommandType.AC:
                        ServerManager.AddComment(frame);
                        ServerResponse(frameHandler, "Comentario Creado");
                        break;
                    case CommandType.FF:
                        ServerResponse(frameHandler, "Cerrando");
                        return false;
                    default:
                        ServerResponse(frameHandler, "Formato de trama invalido vuelva a enviar");
                        break;
                }

                SenderService.CreateLog(command.ToString().ToUpper(), frame);

                return true;
            }
            catch (System.Exception ex)
            {
                SenderService.CreateLog( "EX", frame);
                if (ex is FormatException || ex is ArgumentException)
                {
                    IFrameHandler frameHandler = new FrameHandler(networkStream);
                    ServerResponse(frameHandler, "La trama recibida fue invalida");
                }
                else if (ex is IndexOutOfRangeException)
                {
                    IFrameHandler frameHandler = new FrameHandler(networkStream);
                    ServerResponse(frameHandler, "El objeto solicitado en el Data esta mal construido");
                }
                else
                {
                    IFrameHandler frameHandler = new FrameHandler(networkStream);
                    ServerResponse(frameHandler, ex.Message);
                }

                return true;
            }
        }

        private static bool IsHeaderStructureOk(HeaderStructure headerStructure, byte[] frame)
        {
            return headerStructure.CommandType.GetType().IsEnum && headerStructure.FlagType.GetType().IsEnum;
        }

        private static void SendMenu(NetworkStream networkStream, bool isLogged)
        {
            IMenuDataAccess menuDA = new MenuDataAccess();
            IMenuService menuService = new MenuService(menuDA);
            IFrameHandler frameHandler = new FrameHandler(networkStream);
            var items = menuService.GetMenuItems(isLogged);


            var menu = "Seleccionar opcion :" + "\n";
            var count = 1;
            foreach (var item in items)
            {
                menu += count + ") " + item + "\n";
                count++;
            }

            var data = Encoding.UTF8.GetBytes(menu);
            frameHandler.SendMessageAsync(data);
        }
    }
}