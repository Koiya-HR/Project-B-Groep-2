//aangepast
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

    // private string maakStoelenList(object[] stoelenArray)
    // {
    //     return
    // }

    public static void betaalSysteem(List<(int row, int col)> selectedChairs, Stoel[,] stoelArray)
    {
        /*
        //=========================================================================================================
        // Read JSON from file
        ConsoleKeyInfo choice; // Stores user input
        int selectedIndex = 0; // Index of the selected option
        double totalCost = 0.0; // Total cost of the order

        string json = File.ReadAllText("bonnetje.json");

        // Deserialize JSON to Bonnetje object
        Bonnetje bonnetjeInfo = JsonSerializer.Deserialize<Bonnetje>(json);

        // Extract necessary information from the Bonnetje object
        string filmnaam = bonnetjeInfo.Filmnaam;
        DateTime starttijd = bonnetjeInfo.starttijd;
        DateTime eindtijd = bonnetjeInfo.eindtijd;
        string locatie = bonnetjeInfo.Locatie;
        List<string> stoelen = bonnetjeInfo.Stoelen;
        List<Drankjes> drankjes = bonnetjeInfo.Drankjes;
        double singlePrice = bonnetjeInfo.Price;

        // Format drink details
        string drankjesFormatted = string.Join("\n    ", drankjes.Select(d =>
        {
            double totalPriceDrink = d.Count * singlePrice;
            return $"x{d.Count} {d.Name,-15} €{d.Count * singlePrice,5:F2}";
        }));

        // Calculate total price
        double totalPrice = drankjes.Sum(d => d.Count * singlePrice);
        //=========================================================================================================
    */

        //=========================================================================================================
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

        var stoelenArray = root.GetProperty("Stoelen").EnumerateArray().ToList();
        List<string> formattedStoelen = new List<string>();

        // Loop through the seats and format them in pairs
        for (int i = 0; i < stoelenArray.Count; i += 2)
        {
            var firstSeat = stoelenArray[i].GetString().Split('-');
            string rij1 = firstSeat[0];
            string stoel1 = firstSeat[1];

            // Check if there is a second seat in the pair
            if (i + 1 < stoelenArray.Count)
            {
                var secondSeat = stoelenArray[i + 1].GetString().Split('-');
                string rij2 = secondSeat[0];
                string stoel2 = secondSeat[1];
                formattedStoelen.Add($"rij: {rij1} stoel: {stoel1}   rij: {rij2} stoel: {stoel2}");
            }
            else
            {
                // If there is no second seat, leave it empty
                formattedStoelen.Add($"rij: {rij1} stoel: {stoel1}");
            }
        }

        // Combine the formatted strings into a single string with new lines
        string stoelen = string.Join("\n    ", formattedStoelen);
        //=======================================================================================================


        /*
                string stoelen = string.Join("\n", stoelenArray
            .Select((s, index) =>
            {
                var split = s.GetString().Split('-');
                string rij = split[0];
                string stoel = split[1];

                // Calculate the column number for the current chair
                int columnNumber = (index % 4) + 1;

                // If it's the first chair in a new row, format it without indentation
                if (columnNumber == 1)
                {
                    return $"rij: {rij} stoel: {stoel}";
                }
                else
                {
                    // Append the chair to the current row with appropriate indentation
                    return $"    rij: {rij} stoel: {stoel}";
                }
            }));

            */



        // Extracting Drankjes array and format to show counts with prices


        var drankjesArray = root.GetProperty("Drankjes").EnumerateArray();
        string drankjes = string.Join("\n    ", drankjesArray.Select(d =>
        {
            int count = d.GetProperty("Count").GetInt32();
            string name = d.GetProperty("Name").GetString();
            double totalPriceDrink = count * singlePrice;
            return $"x{count} {name,-8} €{totalPriceDrink,5:F2}";
        }));

        // Calculate total price
        double totalPrice = drankjesArray.Sum(d => d.GetProperty("Count").GetInt32() * singlePrice);
        totalPrice += Extras.completePrijs;

        if (Extras.drankPrijs == 0)
        {
            Extras.completePrijs = Extras.ticketPrijs;
        }
        Console.Clear();
        StartScreen.DisplayAsciiArt();
        Console.WriteLine("\u001b[38;2;250;156;55m=====================================================================================================================\u001b[0m");
        Console.WriteLine($"Gebruik de \u001b[38;2;250;156;55mPIJLTOETSEN\u001b[0m om te navigeren door dit menu \nDruk \u001b[38;2;250;156;55mENTER\u001b[0m om te selecteren\nDruk op \u001b[38;2;250;156;55mF\u001b[0m om lidmaatschap te gebruiken");
        Console.WriteLine("\u001b[38;2;250;156;55m=====================================================================================================================\u001b[0m");
        Console.WriteLine($@"
=====================================================================================================================
Film: {filmnaam}                                                                                                     
Datum & Tijd: {starttijd}
Eind Tijd: {eindtijd}
Locatie: {locatie}
Stoelen:
    {stoelen}
Aantal Tickets:    {Extras.numTickets}
Prijs Tickets:     €{Extras.ticketPrijs,5:F2}
Drankjes:
    {drankjes}      
Totale Prijs:      €{Extras.completePrijs,5:F2}
=====================================================================================================================
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

            if (choice.KeyChar == 'f')
            {
                double huidigePrijs = Extras.completePrijs;
                double huidigeTicketPrijs = Extras.ticketPrijs;
                double huidigeDrankPrijs = Extras.drankPrijs;
                bool lidmToegepast = LidmaatschapInvoer.LidmaatschapInvoeren();
                if (lidmToegepast == true)
                {
                    double aantalEuroKorting = huidigePrijs - Extras.completePrijs;


                    Console.Clear();
                    StartScreen.DisplayAsciiArt();
                    Console.WriteLine();
                    Console.WriteLine("\u001b[38;2;250;156;55m=====================================================================================================================\u001b[0m");
                    Console.WriteLine($"Gebruik de \u001b[38;2;250;156;55mPIJLTOETSEN\u001b[0m om te navigeren door dit menu \nDruk \u001b[38;2;250;156;55mENTER\u001b[0m om te selecteren");
                    Console.WriteLine("\u001b[38;2;250;156;55m=====================================================================================================================\u001b[0m");

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Korting successvol toegepast!");
                    Console.ResetColor();

                    Console.WriteLine($@"
=====================================================================================================================
Film: {filmnaam}                                                                                                     
Datum & Tijd: {starttijd}
Eind Tijd: {eindtijd}
Locatie: {locatie}
Stoelen:
    {stoelen}
Aantal Tickets: {Extras.numTickets}
Prijs Tickets: €{huidigeTicketPrijs,5:F2}
Drankjes: 
{drankjes}
Prijs Drankjes: €{huidigeDrankPrijs,5:F2}
Korting: € - {aantalEuroKorting,5:F2}
Totale Prijs: €{Extras.completePrijs,5:F2}
=====================================================================================================================

        ");                  //string[] options = { "Naar betalen", "Bestelling annuleren" };
        }
    }

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

            int newPosition = Console.CursorTop - 2;
            if (newPosition >= 0)
            {
                Console.SetCursorPosition(0, newPosition);
            }



        } while (choice.Key != ConsoleKey.Enter);

        // If user selects "Bestelling annuleren", return
        if (selectedIndex == 1)
        {
            Extras.stopTimer = true;
            Reservation.CancelReservation();
            DeselectChairs(selectedChairs, stoelArray);
            Extras.completePrijs = 0.0;
            Extras.numTickets = 0;
            Extras.ticketPrijs = 0.0;
            Console.Clear();
            StartScreen.DisplayAsciiArt();
            Console.WriteLine();
            Console.WriteLine("\u001b[38;2;230;214;76mbestelling geannuleerd\u001b[0m");
            Console.WriteLine("Druk op een toets om terug te keren naar het hoofdmenu.");
            Console.ReadKey();
            return;
        }
        // Process payment or continue with other actions
        ProcessSelectedOption(options[selectedIndex], selectedChairs, stoelArray, filmnaam, starttijd, eindtijd, locatie, stoelen, drankjes, totalPrice, Extras.numTickets, Extras.ticketPrijs);
    }

    // Placeholder method to process the selected option
    static void ProcessSelectedOption(string selectedOption, List<(int row, int col)> selectedChairs, Stoel[,] stoelArray, string filmnaam, DateTime starttijd, DateTime eindtijd, string locatie, string stoelen, string drankjes, double totalPrice, int numTickets, double ticketPrijs)
    {
        if (selectedOption == "Naar betalen")
        {
            Extras.paymentSystem.SelectPaymentMethodAndConfirm(selectedOption, selectedChairs, stoelArray, filmnaam, starttijd, eindtijd, locatie, stoelen, drankjes, totalPrice, Extras.numTickets, Extras.ticketPrijs, Extras.completePrijs);
        }
        else if (selectedOption == "Bestelling annuleren")
        {
            Extras.stopTimer = true;
            Reservation.CancelReservation();
            DeselectChairs(selectedChairs, stoelArray);
            Extras.paymentSystem._onPaymentSuccess.Invoke();
            Extras.completePrijs = 0.0;
            Extras.numTickets = 0;
            Extras.ticketPrijs = 0.0;
            return;
        }
        return;
    }
}

