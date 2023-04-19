using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Models
{
    public class ProjectTask
    {
        [Key]
        public int TaskId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        //relations
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        
        [DisplayName("User Name")]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
