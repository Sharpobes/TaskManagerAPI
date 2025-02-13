﻿using System;
using System.Collections.Generic;

namespace TaskManagerAPI.Entities;

public partial class Project
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int OwnerId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User Owner { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
