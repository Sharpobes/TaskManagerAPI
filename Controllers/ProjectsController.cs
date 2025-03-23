using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly TaskApiContext _context;
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProjectsController> _logger;

        // В конструкторе регистрируем контекст базы (для получения списка проектов)
        // и HttpClient (для создания проекта через API, если нужно).
        public ProjectsController(TaskApiContext context, HttpClient httpClient, ILogger<ProjectsController> logger)
        {
            _context = context;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7004");
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Получаем все проекты из базы
            var projects = await _context.Projects.ToListAsync();

            // Передаем список проектов в представление
            return View(projects);
        }
        [HttpGet]
        public IActionResult Create()
        {
            // Если нужно, можно предварительно загрузить что-то (например, список пользователей)
            // и передать через ViewBag, если форма требует выпадающих списков.
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Создаём объект Project
            var newProject = new Project
            {
                Name = model.Name,
                Description = model.Description,
                OwnerId = model.OwnerId,
                CreatedAt = DateTime.Now
            };

            _context.Projects.Add(newProject);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Пользователь создал проект");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound();

            // Заполняем модель для формы
            var model = new ProjectViewModel
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                OwnerId = project.OwnerId
            };

            return View(model);  // передаём во View/Projects/Edit.cshtml
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Если ошибки валидации, просто вернём ту же форму
                // Можно при желании передать сообщение об ошибке через ViewBag
                return View(model);
            }

            // Ищем проект в базе
            var project = await _context.Projects.FindAsync(model.Id);
            if (project == null)
                return NotFound();

            // Обновляем поля
            project.Name = model.Name;
            project.Description = model.Description;
            project.OwnerId = model.OwnerId;

            // Сохраняем
            await _context.SaveChangesAsync();

            // Перенаправляем на список проектов (или куда вам нужно)
            return RedirectToAction("Index");
        }


    }
}
