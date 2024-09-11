using Api.Data.Model;
using TaskStatus = Api.Data.Model.TaskStatus;

namespace Simple_task_management_system.Data.DTO
{
    public class UpdateTaskDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskStatus? Status { get; set; }
        public TaskPriority? Priority { get; set; }
    }
}
