using System.Collections.Generic;
using Domain;

namespace IServices
{
    public interface IPhotoService
    {
        void UploadPhoto(string username, Photo photo);
        List<Photo> GetPhotos(string username);
    }
}