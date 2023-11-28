// See https://aka.ms/new-console-template for more information

using System.Text;

int[] denominations = { 10, 50, 100 };

do
{
    Console.WriteLine("Available payout amounts:");
    Console.WriteLine("1. 30 EUR");
    Console.WriteLine("2. 50 EUR");
    Console.WriteLine("3. 60 EUR");
    Console.WriteLine("4. 80 EUR");
    Console.WriteLine("5. 140 EUR");
    Console.WriteLine("6. 230 EUR");
    Console.WriteLine("7. 370 EUR");
    Console.WriteLine("8. 610 EUR");
    Console.WriteLine("9. 980 EUR");
    Console.WriteLine("0. Exit");

    Console.Write("Choose a payout amount (0-9): ");
    if (int.TryParse(Console.ReadLine(), out int choice))
    {
        if (choice == 0)
        {
            Console.WriteLine("Exiting the program. Goodbye!");
            break;
        }

        if (choice >= 1 && choice <= 9)
        {
            int[] payoutAmounts = { 30, 50, 60, 80, 140, 230, 370, 610, 980 };
            int selectedAmount = payoutAmounts[choice - 1];

            Console.WriteLine($"You selected {selectedAmount} EUR.");
            Console.WriteLine($"Possible combinations for {selectedAmount} EUR:");

            var combinations = CalculateCombinations(selectedAmount, denominations);
            PrintCombinations(combinations);
        }
        else
        {
            Console.WriteLine("Invalid choice. Please enter a number between 0 and 9.");
        }
    }
    else
    {
        Console.WriteLine("Invalid input. Please enter a valid number.");
    }

    Console.WriteLine();
} while (true);

static List<List<int>> CalculateCombinations(int amount, int[] denominations)
{
    List<List<int>> result = new List<List<int>>();
    CalculateCombinationsRecursive(amount, denominations, 0, new List<int>(), result);
    return result;
}

static void CalculateCombinationsRecursive(int amount, int[] denominations, int index, List<int> currentCombination, List<List<int>> result)
{
    if (amount == 0)
    {
        result.Add(new List<int>(currentCombination));
        return;
    }

    for (var i = index; i < denominations.Length; i++)
    {
        if (amount >= denominations[i])
        {
            currentCombination.Add(denominations[i]);
            CalculateCombinationsRecursive(amount - denominations[i], denominations, i, currentCombination, result);
            currentCombination.RemoveAt(currentCombination.Count - 1);
        }
    }
}


static void PrintCombinations(List<List<int>> combinations)
{
    foreach (var combination in combinations)
    {
        var builder = new StringBuilder("Combination: ");

        var countByDenomination = combination
            .GroupBy(amount => amount)
            .ToDictionary(group => group.Key, group => group.Count());

        var isFirstDenomination = true;

        foreach (var kvp in countByDenomination.Where(kvp => kvp.Value > 0))
        {
            if (!isFirstDenomination)
            {
                builder.Append(" + ");
            }

            builder.Append($"{kvp.Value} x {kvp.Key} EUR");
            isFirstDenomination = false;
        }

        Console.WriteLine(builder.ToString());
    }
}