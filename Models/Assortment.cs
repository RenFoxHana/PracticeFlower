namespace Practice.Models;

public partial class Assortment
{
    public int IdAssortment { get; set; }

    public string Name { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<PlantsAssortmentFlorist> PlantsAssortmentFlorists { get; set; } = new List<PlantsAssortmentFlorist>();
}
