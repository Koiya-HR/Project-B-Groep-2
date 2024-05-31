using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Chair
{
    public int Row { get; set; }
    public int Col { get; set; }
    public bool Taken { get; set; }
}

public class Event
{
    public string Locatie { get; set; }
    public int EventID { get; set; }
    public string MovieID { get; set; }
    public List<Chair> Chairs { get; set; }
    public DateTime StartTijd { get; set; }
    public DateTime EindTijd { get; set; }

    private string Color { get; set; } = "\u001b[0m";
    private const string ResetColor = "\u001b[0m";
    private const string HighlightColor = "\u001b[38;2;250;156;55m";

    public void PrintControllInfo()
    {
        Console.WriteLine($"\u001b[38;2;250;156;55m=====================================================================================================================\u001b[0m");
        Console.WriteLine("Gebruik de \u001b[38;2;250;156;55mPIJL OMHOOG\u001b[0m en \u001b[38;2;250;156;55mOMLAAG\u001b[0m om door de lijst te gaan \ndruk \u001b[38;2;250;156;55mSPATIE\u001b[0m om te selecteren \ndruk \u001b[38;2;250;156;55mBACKSPACE\u001b[0m om terug te gaan");
        Console.WriteLine($"\u001b[38;2;250;156;55m=====================================================================================================================\u001b[0m");
    }

    public void PrintEvent(bool isHighlighted)
    {
        SetColor(isHighlighted);
        Console.WriteLine($"{Color}====================================================================================================================={ResetColor}");
        Console.WriteLine($"{HighlightColor}Start tijd:\u001b[0m {StartTijd:dd-MM-yyyy HH:mm:ss}");
        Console.WriteLine($"{HighlightColor}Eind tijd:\u001b[0m {EindTijd:dd-MM-yyyy HH:mm:ss}");
        Console.WriteLine($"{Color}====================================================================================================================={ResetColor}");
    }

    private void SetColor(bool isHighlighted)
    {
        if (isHighlighted)
        {
            Color = "\u001b[38;2;58;156;58m";
        }
        else
        {
            Color = ResetColor;
        }
    }
}

public class Location
{
    private static List<Event> Events { get; set; }

    public static void Main()
    {
        // Read events from JSON file
        ReadEventsFromJson();

        if (Events == null || Events.Count == 0)
        {
            Console.WriteLine("Geen evenementen beschikbaar.");
            return;
        }

        // Initialize variables
        ConsoleKeyInfo choice; // Stores user input
        int selectedIndex = 0; // Index of the selected option
        string[] locations = { "Den Haag", "Rotterdam", "Delft" };

        // Initial prompt
        Console.WriteLine("Op welk locatie zou u deze film willen bekijken?");
        do
        {
            // Display options and highlight selected option
            DisplayLocations(locations, selectedIndex);

            // Capture user input
            choice = Console.ReadKey(true);

            // Update selected index based on arrow keys
            if (choice.Key == ConsoleKey.UpArrow && selectedIndex > 0)
            {
                selectedIndex--;
            }
            else if (choice.Key == ConsoleKey.DownArrow && selectedIndex < locations.Length - 1)
            {
                selectedIndex++;
            }

            // Move cursor up to overwrite previous options
            Console.SetCursorPosition(0, Console.CursorTop - locations.Length);

        } while (choice.Key != ConsoleKey.Enter);

        // Clear console and reset variables
        Console.Clear();
        string selectedLocation = locations[selectedIndex];
        Console.WriteLine($"U hebt gekozen voor: {selectedLocation}");
        Console.WriteLine("Beschikbare evenementen:");

        // Filter and display events for the selected location
        var filteredEvents = Events.FindAll(e => e.Locatie.Equals(selectedLocation, StringComparison.OrdinalIgnoreCase));

        if (filteredEvents.Count == 0)
        {
            Console.WriteLine("Geen evenementen beschikbaar voor deze locatie.");
        }
        else
        {
            selectedIndex = 0; // Reset selected index for events
            do
            {
                // Print control info
                filteredEvents[selectedIndex].PrintControllInfo();

                // Display events and highlight selected event
                DisplayEvents(filteredEvents, selectedIndex);

                // Capture user input
                choice = Console.ReadKey(true);

                // Update selected index based on arrow keys
                if (choice.Key == ConsoleKey.UpArrow && selectedIndex > 0)
                {
                    selectedIndex--;
                }
                else if (choice.Key == ConsoleKey.DownArrow && selectedIndex < filteredEvents.Count - 1)
                {
                    selectedIndex++;
                }

                // Move cursor up to overwrite previous events
                int linesToClear = (filteredEvents.Count + 1) * 4 + 5;
                ClearLinesAbove(Console.CursorTop, linesToClear);

            } while (choice.Key != ConsoleKey.Enter);
        }
    }

    private static void DisplayLocations(string[] locations, int selectedIndex)
    {
        for (int i = 0; i < locations.Length; i++)
        {
            if (i == selectedIndex)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($"=> {locations[i]}");
            }
            else
            {
                Console.WriteLine($"   {locations[i]}");
            }
            Console.ResetColor();
        }
    }

    private static void DisplayEvents(List<Event> events, int selectedIndex)
    {
        for (int i = 0; i < events.Count; i++)
        {
            events[i].PrintEvent(i == selectedIndex);
        }
    }

    private static void ClearLinesAbove(int cursorTop, int linesToClear)
    {
        int currentLine = cursorTop - linesToClear;
        if (currentLine < 0)
        {
            currentLine = 0;
        }

        Console.SetCursorPosition(0, currentLine);
        for (int i = 0; i < linesToClear; i++)
        {
            Console.WriteLine(new string(' ', Console.WindowWidth));
        }

        Console.SetCursorPosition(0, currentLine);
    }

    public static void ReadEventsFromJson()
    {
        var filePath = "events.json";

        try
        {
            if (File.Exists(filePath))
            {
                var jsonData = File.ReadAllText(filePath);
                Events = JsonSerializer.Deserialize<List<Event>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
                });
            }
            else
            {
                Console.WriteLine("Events file not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the events file: {ex.Message}");
        }
    }
}
