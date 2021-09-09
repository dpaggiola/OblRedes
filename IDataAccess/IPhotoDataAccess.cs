using System.Collections.Generic;
using Domain;

namespace IDataAccess
{
    public interface IPhotoDataAccess
    {
        void UploadPhoto(string username, Photo photo);
        List<Photo> GetPhotos(string username);
    }
}