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
        [DisplayName("Start")]
        public DateTime StartDate { get; set; }
        [DisplayName("End")]
        public DateTime EndDate { get; set; }
        [DisplayName("Time (in hours)")]
        public int Time { get; set; }
        public bool Status { get; set; }
        public string? Result { get; set; }

        //relations
        [DisplayName("Project Name")]
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        
        [DisplayName("User Name")]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
