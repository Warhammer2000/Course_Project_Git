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

		public PhotoService(Cloudinary cloudinary)
		{
			_cloudinary = cloudinary;
		}

		public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
		{
			var uploadResult = new ImageUploadResult();

			if (file.Length > 0)
			{
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
