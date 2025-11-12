using WorkNest.Models.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkNest.Models.Column;

namespace WorkNest.Models.Task
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime DueDate { get; set; }

        public int ColumnId { get; set; }
        [ForeignKey("ColumnId")]
        public Column.Column Column { get; set; } = null!;

        public int AssignedUserId { get; set; }
        [ForeignKey("AssignedUserId")]
        public User.User AssignedUser { get; set; } = null!;
    }
}
