using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Entities;
using TaskStatus = TaskManagerAPI.Entities.TaskStatus;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskStatusesController : ControllerBase
    {
        private readonly TaskApiContext _context;

        public TaskStatusesController(TaskApiContext context)
        {
            _context = context;
        }

        // GET: api/TaskStatuses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskStatus>>> GetTaskStatuses()
        {
            return await _context.TaskStatuses.ToListAsync();
        }

        // GET: api/TaskStatuses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskStatus>> GetTaskStatus(int id)
        {
            var taskStatus = await _context.TaskStatuses.FindAsync(id);

            if (taskStatus == null)
            {
                return NotFound();
            }

            return taskStatus;
        }

        // PUT: api/TaskStatuses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskStatus(int id, TaskStatus taskStatus)
        {
            if (id != taskStatus.Id)
            {
                return BadRequest();
            }

            _context.Entry(taskStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskStatusExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TaskStatuses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaskStatus>> PostTaskStatus(TaskStatus taskStatus)
        {
            _context.TaskStatuses.Add(taskStatus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaskStatus", new { id = taskStatus.Id }, taskStatus);
        }

        // DELETE: api/TaskStatuses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskStatus(int id)
        {
            var taskStatus = await _context.TaskStatuses.FindAsync(id);
            if (taskStatus == null)
            {
                return NotFound();
            }

            _context.TaskStatuses.Remove(taskStatus);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskStatusExists(int id)
        {
            return _context.TaskStatuses.Any(e => e.Id == id);
        }
    }
}
