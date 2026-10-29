using Microsoft.Data.Sqlite;
class DatabaseRepository : IRepository
{
	private List<Dish> _dishes = new List<Dish>();
	private List<Order> _orders = new List<Order>();
	private int nextOrderNumber = 101;
	public DatabaseRepository()
	{
		using var _db = new SqliteConnection("Data Source=Penne.db");
		_db.Open();

		using var createTables = _db.CreateCommand();

		createTables.CommandText = """
			CREATE TABLE IF NOT EXISTS Dishes(
				Id INTEGER PRIMARY KEY,
				Name TEXT,
				Description TEXT,
				Price INTEGER
			);
			CREATE TABLE IF NOT EXISTS PenneOrder(
				Id INTEGER PRIMARY KEY,
				OrderNumber INTEGER,
				Created DATE,
				EatHere BOOL,
				PaymentMethod TEXT
			);
			CREATE TABLE IF NOT EXISTS OrderKeeper(
				PenneOrderId INTEGER,
				DishId INTEGER
			);
		""";
		createTables.ExecuteNonQuery();

		using var getOrdernumber = _db.CreateCommand();
		getOrdernumber.CommandText = "SELECT OrderNumber from PenneOrder ORDER BY OrderNumber DESC LIMIT 1";

		using var reader = getOrdernumber.ExecuteReader();
		int orderNum = 0;
		while (reader.Read())
		{
			orderNum = reader.GetInt32(0);
		}

		if (orderNum > nextOrderNumber) nextOrderNumber = orderNum + 1;
		else if (orderNum == nextOrderNumber) nextOrderNumber++;
	}
	public void AddOrder(Order order)
	{
		using var _db = new SqliteConnection("Data Source=Penne.db");
		_db.Open();
		using var insertCommand = _db.CreateCommand();

		insertCommand.CommandText = """
			INSERT INTO PenneOrder(OrderNumber, Created, EatHere, PaymentMethod)
			VALUES($orderNumber, $created, $eatHere, $paymentMethod);
		""";
		order.OrderNumber = nextOrderNumber;
		nextOrderNumber++;

		insertCommand.Parameters.AddWithValue("$orderNumber", order.OrderNumber);
		insertCommand.Parameters.AddWithValue("$created", order.Created);
		insertCommand.Parameters.AddWithValue("$eatHere", order.EatHere);
		insertCommand.Parameters.AddWithValue("$paymentMethod", order.PaymentMethod);

		insertCommand.ExecuteNonQuery();

		using var getIdCommand = _db.CreateCommand();
		getIdCommand.CommandText = "SELECT Id from PenneOrder WHERE OrderNumber = $orderNumber";
		getIdCommand.Parameters.AddWithValue("$orderNumber", order.OrderNumber);
		using var reader = getIdCommand.ExecuteReader();
		int orderId = 0;
		while (reader.Read()) orderId = reader.GetInt32(0);

		using var orderKeeper = _db.CreateCommand();
		orderKeeper.CommandText = "INSERT INTO OrderKeeper(PenneOrderId, DishId) VALUES($orderId, $dishId)";
		orderKeeper.Parameters.AddWithValue("$orderId", orderId);
		var dishId = orderKeeper.CreateParameter();
		dishId.ParameterName = "$dishId";
		orderKeeper.Parameters.Add(dishId);

		foreach (Dish d in order.Dishes)
		{
			dishId.Value = d.Id;
			orderKeeper.ExecuteNonQuery();
		}
		_orders.Add(order);
	}

	public List<Dish> GetAllDishes()
	{
		if (_dishes.Count > 0) return _dishes;

		using var _db = new SqliteConnection("Data Source=Penne.db");
		_db.Open();
		using var getAll = _db.CreateCommand();
		getAll.CommandText = "SELECT * FROM Dishes";
		using var reader = getAll.ExecuteReader();

		while (reader.Read())
		{
			int id = reader.GetInt32(0);
			string name = reader.GetString(1);
			string desc = reader.GetString(2);
			int price = reader.GetInt32(3);

			_dishes.Add(new(id, name, desc, price));
		}
		return _dishes;
	}
}