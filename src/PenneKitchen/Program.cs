class Program
{
	private static void Main(string[] args)
	{
		IRepository repo = new DatabaseRepository();
		Console.WriteLine("Kökets skärm");
	}
}
