using System;
using System.Collections.Generic;

namespace RPBDISlab2;

public partial class Tank
{
    public int TankId { get; set; }

    public string? TankType { get; set; }

    public float? TankVolume { get; set; }

    public float? TankWeight { get; set; }

    public string? TankMaterial { get; set; }

    public string? TankPicture { get; set; }

    public virtual ICollection<Operation> Operations { get; set; } = new List<Operation>();
}
