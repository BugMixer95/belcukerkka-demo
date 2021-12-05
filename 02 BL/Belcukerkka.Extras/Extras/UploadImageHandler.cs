using Belcukerkka.Models.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Belcukerkka.Services.Extras
{
    /// <summary>
    /// Handler which is called when image is being uploaded to a server.
    /// </summary>
    public class UploadImageHandler
    {
        /// <summary>
        /// Uploads new image for an entity. Creates new one if no image for specified entity exists. 
        /// Otherwise, the old will be removed and replaced with the new one.
        /// </summary>
        /// <param name="entity">Entity which iamge is uploaded for.</param>
        /// <param name="image">Image file to upload.</param>
        /// <param name="webRootPath">Physical path to wwwroot folder.</param>
        /// <param name="imageFolder">Physical path to images folder.</param>
        /// <returns>Full path to uploaded image.</returns>
        public static string ProcessUploadedImage<TEntity>(TEntity entity, IFormFile image, string webRootPath, string imageFolder)
            where TEntity : Entity
        {
            string uniqueFileName = null;

            var propertyInfo = entity.GetType().GetProperty("ImagePath");
            var propertyValue = propertyInfo.GetValue(entity, null)?.ToString() ?? string.Empty;
            
            if (image != null)
            {
                if (!string.IsNullOrEmpty(propertyValue))
                {
                    string imagePath = Path.Combine(webRootPath, imageFolder, propertyValue);
                    File.Delete(imagePath);
                }

                string uploadsFolder = Path.Combine(webRootPath, imageFolder);
                uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}
