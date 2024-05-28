using System.ComponentModel.DataAnnotations;
using CourseProjectItems.Entity;
using CourseProjectItems.Interfaces;
using CourseProjectItems.Models.Enums;

namespace CourseProjectItems.Models
{
    public class Collection : BaseEntity, ICollectionItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string AuthorID { get; set; }
        public string AuthorUserName { get; set; }
        public string Name { get; set; }
        public CollectionType Type { get; set; }
        public string Category { get; set; }
		public string Description { get; set; }

		public string PhotoUrl { get; set; }

        public string IconClass { get; set; }

        public string? IntName1 { get; set; }
        public string? IntName2 { get; set; }
        public string? IntName3 { get; set; }

        public string? StringName1 { get; set; }
        public string? StringName2 { get; set; }
        public string? StringName3 { get; set; }

        public string? BoolName1 { get; set; }
        public string? BoolName2 { get; set; }
        public string? BoolName3 { get; set; }

        public string? DateName1 { get; set; }
        public string? DateName2 { get; set; }
        public string? DateName3 { get; set; }

        public string? LargeStringName1 { get; set; }
        public string? LargeStringName2 { get; set; }
        public string? LargeStringName3 { get; set; }

        public ICollection<Item> Items { get; set; } = new List<Item>();
	}
}
