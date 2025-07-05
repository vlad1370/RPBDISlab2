using System;
using System.Collections.Generic;

namespace RPBDISlab2;

public partial class ViewAllOperation
{
    public int OperationId { get; set; }

    public int? FuelId { get; set; }

    public int? TankId { get; set; }

    public float? IncExp { get; set; }

    public DateOnly? Date { get; set; }

    public string? FuelType { get; set; }

    public string? TankType { get; set; }
}
