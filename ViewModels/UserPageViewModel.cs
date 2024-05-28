using CourseProjectItems.Models;
using Microsoft.AspNetCore.Identity;

namespace CourseProjectItems.ViewModels
{
    public class UserPageViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
        public FilterViewModel FilterViewModel { get; set; }
        public IEnumerable<Collection> Collections { get; set; }
    }
}
