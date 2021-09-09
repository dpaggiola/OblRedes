using System.Collections.Generic;
using Domain;

namespace IServices
{
    public interface ICommentService
    {
        void UploadComment(string username, string photo, Comment comment);
        List<Comment> GetComments(string username, string photo);
    }
}