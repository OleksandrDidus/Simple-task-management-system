using Api.Data.Model;
using Api.Services;
using Api.Supporting_Classes;

namespace Api.Interfaces
{
    public interface ITaskService
    {
        Task<ServiceResponse<IEnumerable<Tasks>>> GetTasksAsync(Guid userId, TaskQueryParameters queryParameters);
        Task<ServiceResponse<Tasks?>> GetTaskByIdAsync(Guid userId, Guid taskId);
        Task<ServiceResponse<Tasks>> CreateTaskAsync(Guid userId, Tasks task);
        Task<ServiceResponse<Tasks>> UpdateTaskAsync(Guid userId, Guid taskId, Tasks task);
        Task<ServiceResponse<bool>> DeleteTaskAsync(Guid userId, Guid taskId);
    }
}
