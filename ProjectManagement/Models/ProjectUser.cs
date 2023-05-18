using Azure;
using Microsoft.Extensions.Hosting;

namespace ProjectManagement.Models
{
    public class ProjectUser
    {
        public string UserId { get; set; }
        public int ProjectId { get; set; }
        public User User { get; set; }
        public Project Project { get; set; }
        public bool IsManager { get; set; }
    }
}
