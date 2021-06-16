using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;

namespace ApertureScience
{
    public struct Photo
    {
        public string FileName { get { return fileName; } set { fileName = value.ToLower(); } }
        public int EmployeeId { get; set; }
        public string FullName { get { return $"{EmployeeId}/{FileName}"; } }
        public string ContentType { get { return CONTENT_TYPE[Extension]; } }
        public string Extension { get { return Path.GetExtension(FileName).ToLower(); } }
        public string GenerateUniqueName() { return FileName = $"{Guid.NewGuid()}{Extension}"; }

        public static implicit operator bool(Photo photo)
        {
            if (photo.FileName == null)
                return false;

            return CONTENT_TYPE.ContainsKey(photo.Extension);
        }

        private string fileName;
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
    }
}
