using Api.Data.Model;
using Api.Supporting_Classes;

namespace Api.Interfaces
{
    public interface ITaskRepository
    {
        Task AddTaskAsync(Tasks task);
        Task<IEnumerable<Tasks>> GetTasksAsync(Guid userId, TaskQueryParameters queryParameters);
        Task<Tasks?> GetTaskByIdAsync(Guid userId, Guid taskId);
        Task UpdateTaskAsync(Tasks task);
        Task DeleteTaskAsync(Tasks task);
    }
}
