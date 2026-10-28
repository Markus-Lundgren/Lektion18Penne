// all reading from and writing to the console is done here
class ConsoleUI
{
    public bool AskEatHere()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("****************************************");
            Console.WriteLine("*        VÄLKOMMEN TILL PENNE!         *");
            Console.WriteLine("*    Finsk pasta - Suomalainen pasta   *");
            Console.WriteLine("****************************************");
            Console.WriteLine();
            Console.WriteLine("1. Äta här (+" + Order.EatHereFeePerDish + " kr per rätt)");
            Console.WriteLine("2. Ta med");
            string choice = Ask("Välj: ");

            if (choice == "1")
            {
                return true;
            }
            if (choice == "2")
            {
                return false;
            }
            WaitForEnter("Fel! Du måste skriva 1 eller 2");
        }
    }

    // shows the menu and adds dishes to the order until the customer is done
    public void ChooseDishes(List<Dish> menu, Order order)
    {
        while (true)
        {
            Console.Clear();
            PrintMenu(menu);
            PrintOrder(order);

            string answer = Ask("Välj rätt: ");
            int number;
            if (int.TryParse(answer, out number) == false)
            {
                WaitForEnter("Du måste skriva en siffra!");
            }
            else if (number == 0)
            {
                if (order.Dishes.Count > 0)
                {
                    return;
                }
                WaitForEnter("Du har inte valt något!");
            }
            else if (number > 0 && number <= menu.Count)
            {
                Dish dish = menu[number - 1];
                order.AddDish(dish);
                Console.WriteLine(dish.Name + " har lagts till i din beställning!");
                Thread.Sleep(1000);
            }
            else
            {
                WaitForEnter("Den rätten finns inte!");
            }
        }
    }

    // returns the payment method when the customer has paid
    public string TakePayment(int total)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Att betala: " + total + " kr");
            Console.WriteLine();
            Console.WriteLine("1. Betala med kort");
            Console.WriteLine("2. Betala kontant");
            string choice = Ask("Välj: ");

            if (choice == "1" && PayWithCard())
            {
                return "Kort";
            }
            if (choice == "2" && PayWithCash(total))
            {
                return "Kontant";
            }
        }
    }

    private bool PayWithCard()
    {
        // TODO: use Strajp.dll
        string cardNumber = Ask("Skriv ditt kortnummer (16 siffror): ");
        if (cardNumber.Length != 16)
        {
            WaitForEnter("Fel kortnummer!");
            return false;
        }

        Ask("Skriv din PIN-kod: ");
        Console.WriteLine("Kontaktar banken...");
        Thread.Sleep(2000);
        WaitForEnter("Betalningen är godkänd!");
        return true;
    }

    private bool PayWithCash(int total)
    {
        int paid;
        if (int.TryParse(Ask("Hur mycket betalar du? "), out paid) == false)
        {
            WaitForEnter("Du måste skriva en siffra!");
            return false;
        }
        if (paid < total)
        {
            WaitForEnter("Det räcker inte! Det fattas " + (total - paid) + " kr");
            return false;
        }

        WaitForEnter("Du får tillbaka " + (paid - total) + " kr");
        return true;
    }

    public void PrintReceipt(Order order)
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("                 KVITTO");
        Console.WriteLine("========================================");
        Console.WriteLine("Ordernummer: " + order.OrderNumber);
        Console.WriteLine(order.Created);
        if (order.EatHere)
        {
            Console.WriteLine("Äta här");
        }
        else
        {
            Console.WriteLine("Ta med");
        }
        Console.WriteLine("----------------------------------------");
        foreach (Dish dish in order.Dishes)
        {
            Console.WriteLine(dish.Name + "   " + dish.Price + " kr");
        }
        if (order.EatHere)
        {
            Console.WriteLine("Äta här-tillägg   " + order.CalculateEatHereFee() + " kr");
        }
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("TOTALT: " + order.CalculateTotal() + " kr");
        Console.WriteLine("Betalat med: " + order.PaymentMethod);
        Console.WriteLine("========================================");
        Console.WriteLine("Tack för ditt köp! Kiitos!");
        Console.WriteLine();
    }

    public void WaitForEnter(string message)
    {
        Console.WriteLine(message);
        Console.ReadLine();
    }

    private void PrintMenu(List<Dish> menu)
    {
        Console.WriteLine("================ MENY ================");
        for (int i = 0; i < menu.Count; i++)
        {
            Console.WriteLine(i + 1 + ". " + menu[i].Name + " - " + menu[i].Price + " kr");
            Console.WriteLine("   " + menu[i].Description);
        }
        Console.WriteLine("0. Klar, gå till betalning");
        Console.WriteLine();
    }

    private void PrintOrder(Order order)
    {
        Console.WriteLine("Din beställning:");
        foreach (Dish dish in order.Dishes)
        {
            Console.WriteLine("- " + dish.Name + " " + dish.Price + " kr");
        }
        Console.WriteLine("Totalt: " + order.CalculateTotal() + " kr");
        Console.WriteLine();
    }

    // Console.ReadLine() returns null when the input ends, so we return "" instead
    private string Ask(string question)
    {
        Console.Write(question);
        return Console.ReadLine() ?? "";
    }
}
