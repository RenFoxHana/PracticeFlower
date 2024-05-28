namespace Practice.Models;

public partial class Plant
{
    public int IdPlant { get; set; }

    public string Name { get; set; } = null!;

    public string Color { get; set; } = null!;

    public string RequieredSoil { get; set; } = null!;

    public string FloweringPeriod { get; set; } = null!;

    public string FlowerShape { get; set; } = null!;

    public string FlowerSize { get; set; } = null!;

    public string ShelfLife { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<PlantsAssortmentFlorist> PlantsAssortmentFlorists { get; set; } = new List<PlantsAssortmentFlorist>();
}
