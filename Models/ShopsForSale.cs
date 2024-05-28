namespace Practice.Models;

public partial class ShopsForSale
{
    public int IdShop { get; set; }

    public string Index { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string Building { get; set; } = null!;

    public string NameOfShop { get; set; } = null!;

    public decimal AreaOfTradingFloor { get; set; }

    public string TypeOfSale { get; set; } = null!;

    public string AvailabilityOfOrders { get; set; } = null!;

    public virtual ICollection<Florist> Florists { get; set; } = new List<Florist>();

	public ShopsForSale ShallowCopy()
	{
		return (ShopsForSale)this.MemberwiseClone();
	}
}
