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
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }
        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }
        [DisplayName("Budget (in dollars)")]
        public decimal TotalBudget { get; set; }
        [DisplayName("Project Scope")]
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
