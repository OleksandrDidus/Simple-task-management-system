using Api.Data.DTO;
using Api.Data.Model;
using Api.Interfaces;
using Api.Supporting_Classes;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public TasksController(ITaskService taskService, IUserService userService, IMapper mapper)
        {
            _taskService = taskService;
            _userService = userService;
            _mapper = mapper;

        }
        
        [HttpPost("addtask")]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto createTask)
        {
            var task = _mapper.Map<Tasks>(createTask);
            var userId = _userService.GetUserId();
            var result = await _taskService.CreateTaskAsync(userId, task);
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.Data);
        }

        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetTasks([FromQuery] TaskQueryParameters queryParameters)
        {
            var userId = _userService.GetUserId();
            var tasks = await _taskService.GetTasksAsync(userId, queryParameters);
            var taskDtos = _mapper.Map<List<TaskDto>>(tasks.Data);
            return Ok(taskDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            var userId = _userService.GetUserId();
            var task = await _taskService.GetTaskByIdAsync(userId, id);
            if (task == null)
            {
                return NotFound();
            }
            var taskDto = _mapper.Map<TaskDto>(task);
            return Ok(taskDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] CreateTaskDto updateTask)
        {
            var task = _mapper.Map<Tasks>(updateTask);
            var userId = _userService.GetUserId();
            var result = await _taskService.UpdateTaskAsync(userId,id, task);
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var userId = _userService.GetUserId();
            var result = await _taskService.DeleteTaskAsync(userId, id);
            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }
            return NoContent();
        }
    }

}
