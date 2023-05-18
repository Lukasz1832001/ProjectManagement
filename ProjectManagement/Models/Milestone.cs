namespace ProjectManagement.Models
{
    public class Milestone
    {
        public int MilestoneId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        //relations
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
