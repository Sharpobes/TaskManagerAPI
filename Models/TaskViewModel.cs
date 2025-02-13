using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Models
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        public int StatusId { get; set; }

        [Required]
        public int ProjectId { get; set; }

        public int? AssignedToId { get; set; }

        public DateTime? Deadline { get; set; }
    }
    public class TasksListViewModel
    {
        public IEnumerable<TaskViewModel> Tasks { get; set; } = new List<TaskViewModel>();
        public IEnumerable<ProjectViewModel> Projects { get; set; } = new List<ProjectViewModel>();
    }
}
