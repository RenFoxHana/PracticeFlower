using System;
using System.Collections.Generic;

namespace Practice.Models;

public partial class PlantsAssortmentFlorist
{
    public int IdPlantShopAssortment { get; set; }

    public int? IdPlant { get; set; }

    public int? IdAssortment { get; set; }

    public int? IdFlorist { get; set; }

    public virtual Assortment? IdAssortmentNavigation { get; set; }

    public virtual Florist? IdFloristNavigation { get; set; }

    public virtual Plant? IdPlantNavigation { get; set; }
}
