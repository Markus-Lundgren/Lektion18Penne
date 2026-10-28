Console.OutputEncoding = System.Text.Encoding.UTF8;

IRepository repository = new PenneRepository();
ConsoleUI ui = new ConsoleUI();

while (true)
{
	Order order = new Order();
	order.EatHere = ui.AskEatHere();

	ui.ChooseDishes(repository.GetAllDishes(), order);

	order.PaymentMethod = ui.TakePayment(order.CalculateTotal());

	repository.AddOrder(order);

	ui.PrintReceipt(order);
	ui.WaitForEnter("Tryck Enter för ny beställning");
}
