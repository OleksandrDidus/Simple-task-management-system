using Microsoft.Extensions.Logging;
using Api.Data.Model;
using Api.Interfaces;
using Api.Supporting_Classes;

namespace Api.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TaskService> _logger;

        public TaskService(ITaskRepository taskRepository, ILogger<TaskService> logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;
        }

        public async Task<ServiceResponse<IEnumerable<Tasks>>> GetTasksAsync(Guid userId, TaskQueryParameters queryParameters)
        {
            var tasks = await _taskRepository.GetTasksAsync(userId, queryParameters);
            return new ServiceResponse<IEnumerable<Tasks>> { Data = tasks };
        }

        public async Task<ServiceResponse<Tasks?>> GetTaskByIdAsync(Guid userId, Guid taskId)
        {
            var task = await _taskRepository.GetTaskByIdAsync(userId, taskId);

            if (task == null)
            {
                _logger.LogWarning("Task with ID {TaskId} not found or does not belong to user {UserId}", taskId, userId);
                return new ServiceResponse<Tasks?> { Success = false, Message = "Task not found" };
            }

            return new ServiceResponse<Tasks?> { Data = task };
        }

        public async Task<ServiceResponse<Tasks>> CreateTaskAsync(Guid userId, Tasks task)
        {
            task.Id = Guid.NewGuid();
            task.UserId = userId;
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;

            await _taskRepository.AddTaskAsync(task);

            _logger.LogInformation("Task {TaskId} created for user {UserId}", task.Id, userId);

            return new ServiceResponse<Tasks> { Data = task, Message = "Task created successfully" };
        }

        public async Task<ServiceResponse<Tasks>> UpdateTaskAsync(Guid userId, Guid taskId, Tasks task)
        {
            var existingTask = await _taskRepository.GetTaskByIdAsync(userId, taskId);

            if (existingTask == null)
            {
                _logger.LogWarning("Attempt to update task {TaskId} by user {UserId} failed - task not found or unauthorized", task.Id, userId);
                return new ServiceResponse<Tasks> { Success = false, Message = "Task not found or unauthorized" };
            }

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.DueDate = task.DueDate;
            existingTask.Status = task.Status;
            existingTask.Priority = task.Priority;
            existingTask.UpdatedAt = DateTime.UtcNow;

            await _taskRepository.UpdateTaskAsync(existingTask);

            _logger.LogInformation("Task {TaskId} updated by user {UserId}", task.Id, userId);

            return new ServiceResponse<Tasks> { Data = existingTask, Message = "Task updated successfully" };
        }

        public async Task<ServiceResponse<bool>> DeleteTaskAsync(Guid userId, Guid taskId)
        {
            var task = await _taskRepository.GetTaskByIdAsync(userId, taskId);

            if (task == null)
            {
                _logger.LogWarning("Attempt to delete task {TaskId} by user {UserId} failed - task not found or unauthorized", taskId, userId);
                return new ServiceResponse<bool> { Success = false, Message = "Task not found or unauthorized" };
            }

            await _taskRepository.DeleteTaskAsync(task);

            _logger.LogInformation("Task {TaskId} deleted by user {UserId}", taskId, userId);

            return new ServiceResponse<bool> { Data = true, Message = "Task deleted successfully" };
        }
    }
}
