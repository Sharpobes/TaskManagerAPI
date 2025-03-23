using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Models;
using TaskManagerAPI.Services;
using Task = TaskManagerAPI.Entities.Task;
//using TaskStatus = TaskManagerAPI.Entities.TaskStatus;
using TaskManagerAPI.Enums;
using TaskStatus = TaskManagerAPI.Enums.TaskStatus;

namespace TaskManagerAPI.Controllers
{
    public class TasksController : Controller
    {
        private readonly TaskApiContext _context;
        private readonly TaskStateService _stateService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(TaskApiContext context, ILogger<TasksController> logger)
        {
            _context = context;
            _stateService = new TaskStateService();
            _logger = logger;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Task>>> GetTasks()
        //{
        //    var tasks = await _context.Tasks.Include(t => t.Status).ToListAsync();
        //    return View(tasks);
        //}

        public async Task<IActionResult> TasksList()
        {
            var tasks = await _context.Tasks
                                       .Include(t => t.Status)
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
            return View(taskViewModels);
        }
        //[HttpGet]
        //public async Task<IActionResult> CreateAsync()
        //{
        //    var statuses = await _context.TaskStatuses.ToListAsync();

        //    // ��� �������� ��������� �� � ViewBag
        //    // (��� ����������� ����������� ������, ���� �������������)
        //    ViewBag.StatusList = statuses;

        //    // ���������� ������ ������
        //    return View(new TaskViewModel());
        //}
        [HttpPost]
        public async Task<IActionResult> Create(TaskViewModel taskViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.StatusList = await _context.TaskStatuses.ToListAsync();
                return View(taskViewModel);
            }

            var task = new Task
            {
                Title = taskViewModel.Title,
                Description = taskViewModel.Description,
                StatusId = taskViewModel.StatusId,  // <-- ����� �� �����
                ProjectId = taskViewModel.ProjectId,
                AssignedToId = taskViewModel.AssignedToId,
                Deadline = taskViewModel.Deadline,
                CreatedAt = DateTime.Now
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            _logger.LogInformation("������������ ������ ������");
            return RedirectToAction("TasksList");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // ���� ������ � ��
            var task = await _context.Tasks
                .Include(t => t.Status) // ����� ��� ������������� �������� ��� �������
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return NotFound();

            // ��������� ������� ��� ������
            var statuses = await _context.TaskStatuses.ToListAsync();
            ViewBag.StatusList = statuses;

            // ��������� ������ ��� �����
            var taskViewModel = new TaskViewModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                StatusId = task.StatusId,
                ProjectId = task.ProjectId,
                AssignedToId = task.AssignedToId,
                Deadline = task.Deadline
            };

            return View(taskViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TaskViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // ���� ���� ������ ���������, ����� ������ ��������� �������,
                // ����� ������ �������� �� View �������� �������.
                ViewBag.StatusList = await _context.TaskStatuses.ToListAsync();
                return View(model);
            }

            // ������� ������ �� Id
            var task = await _context.Tasks.FindAsync(model.Id);
            if (task == null)
                return NotFound();

            // ��������� ����
            task.Title = model.Title;
            task.Description = model.Description;
            task.StatusId = model.StatusId;
            task.ProjectId = model.ProjectId;
            task.AssignedToId = model.AssignedToId;
            task.Deadline = model.Deadline;
            task.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            // ������������ � ������ �����
            return RedirectToAction("TasksList");
        }



    }
}
