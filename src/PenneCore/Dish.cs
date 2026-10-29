public class Dish
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public int Price { get; set; }

	public Dish(int id, string name, string description, int price)
	{
		Id = id;
		Name = name;
		Description = description;
		Price = price;
	}
}
