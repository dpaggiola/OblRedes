using System.Collections.Generic;
using Domain;
using IDataAccess;
using IServices;

namespace Services
{
    public class CommentService : ICommentService
    {
        public ICommentDataAccess commentDataAccess;

        public CommentService(ICommentDataAccess commentDataAccess)
        {
            this.commentDataAccess = commentDataAccess;
        }

        public void UploadComment(string username, string photo, Comment comment)
        {
            commentDataAccess.UploadComment(username, photo, comment);
        }

        public List<Comment> GetComments(string username, string photo)
        {
            return commentDataAccess.GetComments(username, photo);
        }
    }
}