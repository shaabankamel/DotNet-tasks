internal class Program
{
    static void Main(string[] args)
    {
        string[] names = { "Keyboard", "Mouse", "Headset", "Monitor" };
        decimal[] prices = { 350m, 150m, 300m, 1200m };
        int[] cart = new int[names.Length];

        Console.Write("Enter your name: ");
        string customerName = FormatName(Console.ReadLine());

        PrintCatalog(names, prices);

        while (true)
        {
            Console.WriteLine("\n1. Add item");
            Console.WriteLine("2. Checkout");

            int choice = ReadValidInt("Choose: ");

            if (choice == 1)
            {
                int productNumber = ReadValidInt("Enter product number: ");
                int quantity = ReadValidInt("Enter quantity: ");

                if (productNumber <= names.Length)
                {
                    cart[productNumber - 1] += quantity;
                }
                else
                {
                    Console.WriteLine("Invalid product number.");
                }
            }
            else if (choice == 2)
            {
                break;
            }
            else
            {
                Console.WriteLine("Please choose 1 or 2.");
            }
        }

        decimal subtotal = CalculateSubtotal(names, prices, cart);
        decimal discountRate = GetDiscountRate(subtotal);

        PrintReceipt(
            customerName,
            names,
            prices,
            cart,
            subtotal,
            discountRate
        );
    }
    static string FormatName(string name)
    {
        while (string.IsNullOrWhiteSpace(name))
        {
            Console.Write("Please enter your name: ");
            name = Console.ReadLine()!;
        }

        name = name.Trim();
        name = char.ToUpper(name[0]) + name.Substring(1);

        return name;
    }
    static void PrintCatalog(string[] names, decimal[] prices)
    {
        for (int i = 0; i < names.Length; i++)
        {
            Console.WriteLine($"{i+1}. {names[i]} - {prices[i]:F2}");
        }
    }

    static int ReadValidInt(string message)
    {
        // TryParse + validation
        Console.WriteLine(message);
        string x = Console.ReadLine();
        while (true)
        {
            if(int.TryParse(x, out int number) &&number>0)
            {
                return number;
            }
            else
            {
                Console.Write(message);
                x= Console.ReadLine();
                Console.WriteLine();
            }
        }
    }

    static decimal CalculateSubtotal(
        string[] names,
        decimal[] prices,
        int[] cart)
    {
        // حسا
        decimal subtotal = 0;
        for (int i = 0; i < names.Length; i++)
        {
            subtotal += prices[i] * cart[i];
        }

        return subtotal;
    }

    static decimal GetDiscountRate(decimal subtotal)
    {
        return subtotal switch
        {

            >= 500 => 0.10m,
            >= 200 => 0.05m,
            _ => 0m
        };

    }

    static void PrintReceipt(
    string customerName,
    string[] names,
    decimal[] prices,
    int[] cart,
    decimal subtotal,
    decimal discountRate)
    {
        decimal discount = -(subtotal * discountRate);
        decimal total = subtotal + discount;

        Console.WriteLine($"Customer: {customerName}");

        int index = 0;

        foreach (int quantity in cart)
        {
            if (quantity > 0)
            {
                decimal itemTotal = prices[index] * quantity;

                Console.WriteLine(
                    $"{names[index]} x{quantity} = {itemTotal:F2}");
            }

            index++;
        }

        Console.WriteLine($"Subtotal:          {subtotal:F2}");
        Console.WriteLine(
            $"Discount ({discountRate:P0}):    {discount:F2}");
        Console.WriteLine($"Total:             {total:F2}");

        string shipping = total >= 500
            ? "FREE SHIPPING"
            : "Shipping: 50.00";

        Console.WriteLine(shipping);
    }
}
