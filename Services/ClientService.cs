using System.Collections.Generic;
using Domain;
using IDataAccess;
using IServices;

namespace Services
{
    public class ClientService : IClientService
    {
        public IClientDataAccess clientsDataAccess;

        public ClientService(IClientDataAccess iClientsDataAccess)
        {
            clientsDataAccess = iClientsDataAccess;
        }

        public void RegisterUser(string username, string password)
        {
            clientsDataAccess.RegisterUser(username, password);
        }

        public void LoginUser(string username, string password)
        {
            clientsDataAccess.LoginUser(username, password);
        }

        public User GetUser(string username)
        {
            return clientsDataAccess.GetUser(username);
        }

        public List<User> GetUsers()
        {
            var users = clientsDataAccess.GetUsers();
            return users;
        }

        public List<User> GetUsersConnected()
        {
            var users = clientsDataAccess.GetUsersConnected();
            return users;
        }

        public void LogOutUser(string username)
        {
            clientsDataAccess.LogOutUser(username);
        }

        public void DeleteUser(string username)
        {
            clientsDataAccess.DeleteUser(username);
        }

        public void UpdateUser(string oldUsername, string username, string newPassword)
        {
            clientsDataAccess.UpdateUser(oldUsername,username, newPassword);
        }
    }
}