using System;
using System.Collections.Generic;

namespace RPBDISlab2;

public partial class Operation
{
    public int OperationId { get; set; }

    public int? FuelId { get; set; }

    public int? TankId { get; set; }

    public float? IncExp { get; set; }

    public DateOnly? Date { get; set; }

    public virtual Fuel? Fuel { get; set; }

    public virtual Tank? Tank { get; set; }
}
