using System;
using System.Collections.Generic;

namespace Practice.Models;

public partial class Flower
{
    public int IdFlower { get; set; }

    public string Name { get; set; } = null!;

    public string Color { get; set; } = null!;

    public string FlowerSize { get; set; } = null!;

    public string ShelfLife { get; set; } = null!;

    public int BouquetSize { get; set; }

    public string BouquetDesign { get; set; } = null!;

    public string Packaging { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<FlowerFlorist> FlowerFlorists { get; set; } = new List<FlowerFlorist>();
}
