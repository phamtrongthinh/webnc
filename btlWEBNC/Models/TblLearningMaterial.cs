using System;
using System.Collections.Generic;

namespace btlWEBNC.Models;

public partial class TblLearningMaterial
{
    public int MaterialId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int CourseId { get; set; }

    public int TeacherId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual TblCourse Course { get; set; } = null!;

    public virtual TblUser Teacher { get; set; } = null!;
}
