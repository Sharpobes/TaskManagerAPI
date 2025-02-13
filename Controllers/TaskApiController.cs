using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Models;
using TaskManagerAPI.Services;
using Task = TaskManagerAPI.Entities.Task;

namespace TaskManagerAPI.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TasksApiController : ControllerBase
    {
        private readonly TaskApiContext _context;

        public TasksApiController(TaskApiContext context)
        {
            _context = context;
        }

        [HttpGet("gettasks")]
        [Authorize]
        public async Task<IActionResult> GetTasks()
        {
            var tasks = await _context.Tasks
                .Include(t => t.Status)
                .Include(t => t.Project)
                .ToListAsync();

            var taskViewModels = tasks.Select(t => new TaskViewModel
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                StatusId = t.StatusId,
                ProjectId = t.ProjectId,
                AssignedToId = t.AssignedToId,
                Deadline = t.Deadline
            }).ToList();

            return Ok(taskViewModels);
        }

        // POST: api/tasks
        [HttpPost]
        public async Task<ActionResult<Task>> CreateTask(TaskViewModel taskViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // если данные невалидные
            }

            // Создаём новую задачу
            var task = new Task
            {
                Title = taskViewModel.Title,
                Description = taskViewModel.Description,
                StatusId = taskViewModel.StatusId,
                ProjectId = taskViewModel.ProjectId,
                AssignedToId = taskViewModel.AssignedToId,
                Deadline = taskViewModel.Deadline,
                CreatedAt = DateTime.Now
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            // Возвращаем данные о созданной задаче
            return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, task);
        }
    }
}
