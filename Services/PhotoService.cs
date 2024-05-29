using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using CourseProjectItems.Interfaces;

namespace CourseProjectItems.Services
{
	public class PhotoService : IPhotoService
	{
        private readonly Cloudinary _cloudinary;
        private readonly string[] _validExtensions = { ".png", ".jpeg", ".jpg" };

        public PhotoService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();

                if (!_validExtensions.Contains(extension))
                {
                    uploadResult.Error = new Error { Message = "Invalid file type. Only .png, .jpeg, and .jpg files are allowed." };
                    return uploadResult;
                }

                await using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream)
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicUrl)
        {
            var deleteParams = new DeletionParams(publicUrl);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result;
        }
    }
}
