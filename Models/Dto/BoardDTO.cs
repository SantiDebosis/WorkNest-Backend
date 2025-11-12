using System.ComponentModel.DataAnnotations;
using WorkNest.Models.User.Dto;

namespace WorkNest.Models.Dto
{
    public class TaskDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public int ColumnId { get; set; }
        public UserWithoutPassDTO AssignedUser { get; set; } = null!; 
    }

    public class CreateTaskDTO
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        public int ColumnId { get; set; } 
        [Required]
        public int AssignedUserId { get; set; } 
    }

    public class UpdateTaskDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int? AssignedUserId { get; set; }
    }

    public class MoveTaskDTO
    {
        [Required]
        public int TargetColumnId { get; set; }
    }

    public class ColumnDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Order { get; set; }
        public List<TaskDTO> Tasks { get; set; } = new();
    }

    public class BoardDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class BoardDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<ColumnDTO> Columns { get; set; } = new();
    }

    public class CreateBoardDTO
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; } = null!;
    }
}
