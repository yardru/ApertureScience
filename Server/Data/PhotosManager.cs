using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApertureScience
{
    static public class PhotosManager
    {
        static public IEnumerable<Photo> Photos(int employeeId)
        {
            return Directory.EnumerateFiles($"{PHOTOS_PATH}/{employeeId}").Select(fileName => Photo(employeeId, fileName));
        }

        static public Photo Photo(int employeeId, string fileName)
        {
            return new Photo { EmployeeId = employeeId, FileName = fileName };            
        }

        static public FileStream Load(Photo photo)
        {
            if (!IsExists(photo))
                return null;
            return new FileStream(PhotoPath(photo), FileMode.Open);
        }

        static public async Task Save(Photo photo, IFormFile upload, bool isOverriding = true)
        {
            if (isOverriding)
                photo.GenerateUniqueName();
            using (var fileStream = new FileStream(PhotoPath(photo), isOverriding ? FileMode.Create : FileMode.CreateNew))
                await upload.CopyToAsync(fileStream);
        }

        static public void Delete(Photo photo)
        {
            if (IsExists(photo))
                File.Delete(PhotoPath(photo));
        }

        static public bool IsExists(Photo photo)
        {
            return File.Exists(PhotoPath(photo));
        }

        static private string PhotoPath(Photo photo)
        {
            return $"{PHOTOS_PATH}/{photo.FullName}";
        }
        static private readonly string PHOTOS_PATH = "../photos";
    }
}
