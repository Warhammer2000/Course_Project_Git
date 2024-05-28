using CourseProjectItems.Entity;
using System.ComponentModel.DataAnnotations;

namespace CourseProjectItems.Models
{
    public class Comment : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
		public int ItemId { get; set; }

        public string UserName { get; set; }
        public Item Item { get; set; }

        [Required]
        public string UserId { get; set; }

        public DateTime DateTime { get; set; }

        public string Message { get; set; }
    }
}
