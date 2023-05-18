using Microsoft.AspNetCore.Identity;

namespace ProjectManagement.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[]? Picture { get; set; }
        public long TotalTime { get; set; }

        //relation
        public List<ProjectTask> Tasks { get; set; }
        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }
    }
}
