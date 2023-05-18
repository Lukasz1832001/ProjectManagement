using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagement.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Status { get; set; }
        public double TotalBudget { get; set; }
        public string ProjectScope { get; set; }
        public string Sponsor { get; set; }
        public string Stakeholders { get; set; }
        public string? Results { get; set; }

        //relations
        public List<ProjectTask>? Tasks { get; set; }
        public List<Risk>? Risks { get; set; }
        public List<Comment>? Comments { get; set; }
        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }
        public List<Goal>? Goals { get; set; }
        public List<Milestone>? Milestones { get; set; }
    }
}
