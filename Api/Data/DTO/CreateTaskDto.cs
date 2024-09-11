using Api.Data.Model;
using System.ComponentModel.DataAnnotations;

namespace Api.Data.DTO
{
    public class CreateTaskDto
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; }

        public string? Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        [Required]
        public Model.TaskStatus Status { get; set; }

        [Required]
        public TaskPriority Priority { get; set; }

        public Guid UserId { get; set; }

    }
}
