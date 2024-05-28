using System.ComponentModel.DataAnnotations;

namespace CourseProjectItems.Models
{
    public class Tag
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }
    }
}



