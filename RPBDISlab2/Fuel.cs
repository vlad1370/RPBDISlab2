using System;
using System.Collections.Generic;

namespace RPBDISlab2;

public partial class Fuel
{
    public int FuelId { get; set; }

    public string? FuelType { get; set; }

    public float? FuelDensity { get; set; }

    public virtual ICollection<Operation> Operations { get; set; } = new List<Operation>();
}
