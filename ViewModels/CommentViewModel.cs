using System.ComponentModel.DataAnnotations;

namespace CourseProjectItems.ViewModels;

public class CommentViewModel
{
	public int Id { get; set; }

    [Required(ErrorMessage = "Message is required")]
    public string Message { get; set; }

    [Required(ErrorMessage = "UserName is required")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "UserId is required")]
    public string UserId { get; set; }

    public DateTime DateTime { get; set; }
}