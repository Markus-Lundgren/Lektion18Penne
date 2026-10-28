using Microsoft.Data.Sqlite;
class DatabaseRepository : IRepository
{
	private List<Dish> _dishes = new List<Dish>();
	private List<Order> _orders = new List<Order>();
	private readonly SqliteConnection _db;
	public DatabaseRepository()
	{
		_db = new SqliteConnection("Data Source=Penne.db");
		_db.Open();
	}
	void IRepository.AddOrder(Order order)
	{
		throw new NotImplementedException();
	}

	public List<Dish> GetAllDishes()
	{
		using var getAll = _db.CreateCommand();
		getAll.CommandText = "SELECT * FROM Dishes";
		return _dishes;
	}
}