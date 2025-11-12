using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkNest.Models.Board;

namespace WorkNest.Models.Column
{
    public class Column
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public int Order { get; set; }

        public int BoardId { get; set; }
        [ForeignKey("BoardId")]
        public Board.Board Board { get; set; } = null!;

        public List<Task.Task> Tasks { get; set; } = new();
    }
}
