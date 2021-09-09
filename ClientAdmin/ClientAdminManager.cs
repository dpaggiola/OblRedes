using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain;
using Newtonsoft.Json;

namespace ClientAdmin
{
    public class ClientAdminManager
    {
        public static async void DisplayLogs(HttpClient client)
        {
            var response = await client.GetAsync($"api/logs/");
            string result = response.Content.ReadAsStringAsync().Result;
            if (result != null)
            {
                var logs = JsonConvert.DeserializeObject<List<Log>>(result);
                foreach (var log in logs)
                {
                    Console.WriteLine(" [x] Received log level [{0}], message [{1}]", log.EventType, log.Message);
                }
            }
        }

        public static async void RegisterUser(HttpClient client)
        {
            Console.WriteLine("Ingresar usuario");
            var username = "";
            while (username == "") username = Console.ReadLine();

            Console.WriteLine("Ingresar contrasena");
            var password = "";
            while (password == "") password = Console.ReadLine();

            User user = new User();
            user.Username = username;
            user.Password = password;
            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"api/users", data);
            string result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(result);
        }

        public static async void DeleteUser(HttpClient client)
        {
            Console.WriteLine("Ingresar usuario");
            var username = "";
            while (username == "") username = Console.ReadLine();


            var response = await client.DeleteAsync($"api/users/{username}");
            string result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(result);
        }

        public static async void UpdateUser(HttpClient client)
        {
            Console.WriteLine("Ingresar usuario");
            var oldUsername = "";
            while (oldUsername == "") oldUsername = Console.ReadLine();
            Console.WriteLine("Ingresar usuario a modificar ");
            var username = "";
            while (username == "") username = Console.ReadLine();

            Console.WriteLine("Ingresar contrasena");
            var password = "";
            while (password == "") password = Console.ReadLine();

            User user = new User();
            user.Username = username;
            user.Password = password;

            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(user)
                , Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"api/users/{oldUsername}", httpContent);
            string result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(result);
        }
    }
}