using CourseProjectItems.Data;
using System.ComponentModel.DataAnnotations;

namespace CourseProjectItems.Models
{
	public class Ticket
	{
		[Key]
		public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
		public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
		public DateTime CreatedDate { get; set; }
        public string JiraIssueKey { get; set; }
        public string JiraIssueUrl { get; set; }
        public ApplicationUser User { get; set; }
	}
}
