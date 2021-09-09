using System.Collections.Generic;
using Domain;
using RabbitMQ.Client;

namespace Persistance
{
    public class MemoryDataBase
    {
        private static MemoryDataBase singletonMemoryDB;

        private MemoryDataBase()
        {
            Users = new List<User>();
            UsersConnected = new List<User>();
            Logs = new List<Log>(); 
        }

        public List<User> Users { get; set; }
        public List<User> UsersConnected { get; set; }
        public List<Log> Logs { get; set; }        
       

        public static MemoryDataBase GetInstance()
        {
            if (singletonMemoryDB == null)
            {
                singletonMemoryDB = new MemoryDataBase();
                return singletonMemoryDB;
            }

            return singletonMemoryDB;
        }
    }
}