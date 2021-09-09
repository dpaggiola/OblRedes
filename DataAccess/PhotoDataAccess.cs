using System.Collections.Generic;
using System.Threading;
using Domain;
using Exception;
using IDataAccess;
using Persistance;

namespace DataAccess
{
    public class PhotoDataAccess : IPhotoDataAccess
    {
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private readonly List<User> users = MemoryDataBase.GetInstance().Users;

        public void UploadPhoto(string username, Photo photo)
        {
            try
            {
                semaphore.WaitAsync();

                var index = users.FindIndex(userInList => userInList.Username.Equals(username));
                if (index >= 0)
                    users[index].Photos.Add(photo);
                else
                    throw new UserDoesNotExistsException("No existe usuario especificado");
            }
            finally
            {
                semaphore.Release();
            }
        }

        public List<Photo> GetPhotos(string username)
        {
            try
            {
                semaphore.WaitAsync();

                var index = users.FindIndex(userInList => userInList.Username.Equals(username));
                if (index >= 0)
                    return users[index].Photos;
                else
                    throw new UserDoesNotExistsException("No existe usuario especificado");
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}