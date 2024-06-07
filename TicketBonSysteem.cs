using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Pathe_hr.obj;

public class TicketBonSystem
{

    public static void DeselectChairs(List<(int row, int col)> selectedChairs, Stoel[,] stoelArray)
    {
        foreach (var chair in selectedChairs)
        {
            stoelArray[chair.row, chair.col].selected = false;
            stoelArray[chair.row, chair.col].free = true;
        }
        selectedChairs.Clear();
    }

    public static void betaalSysteem(List<(int row, int col)> selectedChairs, Stoel[,] stoelArray)
    {
        ConsoleKeyInfo choice; // Stores user input
        int selectedIndex = 0; // Index of the selected option
        double totalCost = 0.0; // Total cost of the order
        // Assuming the JSON is stored in a file named "bonnetje.json"
        string json = File.ReadAllText("bonnetje.json");

        // Parse the JSON string
        var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        // Extract values from the JSON
        string filmnaam = root.GetProperty("Filmnaam").GetString();
        DateTime starttijd = root.GetProperty("starttijd").GetDateTime();
        DateTime eindtijd = root.GetProperty("eindtijd").GetDateTime();
        string locatie = root.GetProperty("Locatie").GetString();

        // Define the single price for each drink
        double singlePrice = 2.0;

        // Extracting Stoelen array and format to display each on a new line
        var stoelenArray = root.GetProperty("Stoelen").EnumerateArray();
        string stoelen = string.Join("\n    ", stoelenArray.Select(s =>
        {
            var split = s.GetString().Split('-');
            string rij = split[0];
            string stoel = split[1];
            return $"rij: {rij} stoel: {stoel}";
        }));

        // Extracting Drankjes array and format to show counts with prices
        var drankjesArray = root.GetProperty("Drankjes").EnumerateArray();
        string drankjes = string.Join("\n    ", drankjesArray.Select(d =>
        {
            int count = d.GetProperty("Count").GetInt32();
            string name = d.GetProperty("Name").GetString();
            double totalPriceDrink = count * singlePrice;
            return $"x{count} {name,-15} €{totalPriceDrink,5:F2}";
        }));

        // Calculate total price
        double totalPrice = drankjesArray.Sum(d => d.GetProperty("Count").GetInt32() * singlePrice);

        Console.WriteLine($@"
==================================================================================================================================================
Film: {filmnaam}
Datum & Tijd: {starttijd}
Eind Tijd: {eindtijd}
Locatie: {locatie}
Stoelen:
    {stoelen}
Drankjes:
    {drankjes}
                Totale Prijs: €{totalPrice,5:F2}
==================================================================================================================================================
        ");

        // Menu options
        string[] options = { "Naar betalen", "Bestelling annuleren" };

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
                Console.WriteLine(options[i]);
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

        // If user selects "Bestelling annuleren", return
        if (selectedIndex == 1)
        {
            Extras.stopTimer = true;
            Reservation.CancelReservation();
            DeselectChairs(selectedChairs, stoelArray);
            return;
        }

        // Process payment or continue with other actions
        ProcessSelectedOption(options[selectedIndex], selectedChairs, stoelArray);
    }

    // Placeholder method to process the selected option
    static void ProcessSelectedOption(string selectedOption, List<(int row, int col)> selectedChairs, Stoel[,] stoelArray)
    {
        if (selectedOption == "Naar betalen")
        {
            Extras.paymentSystem.SelectPaymentMethodAndConfirm(selectedChairs, stoelArray);
        }
        else if (selectedOption == "Bestelling annuleren")
        {
            Extras.stopTimer = true;
            Reservation.CancelReservation();
            DeselectChairs(selectedChairs, stoelArray);
            Extras.paymentSystem._onPaymentSuccess.Invoke();
            return;
        }
        return;
    }
}
