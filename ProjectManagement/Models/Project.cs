using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Status { get; set; }
        
        //relations
        public List<ProjectTask> Tasks { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
