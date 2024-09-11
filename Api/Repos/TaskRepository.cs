using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Api.Data;
using Api.Data.Model;
using Api.Interfaces;
using Api.Supporting_Classes;

namespace Api.Repos
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddTaskAsync(Tasks task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tasks>> GetTasksAsync(Guid userId, TaskQueryParameters queryParameters)
        {
            var query = _context.Tasks.Where(t => t.UserId == userId).AsQueryable();

            // Apply filtering
            if (queryParameters.Status.HasValue)
            {
                query = query.Where(t => t.Status == queryParameters.Status);
            }
            if (queryParameters.Priority.HasValue)
            {
                query = query.Where(t => t.Priority == queryParameters.Priority);
            }
            if (queryParameters.DueDate.HasValue)
            {
                query = query.Where(t => t.DueDate == queryParameters.DueDate);
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(queryParameters.OrderBy))
            {
                switch (queryParameters.OrderBy.ToLower())
                {
                    case "duedate":
                        query = query.OrderBy(t => t.DueDate);
                        break;
                    case "priority":
                        query = query.OrderBy(t => t.Priority);
                        break;
                    default:
                        query = query.OrderBy(t => t.CreatedAt);
                        break;
                }
            }

            // Apply pagination
            var skip = (queryParameters.PageNumber - 1) * queryParameters.PageSize;
            return await query.Skip(skip).Take(queryParameters.PageSize).ToListAsync();
        }

        public async Task<Tasks?> GetTaskByIdAsync(Guid userId, Guid taskId)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.UserId == userId && t.Id == taskId);
        }

        public async Task UpdateTaskAsync(Tasks task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(Tasks task)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }

}
