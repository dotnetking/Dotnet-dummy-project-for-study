using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class QualificationDetail
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public string Course { get; set; } = null!;

    public decimal? Percentage { get; set; }

    public int? YearOfPassing { get; set; }

    public string? University { get; set; }
}
