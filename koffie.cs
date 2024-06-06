using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Drink
{
    public string Name { get; set; }
    public double Price { get; set; }
}

public class DrinkList
{
    public List<Drink> Drinks { get; set; }
}

public class Koffie
{
    private static List<Drink> Drinks { get; set; }

    public static void Drank()
    {
        // Read drinks from JSON file
        ReadDrinksFromJson();

        if (Drinks == null || Drinks.Count == 0)
        {
            Console.WriteLine("Geen dranken beschikbaar.");
            return;
        }

        // Initialize variables
        ConsoleKeyInfo choice; // Stores user input
        int selectedIndex = 0; // Index of the selected option
        double totalCost = 0.0; // Total cost of the order
        Dictionary<string, int> drinkCount = new Dictionary<string, int>(); // Dictionary to store the count of each drink

        // Initial prompt
        Console.Clear();
        Console.WriteLine("Voordat u verder gaat met uw bestelling, wilt u nog iets te drinken bestellen?");
        do
        {
            // Display options and highlight selected option
            for (int i = 0; i < 2; i++)
            {
                if (i == selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.WriteLine(i == 0 ? "Ja" : "Nee");
                Console.ResetColor();
            }

            // Capture user input
            choice = Console.ReadKey(true);

            // Update selected index based on arrow keys
            if (choice.Key == ConsoleKey.UpArrow && selectedIndex > 0)
            {
                selectedIndex--;
            }
            else if (choice.Key == ConsoleKey.DownArrow && selectedIndex < 1)
            {
                selectedIndex++;
            }

            // Move cursor up to overwrite previous options
            Console.SetCursorPosition(0, Console.CursorTop - 2);

        } while (choice.Key != ConsoleKey.Enter);

        // If user selects "Nee", return
        if (selectedIndex == 1)
        {
            return;
        }

        // Clear console and reset variables
        Console.Clear();
        selectedIndex = 0;

        // Drink selection prompt
        Console.WriteLine("U kunt kiezen uit de volgende dranken, druk op \u001b[38;2;250;156;55mSpace\u001b[0m om te selecteren en \u001b[38;2;250;156;55mEnter\u001b[0m om akkoord te gaan, en \u001b[38;2;250;156;55mBackspace\u001b[0m om selectie te verwijderen");
        Console.WriteLine($"Totaal: {totalCost} Euro");
        Console.WriteLine("-----------------------------");

        do
        {
            // Display drink options and highlight selected option
            for (int i = 0; i < Drinks.Count; i++)
            {
                if (i == selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }

                // Display drink option with count and price
                int count = drinkCount.ContainsKey(Drinks[i].Name) ? drinkCount[Drinks[i].Name] : 0;
                Console.WriteLine($"{count} - {Drinks[i].Name} - {Drinks[i].Price} euro");
                Console.ResetColor();
            }

            // Capture user input
            choice = Console.ReadKey(true);

            // Update selected index based on arrow keys
            if (choice.Key == ConsoleKey.UpArrow && selectedIndex > 0)
            {
                selectedIndex--;
            }
            else if (choice.Key == ConsoleKey.DownArrow && selectedIndex < Drinks.Count - 1)
            {
                selectedIndex++;
            }

            // Move cursor up to overwrite previous options
            Console.SetCursorPosition(0, Console.CursorTop - Drinks.Count);

            // Process user selection
            if (choice.Key == ConsoleKey.Spacebar) // Add drink to order
            {
                string selectedDrink = Drinks[selectedIndex].Name;
                if (!drinkCount.ContainsKey(selectedDrink))
                {
                    drinkCount[selectedDrink] = 1;
                }
                else
                {
                    drinkCount[selectedDrink]++;
                }
                totalCost += Drinks[selectedIndex].Price;
                Console.Clear();
                // Display updated order
                Console.WriteLine("U kunt kiezen uit de volgende dranken:");
                Console.WriteLine("Uw bestelling:");
                foreach (KeyValuePair<string, int> pair in drinkCount)
                {
                    Console.WriteLine($"{pair.Value} - {pair.Key}");
                }
                Console.WriteLine($"Totaal: {totalCost} Euro");
                Console.WriteLine("-----------------------------");
            }
            else if (choice.Key == ConsoleKey.Backspace) // Remove drink from order
            {
                if (drinkCount.ContainsKey(Drinks[selectedIndex].Name))
                {
                    string selectedDrink = Drinks[selectedIndex].Name;
                    if (drinkCount[selectedDrink] > 1)
                    {
                        drinkCount[selectedDrink]--;
                    }
                    else
                    {
                        drinkCount.Remove(selectedDrink);
                    }
                    totalCost -= Drinks[selectedIndex].Price;
                    Console.Clear();
                    // Display updated order
                    Console.WriteLine("U kunt kiezen uit de volgende dranken:");
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

        // Clear console and display final order
        Console.Clear();
        Console.WriteLine("Uw bestelling:");
        foreach (KeyValuePair<string, int> pair in drinkCount)
        {
            Console.WriteLine($"{pair.Value} - {pair.Key}");
        }
        Console.WriteLine($"Totaal: {totalCost} Euro");
    }

    public static void ReadDrinksFromJson()
    {
        var filePath = "drinks.json";

        try
        {
            if (File.Exists(filePath))
            {
                var jsonData = File.ReadAllText(filePath);
                var drinkList = JsonSerializer.Deserialize<DrinkList>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                });
                Drinks = drinkList?.Drinks ?? new List<Drink>();
            }
            else
            {
                Console.WriteLine("Drinks file not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the drinks file: {ex.Message}");
        }
    }
}


// using System;
// using System.Collections.Generic;
// using Pathe_hr.obj;

// public class Koffie
// {
//     public static void Drank()
//     {

//         Console.Clear();
//         StartScreen.DisplayAsciiArt();
//         // Initialize variables
//         ConsoleKeyInfo choice; // Stores user input
//         int selectedIndex = 0; // Index of the selected option
//         string[] options = { "Ja", "Nee" }; // Options for the initial question
//         string[] options_3 = { "Koffie", "Cappuccino", "Espresso", "Latte", "Groene Thee", "Zwarte thee", "Kamillethee", "Muntthee", "Gemberthee", "Rooibosthee" }; // Options for drink selection


//         double drinkPrice = 2.0; // Price per drink
//         double totalCost = 0.0; // Total cost of the order
//         Dictionary<string, int> drinkCount = new Dictionary<string, int>(); // Dictionary to store the count of each drink

//         if (!Extras.isTimeLeft)
//             return;
//         // Initial prompt
//         Console.WriteLine("Voordat u verder gaat met uw bestelling, wilt u nog iets te drinken bestellen?");
//         do
//         {
//             // Display options and highlight selected option
//             for (int i = 0; i < options.Length; i++)
//             {
//                 if (i == selectedIndex)
//                 {
//                     Console.BackgroundColor = ConsoleColor.Gray;
//                     Console.ForegroundColor = ConsoleColor.Black;
//                 }
//                 Console.WriteLine(options[i]);
//                 Console.ResetColor();
//             }

//             // Capture user input
//             choice = Console.ReadKey(true);

//             // Update selected index based on arrow keys
//             if (choice.Key == ConsoleKey.UpArrow && selectedIndex > 0)
//             {
//                 selectedIndex--;
//             }
//             else if (choice.Key == ConsoleKey.DownArrow && selectedIndex < options.Length - 1)
//             {
//                 selectedIndex++;
//             }

//             // Move cursor up to overwrite previous options
//             Console.SetCursorPosition(0, Console.CursorTop - options.Length);

//         } while (choice.Key != ConsoleKey.Enter && Extras.isTimeLeft != false);
//         if (selectedIndex == 1)
//         {
//             return;
//         }
//         // Clear console and reset variables
//         Console.Clear();
//         selectedIndex = 0;
//         StartScreen.DisplayAsciiArt();
//         // Drink selection prompt

//         Console.WriteLine("U kunt kiezen uit de volgende dranken, druk op \u001b[38;2;250;156;55mSpace\u001b[0m om te selecteren en \u001b[38;2;250;156;55mEnter\u001b[0m om akkoord te gaan, en \u001b[38;2;250;156;55mBackspace\u001b[0m om selectie te verwijderen");
//         Console.WriteLine($"Totaal: {totalCost} Euro");
//         Console.WriteLine("-----------------------------");
//         do
//         {
//             // Display drink options and highlight selected option
//             for (int i = 0; i < options_3.Length; i++)
//             {
//                 if (i == selectedIndex)
//                 {
//                     Console.BackgroundColor = ConsoleColor.Gray;
//                     Console.ForegroundColor = ConsoleColor.Black;
//                 }

//                 // Display drink option with count and price

//                 int count = drinkCount.ContainsKey(options_3[i]) ? drinkCount[options_3[i]] : 0;
//                 Console.WriteLine($"{count} - {options_3[i]} - {drinkPrice} euro");
//                 Console.ResetColor();
//             }

//             // Capture user input
//             choice = Console.ReadKey(true);

//             // Update selected index based on arrow keys
//             if (choice.Key == ConsoleKey.UpArrow && selectedIndex > 0)
//             {
//                 selectedIndex--;
//             }
//             else if (choice.Key == ConsoleKey.DownArrow && selectedIndex < options_3.Length - 1)
//             {
//                 selectedIndex++;
//             }

//             // Move cursor up to overwrite previous options
//             Console.SetCursorPosition(0, Console.CursorTop - options_3.Length);

//             // Process user selection
//             if (choice.Key == ConsoleKey.Spacebar) // Add drink to order
//             {
//                 string selectedDrink = options_3[selectedIndex];
//                 if (!drinkCount.ContainsKey(selectedDrink))
//                 {
//                     drinkCount[selectedDrink] = 1;
//                 }
//                 else
//                 {
//                     drinkCount[selectedDrink]++;
//                 }
//                 totalCost += drinkPrice;
//                 Console.Clear();
//                 StartScreen.DisplayAsciiArt();
//                 // Display updated order

//                 Console.WriteLine("U kunt kiezen uit de volgende dranken, druk op \u001b[38;2;250;156;55mSpace\u001b[0m om te selecteren en \u001b[38;2;250;156;55mEnter\u001b[0m om akkoord te gaan, en \u001b[38;2;250;156;55mBackspace\u001b[0m om selectie te verwijderen");
//                 Console.WriteLine("Uw bestelling:");
//                 foreach (KeyValuePair<string, int> pair in drinkCount)
//                 {
//                     Console.WriteLine($"{pair.Value} - {pair.Key}");
//                 }
//                 Console.WriteLine($"Totaal: {totalCost} Euro");
//                 Console.WriteLine("-----------------------------");
//             }
//             else if (choice.Key == ConsoleKey.Backspace) // Remove drink from order
//             {
//                 if (drinkCount.ContainsKey(options_3[selectedIndex]))
//                 {
//                     string selectedDrink = options_3[selectedIndex];
//                     if (drinkCount[selectedDrink] > 1)
//                     {
//                         drinkCount[selectedDrink]--;
//                     }
//                     else
//                     {
//                         drinkCount.Remove(selectedDrink);
//                     }
//                     totalCost -= drinkPrice;
//                     Console.Clear();
//                     // Display updated order
//                     StartScreen.DisplayAsciiArt();
//                     Console.WriteLine("U kunt kiezen uit de volgende dranken, druk op 'Space' om te selecteren en 'Enter' om akkoord te gaan, en 'Backspace' om selectie te verwijderen");
//                     Console.WriteLine("Uw bestelling:");
//                     foreach (KeyValuePair<string, int> pair in drinkCount)
//                     {
//                         Console.WriteLine($"{pair.Value} - {pair.Key}");
//                     }
//                     Console.WriteLine($"Totaal: {totalCost} Euro");
//                     Console.WriteLine("-----------------------------");
//                 }
//             }

//         } while (choice.Key != ConsoleKey.Enter);

//         // Clear console and display final order
//         Console.Clear();
//         StartScreen.DisplayAsciiArt();
//         Console.WriteLine("Uw bestelling:");
//         foreach (KeyValuePair<string, int> pair in drinkCount)
//         {
//             Console.WriteLine($"{pair.Value} - {pair.Key}");
//         }
//         Console.WriteLine($"Totaal: {totalCost} Euro");
//     }
// }