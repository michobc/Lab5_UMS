using System;
using System.Collections.Generic;

namespace LabSession5.Domain.Models;

public partial class Grade
{
    public long Id { get; set; }

    public long StudentId { get; set; }

    public long ClassId { get; set; }

    public double Value { get; set; }

    public virtual TeacherPerCourse Class { get; set; } = null!;

    public virtual User Student { get; set; } = null!;
}
