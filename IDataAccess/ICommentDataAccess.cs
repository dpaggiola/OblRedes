using System.Collections.Generic;
using Domain;

namespace IDataAccess
{
    public interface ICommentDataAccess
    {
        void UploadComment(string username, string photo, Comment comment);
        List<Comment> GetComments(string username, string photo);
    }
}