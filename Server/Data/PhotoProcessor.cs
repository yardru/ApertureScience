using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApertureScience
{
    public class PhotoProcessor
    {
        public string FileName { get; private set; }
        private PhotoProcessor() { }
        private PhotoProcessor(string fileName) { FileName = fileName; }
        static public PhotoProcessor GetProcessor(string fileName) { return new PhotoProcessor(fileName); }

        public string ContentType { get { return CONTENT_TYPE[Extension]; } }
        public string Extension { get { return Path.GetExtension(FileName).ToLower(); } }
        public static implicit operator bool(PhotoProcessor proc)
        {
            if (proc == null || proc.FileName == null || proc.IsUsed)
                return false;

            return CONTENT_TYPE.ContainsKey(proc.Extension);
        }

        public bool IsExists()
        {
            return this && File.Exists(ImagePath(FileName));
        }

        public FileStream Get()
        {
            if (!IsExists())
                return null;
            IsUsed = true;

            return new FileStream(ImagePath(FileName), FileMode.Open);
        }

        public async Task Save(IFormFile uploadedFile)
        {
            if (!this)
                return;

            FileName = $"{Guid.NewGuid()}{Extension}";
            using (var fileStream = new FileStream(ImagePath(FileName), FileMode.Create))
                await uploadedFile.CopyToAsync(fileStream);
            IsUsed = true;
        }

        public void Delete()
        {
            if (IsExists())
                File.Delete(ImagePath(FileName));
            FileName = null;
            IsUsed = true;
        }

        static private string ImagePath(string fileName)
        {
            return $"{IMAGES_PATH}/{fileName}";
        }

        static private readonly string IMAGES_PATH = "../photos";
        static private readonly Dictionary<string, string> CONTENT_TYPE = new Dictionary<string, string>
        {
            { ".apng", "image/apng" },
            { ".avif", "image/avif" },
            { ".gif", "image/gif" },
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".jfif", "image/jpeg" },
            { ".pjpeg", "image/jpeg" },
            { ".pjp", "image/jpeg" },
            { ".png", "image/png" },
            { ".svg", "image/svg+xml" },
            { ".webp", "image/webp" },
            { ".bmp", "image/bmp" },
            { ".ico", "image/x-icon" },
            { ".cur", "image/x-icon" },
            { ".tiff", "image/tiff" },
            { ".tif", "image/tiff" },
        };
        private bool IsUsed = false;
    }
}
