namespace ProjectManagement.Models
{
    public class Goal
    {
        public int GoalId { get; set; }
        public string Name { get; set; }

        //relations
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
