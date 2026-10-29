public class Order
{
	// eating here costs 10 kr extra per dish
	public const int EatHereFeePerDish = 10;

	public int OrderNumber { get; set; }
	public DateTime Created { get; set; } = DateTime.Now;
	public bool EatHere { get; set; }
	public string PaymentMethod { get; set; } = ""; // Borde vara enum!
	public List<Dish> Dishes { get; set; } = new List<Dish>();

	public void AddDish(Dish dish)
	{
		Dishes.Add(dish);
	}

	public int CalculateEatHereFee()
	{
		if (EatHere)
		{
			return Dishes.Count * EatHereFeePerDish;
		}
		return 0;
	}

	public int CalculateTotal()
	{
		int total = 0;
		foreach (Dish dish in Dishes)
		{
			total = total + dish.Price;
		}
		return total + CalculateEatHereFee();
	}
}
