using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ClientAdmin
{
    public class Program
    {
        private static bool isServerUp = true;
        private static IConfiguration Configuration;
        private static readonly HttpClient clientABM = new HttpClient();
        private static readonly HttpClient clientLog = new HttpClient();

        private static async Task Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            clientABM.BaseAddress= new Uri(Configuration.GetSection("ABM_URI").Value);
            clientABM.DefaultRequestHeaders.Accept.Clear();
            clientABM.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            clientLog.BaseAddress = new Uri(Configuration.GetSection("LOGS_URI").Value);
            clientLog.DefaultRequestHeaders.Accept.Clear();
            clientLog.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            PrintServerCommands();
            while (isServerUp)
            {
                var serverFunction = Console.ReadLine();
                HandleServer(serverFunction);
            }
        }
        private static bool HandleServer(string function)
        {
            switch (function)
            {
                case "1":
                   ClientAdminManager.DisplayLogs(clientLog);
                    break;
                case "2":
                    ClientAdminManager.RegisterUser(clientABM);
                    break;
                case "3":
                    ClientAdminManager.DeleteUser(clientABM);
                    break;
                case "4":
                    ClientAdminManager.UpdateUser(clientABM);
                    break;
                case "5":
                    //EXIT
                    isServerUp = false;
                    break;
                default:
                    Console.WriteLine(function + " no es un comando del cliente");
                    PrintServerCommands();
                    break;
            }

            return true;
        }

        private static void PrintServerCommands()
        {
            Console.WriteLine("----COMANDOS DEL CLIENTE ADMINSTRATIVO----");
            Console.WriteLine("1) Mostrar logs");
            Console.WriteLine("2) Alta de cliente");
            Console.WriteLine("3) Baja de cliente");
            Console.WriteLine("4) Modificacion de cliente");
            Console.WriteLine("5) EXIT");
            Console.Write("\r\nEliga una opción: ");
        }
    }
}