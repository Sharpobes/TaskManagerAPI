﻿using System;
using System.Collections.Generic;

namespace TaskManagerAPI.Entities;

public partial class TaskStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
