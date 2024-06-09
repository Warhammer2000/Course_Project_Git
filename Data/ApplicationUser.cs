using Microsoft.AspNetCore.Identity;

namespace CourseProjectItems.Data
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsBlocked { get; set; }
        public string ApiToken { get; set; }
    }
}
