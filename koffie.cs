using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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

            if (!Extras.isTimeLeft)
                return;

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
            Console.Clear();
            string bonnetjedata = File.ReadAllText("bonnetje.json");
            Bonnetje bonnetje = JsonConvert.DeserializeObject<Bonnetje>(bonnetjedata);
            bonnetje.Drankjes = [];
            string updatedbonnetje = JsonConvert.SerializeObject(bonnetje, Formatting.Indented);
            File.WriteAllText("bonnetje.json", updatedbonnetje);
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

            if (!Extras.isTimeLeft)
                return;

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
        UpdateBonnetjeJson(drinkCount, totalCost);
    }

    public static void ReadDrinksFromJson()
    {
        var filePath = "drinks.json";

        try
        {
            if (File.Exists(filePath))
            {
                var jsonData = File.ReadAllText(filePath);
                var drinkList = JsonConvert.DeserializeObject<DrinkList>(jsonData);
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
    private static void UpdateBonnetjeJson(Dictionary<string, int> drinkCount, double totalCost)
    {
        string bonnetjeFilePath = "bonnetje.json";


        try
        {
            if (File.Exists(bonnetjeFilePath))
            {
                // Read existing JSON content from the file
                string jsonContent = File.ReadAllText(bonnetjeFilePath);
                JObject bonnetjeObject;

                // Parse the JSON content into a JObject
                if (!string.IsNullOrWhiteSpace(jsonContent))
                {
                    bonnetjeObject = JObject.Parse(jsonContent);
                }
                else
                {
                    bonnetjeObject = new JObject();
                }

                // Create a JSON array for the drinks
                JArray drinksArray = new JArray();
                foreach (var drink in drinkCount)
                {
                    JObject drinkObject = new JObject
                {
                    { "Name", drink.Key },  // Change "Name" to match the JSON structure of drinks.json
                    { "Count", drink.Value }
                };
                    drinksArray.Add(drinkObject);
                }


                // Add the drinks array to the bonnetjeObject
                bonnetjeObject["Drankjes"] = drinksArray;


                // Add the total cost to the bonnetjeObject
                bonnetjeObject["Price"] = totalCost;


                // Write the updated JSON back to the file
                File.WriteAllText(bonnetjeFilePath, bonnetjeObject.ToString());
            }
            else
            {
                Console.WriteLine("Bonnetje file not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while updating the bonnetje file: {ex.Message}");
        }
    }
}


