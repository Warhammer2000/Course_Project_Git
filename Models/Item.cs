using CourseProjectItems.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProjectItems.Models
{
    public class Item : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public int CollectionId { get; set; }

        [Required]
        public string AuthorId { get; set; }
        public string AuthorUserName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? PhotoUrl { get; set; }
        public string Tags { get; set; }
		public Collection Collection { get; set; }

        [Required]
        public string CollectionName { get; set; } 
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public string AdditionalFields { get; set; } = "{}";

		public ICollection<Like> Likes { get; set; } = new List<Like>();

		public int IntCustom1 { get; set; }

        public int IntCustom2 { get; set; }

        public int IntCustom3 { get; set; }


        public string? StringCustom1 { get; set; }

        public string? StringCustom2 { get; set; }

        public string? StringCustom3 { get; set; }


        public DateTime DateCustom1 { get; set; }

        public DateTime DateCustom2 { get; set; }

        public DateTime DateCustom3 { get; set; }


        public bool BoolCustom1 { get; set; }

        public bool BoolCustom2 { get; set; }

        public bool BoolCustom3 { get; set; }


        public string? LargeDescriptionCustom1 { get; set; }

        public string? LargeDescriptionCustom2 { get; set; }

        public string? LargeDescriptionCustom3 { get; set; }
    }
}
