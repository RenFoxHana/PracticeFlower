using System;
using System.Collections.Generic;

namespace Practice.Models;

public partial class FlowerFlorist
{
    public int IdFlowerShops { get; set; }

    public int? IdFlower { get; set; }

    public int? IdFlorist { get; set; }

    public virtual Florist? IdFloristNavigation { get; set; }

    public virtual Flower? IdFlowerNavigation { get; set; }
}
