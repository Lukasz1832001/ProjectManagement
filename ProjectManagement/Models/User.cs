using Microsoft.AspNetCore.Identity;

namespace ProjectManagement.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //relation
        public List<ProjectTask> Tasks { get; set; }
    }
}
