namespace Practice.Models;

public partial class Florist
{
    public int IdFlorist { get; set; }

    public string FName { get; set; } = null!;

    public string LName { get; set; } = null!;

    public string? Patronymic { get; set; }

    public int IdShop { get; set; }

    public int IdUser { get; set; }

    public virtual ICollection<FlowerFlorist> FlowerFlorists { get; set; } = new List<FlowerFlorist>();

    public virtual ShopsForSale IdShopNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<PlantsAssortmentFlorist> PlantsAssortmentFlorists { get; set; } = new List<PlantsAssortmentFlorist>();

	public Florist ShallowCopy()
	{
		return (Florist)this.MemberwiseClone();
	}
}
