// keeps all dishes and orders in memory while the program is running
// only the operations the program actually needs are here
class PenneRepository : IRepository
{
	private List<Dish> dishes = new List<Dish>();
	private List<Order> orders = new List<Order>();
	private int nextOrderNumber = 101;

	public PenneRepository()
	{
		dishes.Add(new Dish(1, "Makaronilaatikko 2.0", "Finsk ugnsmakaronilåda med köttfärs i modern version", 149));
		dishes.Add(new Dish(2, "Kantarellikastike Penne", "Penne i krämig kantarellsås", 139));
		dishes.Add(new Dish(3, "Porkkana Fusilli", "Morotspasta i lätt smörsås med persilja", 119));
		dishes.Add(new Dish(4, "Puikulaperuna Gnocchi", "Gnocchi på Lapplands-potatisen Puikula", 159));
		dishes.Add(new Dish(5, "Korvapuustipasta", "Kanelbullepasta med kardemumma och glasyr", 79));
	}

	public List<Dish> GetAllDishes()
	{
		return dishes;
	}

	// saves the order and gives it the next order number
	public void AddOrder(Order order)
	{
		order.OrderNumber = nextOrderNumber;
		nextOrderNumber++;
		orders.Add(order);
	}
}
