using System;
using System.Collections.Generic;

public class Koffie
{
    public static void Drank()
    {
        ConsoleKeyInfo choice;
        int selectedIndex = 0;
        string[] options = { "Ja", "Nee" };
        string[] options_3 = { "Koffie", "Cappuccino", "Espresso", "Latte", "Groene Thee", "Zwarte thee", "Kamillethee", "Muntthee", "Gemberthee", "Rooibosthee" };

        double drinkPrice = 2.0;
        double totalCost = 0.0;
        Dictionary<string, int> drinkCount = new Dictionary<string, int>();

        Console.WriteLine("Voordat u verder gaat met uw bestelling, wilt u nog iets te drinken bestellen?");
        do
        {
            for (int i = 0; i < options.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.WriteLine(options[i]);
                Console.ResetColor();
            }

            choice = Console.ReadKey(true);

            if (choice.Key == ConsoleKey.UpArrow && selectedIndex > 0)
            {
                selectedIndex--;
            }
            else if (choice.Key == ConsoleKey.DownArrow && selectedIndex < options.Length - 1)
            {
                selectedIndex++;
            }

            Console.SetCursorPosition(0, Console.CursorTop - options.Length);

        } while (choice.Key != ConsoleKey.Enter);

        Console.Clear();
        selectedIndex = 0;
        Console.WriteLine("U kunt kiezen uit de volgende dranken, druk op 'Space' om te selecteren en 'Enter' om akkoord te gaan, en 'Backspace' om selectie te verwijderen");
        Console.WriteLine($"Totaal: {totalCost} Euro");
        Console.WriteLine("-----------------------------");
        do
        {
            for (int i = 0; i < options_3.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }

                int count = drinkCount.ContainsKey(options_3[i]) ? drinkCount[options_3[i]] : 0;
                Console.WriteLine($"{count} - {options_3[i]} - {drinkPrice} euro");
                Console.ResetColor();
            }

            choice = Console.ReadKey(true);

            if (choice.Key == ConsoleKey.UpArrow && selectedIndex > 0)
            {
                selectedIndex--;
            }
            else if (choice.Key == ConsoleKey.DownArrow && selectedIndex < options_3.Length - 1)
            {
                selectedIndex++;
            }

            Console.SetCursorPosition(0, Console.CursorTop - options_3.Length);

            if (choice.Key == ConsoleKey.Spacebar)
            {
                string selectedDrink = options_3[selectedIndex];
                if (!drinkCount.ContainsKey(selectedDrink))
                {
                    drinkCount[selectedDrink] = 1;
                }
                else
                {
                    drinkCount[selectedDrink]++;
                }
                totalCost += drinkPrice;
                Console.Clear();
                Console.WriteLine("U kunt kiezen uit de volgende dranken, druk op 'Space' om te selecteren en 'Enter' om akkoord te gaan, en 'Backspace' om selectie te verwijderen");
                Console.WriteLine("Uw bestelling:");
                foreach (KeyValuePair<string, int> pair in drinkCount)
                {
                    Console.WriteLine($"{pair.Value} - {pair.Key}");
                }
                Console.WriteLine($"Totaal: {totalCost} Euro");
                Console.WriteLine("-----------------------------");
            }
            else if (choice.Key == ConsoleKey.Backspace)
            {
                if (drinkCount.ContainsKey(options_3[selectedIndex]))
                {
                    string selectedDrink = options_3[selectedIndex];
                    if (drinkCount[selectedDrink] > 1)
                    {
                        drinkCount[selectedDrink]--;
                    }
                    else
                    {
                        drinkCount.Remove(selectedDrink);
                    }
                    totalCost -= drinkPrice;
                    Console.Clear();
                    Console.WriteLine("U kunt kiezen uit de volgende dranken, druk op 'Space' om te selecteren en 'Enter' om akkoord te gaan, en 'Backspace' om selectie te verwijderen");
                    Console.WriteLine("Uw bestelling:");
                    foreach (KeyValuePair<string, int> pair in drinkCount)
                    {
                        Console.WriteLine($"{pair.Value} - {pair.Key}");
                    }
                    Console.WriteLine($"Totaal: {totalCost} Euro");
                    Console.WriteLine("-----------------------------");
                }
            }

        } while (choice.Key != ConsoleKey.Enter);

        Console.Clear();

        Console.WriteLine("Uw bestelling:");
        foreach (KeyValuePair<string, int> pair in drinkCount)
        {
            Console.WriteLine($"{pair.Value} - {pair.Key}");
        }
        Console.WriteLine($"Totaal: {totalCost} Euro");
    }
}
