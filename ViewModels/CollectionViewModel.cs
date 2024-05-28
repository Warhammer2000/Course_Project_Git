using CourseProjectItems.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CourseProjectItems.ViewModels;

public class CollectionViewModel
{
	public int Id { get; set; }

    [Required]
    public string AuthorId { get; set; }
    public string AuthorUserName { get; set; }

    [Required]
	public string Name { get; set; }
	public string Category { get; set; }
	public string Description { get; set; }
	public CollectionType Type { get; set; }
    public string PhotoUrl { get; set; }

    public string IconClass { get; set; }
	public ICollection<ItemViewModel> Items { get; set; } = new List<ItemViewModel>();
}