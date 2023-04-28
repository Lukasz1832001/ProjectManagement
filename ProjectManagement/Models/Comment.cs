using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        [Required(ErrorMessage = "Treść komentarza jest wymagana")]
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }

        //relations
        public string UserId { get; set; }
        public User User { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
