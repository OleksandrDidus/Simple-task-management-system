using Api.Data.Model;
using TaskStatus = Api.Data.Model.TaskStatus;

namespace Api.Supporting_Classes
{
    public class TaskQueryParameters
    {
        public TaskStatus? Status { get; set; }
        public TaskPriority? Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public string? OrderBy { get; set; } = "createdAt";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
