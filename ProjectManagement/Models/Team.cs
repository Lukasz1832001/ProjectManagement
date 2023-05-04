using System.ComponentModel;

namespace ProjectManagement.Models
{
    public class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; }

        [DisplayName("Leader")]
        public string LeaderId { get; set; }
        public User Leader { get; set; }    
    }
}
