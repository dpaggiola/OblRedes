using System.Collections.Generic;
using Domain;
using IDataAccess;
using IServices;

namespace Services
{
    public class PhotoService : IPhotoService
    {
        public IPhotoDataAccess photoDataAccess;

        public PhotoService(IPhotoDataAccess photoDataAccess)
        {
            this.photoDataAccess = photoDataAccess;
        }

        public void UploadPhoto(string username, Photo photo)
        {
            photoDataAccess.UploadPhoto(username, photo);
        }

        public List<Photo> GetPhotos(string username)
        {
            return photoDataAccess.GetPhotos(username);
        }
    }
}