using System;
using System.Collections.Generic;

namespace TaskManagerAPI.Entities;

public partial class Task
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int StatusId { get; set; }

    public int ProjectId { get; set; }

    public int? AssignedToId { get; set; }

    public DateTime? Deadline { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User? AssignedTo { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual TaskStatus Status { get; set; } = null!;
}
