using System.Collections.Generic;
using Domain;

namespace IDataAccess
{
    public interface IClientDataAccess
    {
        void RegisterUser(string username, string password);
        void LoginUser(string username, string password);
        User GetUser(string username);
        List<User> GetUsers();
        List<User> GetUsersConnected();
        void LogOutUser(string username);
        void DeleteUser(string username);
        void UpdateUser(string oldUsername, string username, string newPassword);
    }
}