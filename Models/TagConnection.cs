using System.ComponentModel.DataAnnotations;

namespace CourseProjectItems.Models
{
    public class TagConnection
    {
        [Key]
        public int Id { get; set; }

        public string TagId { get; set; }

        public int ItemId { get; set; }
    }
}
