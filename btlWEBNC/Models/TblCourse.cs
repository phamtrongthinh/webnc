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

    public virtual ICollection<TblEnrollment> TblEnrollments { get; set; } = new List<TblEnrollment>();

    public virtual ICollection<TblLearningMaterial> TblLearningMaterials { get; set; } = new List<TblLearningMaterial>();

    public virtual TblUser? Teacher { get; set; }
}
