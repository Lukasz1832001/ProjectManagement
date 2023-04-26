using ProjectManagement.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Models
{
    public class Risk
    {
        [Key]
        public int RiskId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ImpactCategories Probability { get; set; }
        public ImpactCategories Influence { get; set; }
        public string Reaction { get; set; }
        
        //relations
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
