using CourseProjectItems.Entity;
using System.ComponentModel.DataAnnotations;

namespace CourseProjectItems.Models
{
    public class Like : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }
        public string UserId { get; set; }
    }
}
