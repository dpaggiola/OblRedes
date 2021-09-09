using System.Collections.Generic;
using System.Threading;
using Domain;
using Exception;
using IDataAccess;
using Persistance;

namespace DataAccess
{
    public class CommentDataAccess : ICommentDataAccess
    {
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private readonly List<User> users = MemoryDataBase.GetInstance().Users;

        public void UploadComment(string username, string photo, Comment comment)
        {
            try
            {
                semaphore.WaitAsync();

                var index = users.FindIndex(userInList => userInList.Username.Equals(username));
                if (index >= 0)
                {
                    var indexPhoto = users[index].Photos.FindIndex(photoInList => photoInList.Name.Equals(photo));
                    if (indexPhoto >= 0)
                        users[index].Photos[indexPhoto].Comments.Add(comment);
                    else
                        throw new PhotoDoesNotExistsException("No existe foto especificada");
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

        public List<Comment> GetComments(string username, string photo)
        {
            try
            {
                semaphore.WaitAsync();

                var index = users.FindIndex(userInList => userInList.Username.Equals(username));
                if (index >= 0)
                {
                    var indexPhoto = users[index].Photos.FindIndex(photoInList => photoInList.Name.Equals(photo));
                    if (indexPhoto >= 0)
                        return users[index].Photos[indexPhoto].Comments;
                    else
                        throw new PhotoDoesNotExistsException("No existe foto especificada");
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