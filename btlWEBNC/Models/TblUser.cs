using System;
using System.Collections.Generic;

namespace btlWEBNC.Models;

public partial class TblUser
{
    public int UserId { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Role { get; set; }

    public virtual ICollection<TblCourse> TblCourses { get; set; } = new List<TblCourse>();

    public virtual ICollection<TblEnrollment> TblEnrollments { get; set; } = new List<TblEnrollment>();

    public virtual ICollection<TblLearningMaterial> TblLearningMaterials { get; set; } = new List<TblLearningMaterial>();
}
