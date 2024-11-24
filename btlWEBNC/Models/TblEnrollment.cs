using System;
using System.Collections.Generic;

namespace btlWEBNC.Models;

public partial class TblEnrollment
{
    public int EnrollmentId { get; set; }

    public int? StudentId { get; set; }

    public int? CourseId { get; set; }

    public DateTime? EnrollmentDate { get; set; }

    public virtual TblCourse? Course { get; set; }

    public virtual TblUser? Student { get; set; }
}
