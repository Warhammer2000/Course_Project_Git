using System.ComponentModel.DataAnnotations;

namespace CourseProjectItems.ViewModels
{
	public class ItemViewModel
	{
		public int Id { get; set; }

        [Required(ErrorMessage = "AuthorID is required")]
        public string AuthorId { get; set; }
        public string AuthorUserName { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string Description { get; set; } 
		public int CollectionId { get; set; }

        [Required(ErrorMessage = "CollectionName is required")]
        public string CollectionName { get; set; }

		public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
		public List<LikeViewModel> Likes { get; set; } = new List<LikeViewModel>();
        public IFormFile Photo { get; set; }
        public string Tags { get; set; }
        public  string AdditionalFields { get; set; } 
		public string? PhotoUrl { get; set; }

    }
}
