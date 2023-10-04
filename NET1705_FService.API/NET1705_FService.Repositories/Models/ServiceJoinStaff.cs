using System;
using System.Collections.Generic;

namespace NET1705_FService.Repositories.Models;

public partial class ServiceJoinStaff
{
    public int Id { get; set; }

    public int? ServiceId { get; set; }

    public int? StaffId { get; set; }

    public DateTime? WorkDate { get; set; }

    public DateTime? DateComplete { get; set; }

    public bool? IsComplete { get; set; }

    public string Feedback { get; set; }

    public int? Rate { get; set; }

    public virtual Service Service { get; set; }

    public virtual Staff Staff { get; set; }
}
