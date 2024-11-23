using System;
using System.Collections.Generic;

namespace btlWEBNC.Models;

public partial class TblCourse
{
    public int CourseId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public double Price { get; set; }

    public int? TeacherId { get; set; }

    public virtual TblUser? Teacher { get; set; }
}
