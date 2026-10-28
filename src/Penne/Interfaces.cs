interface IRepository
{
	List<Dish> GetAllDishes();
	void AddOrder(Order order);
}