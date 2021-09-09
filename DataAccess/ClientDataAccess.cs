using System;
using System.Collections.Generic;
using System.Threading;
using Domain;
using Exception;
using IDataAccess;
using Persistance;

namespace DataAccess
{
    public class ClientDataAccess : IClientDataAccess
    {
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private readonly List<User> users = MemoryDataBase.GetInstance().Users;
        private readonly List<User> usersConnected = MemoryDataBase.GetInstance().UsersConnected;

        public void RegisterUser(string username, string password)
        {
            try
            {
                semaphore.WaitAsync();
                var newUser = new User();
                newUser.Username = username;
                newUser.Password = password;

                var index = users.FindIndex(userInList => userInList.Username.Equals(username));
                if (index < 0)
                    users.Add(newUser);
                else
                    throw new UserAlreadyExistsException("Ya existe usuario.");
            }
            finally
            {
                semaphore.Release();
            }
        }

        public void LoginUser(string username, string password)
        {
            try
            {
                semaphore.WaitAsync();
                var index = users.FindIndex(userInList =>
                    userInList.Username.Equals(username));
                if (index >= 0)
                {
                    var user = users[index];
                    if (user.Password.Equals(password))
                    {
                        var indexConnected =
                            usersConnected.FindIndex(userInList => userInList.Username.Equals(username));
                        if (indexConnected < 0)
                        {
                            var hour = DateTime.Now;
                            user.LastConnection = hour.Hour + ":" + hour.Minute;
                            usersConnected.Add(user);
                        }
                        else
                        {
                            throw new UserAlreadyConnectedException("Ya existe usuario conectado");
                        }
                    }
                    else
                    {
                        throw new UserDoesNotExistsException("Contrasena erronea.");
                    }
                }
                else
                {
                    throw new UserDoesNotExistsException("No existe usuario especificado");
                }
            }
            finally
            {
                semaphore.Release();
            }
        }


        public User GetUser(string username)
        {
            try
            {
                semaphore.WaitAsync();
                var index = users.FindIndex(userInList => userInList.Username.Equals(username));
                if (index >= 0)
                    return users[index];
                else
                    throw new UserDoesNotExistsException("No existe Usuario especificado");
            }
            finally
            {
                semaphore.Release();
            }
        }

        public List<User> GetUsers()
        {
            try
            {
                semaphore.WaitAsync();
                return users;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public List<User> GetUsersConnected()
        {
            try
            {
                semaphore.WaitAsync();
                return usersConnected;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public void LogOutUser(string username)
        {
            try
            {
                semaphore.WaitAsync();
                var index = usersConnected.FindIndex(userInList => userInList.Username.Equals(username));
                if (index >= 0)
                    usersConnected.RemoveAt(index);
                else
                    throw new UserDoesNotExistsException("No esta registrado usuario especificado.");
            }
            finally
            {
                semaphore.Release();
            }
        }

        public void DeleteUser(string username)
        {
            try
            {
                semaphore.WaitAsync();
                var index = users.FindIndex(userInList => userInList.Username.Equals(username));
                if (index >= 0)
                    users.RemoveAt(index);
                else
                    throw new UserDoesNotExistsException("No existe usuario especificado");
            }
            finally
            {
                semaphore.Release();
            }
        }

        public void UpdateUser(string oldUsername, string username, string newPassword)
        {
            try
            {
                semaphore.WaitAsync();
                var index = users.FindIndex(userInList => userInList.Username.Equals(oldUsername));
                if (index >= 0)
                {
                    var user = users[index];
                    user.Username = username;
                    user.Password = newPassword;
                    var indexConnected = usersConnected.FindIndex(userInList => userInList.Username.Equals(username));
                    if (indexConnected >= 0) users[indexConnected] = user;
                }
                else
                {
                    throw new UserDoesNotExistsException("No existe usuario especificado");
                }
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}