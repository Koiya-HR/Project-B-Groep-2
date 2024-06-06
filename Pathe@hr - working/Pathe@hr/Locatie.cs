using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class Location
{
    private static List<Event>? Events { get; set; }

    public static bool chooseLocation()
    {
        // Read events from JSON file
        ReadEventsFromJson();

        if (Events == null || Events.Count == 0)
        {
            Console.WriteLine("Geen evenementen beschikbaar.");
            return false;
        }

        // Initialize variables
        ConsoleKeyInfo choice; // Stores user input
        int selectedIndex = 0; // Index of the selected option for locations
        int eventIndex = 0; // Index of the selected option for events on chosen location
        string[] locations = { "Den Haag", "Rotterdam", "Delft" };

        // Initial prompt

        StartScreen.DisplayAsciiArt();
        Console.WriteLine();
        Console.WriteLine($"Op welke locatie wilt u een reservering maken voor {Extras.gekozenFilm}");
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
            else if (choice.Key == ConsoleKey.Enter)
            {
                //door
            }

            else if (choice.Key == ConsoleKey.Escape)
            {
                //terug gaan naar film kiezen
                return false;
            }



            // Move cursor up to overwrite previous options
            Console.SetCursorPosition(0, Console.CursorTop - locations.Length);

        } while (choice.Key != ConsoleKey.Enter);

        // Clear console and reset variables
        Console.Clear();
        string selectedLocation = locations[selectedIndex];
        // Console.WriteLine($"U hebt gekozen voor: {selectedLocation}");
        // Console.WriteLine("Beschikbare evenementen:");

        // Filter and display events for the selected location
        List<Event> filteredEvents = Events.FindAll(e => e.Locatie!.Equals(selectedLocation, StringComparison.OrdinalIgnoreCase) &&
        e.MovieID!.Equals(Extras.gekozenFilm, StringComparison.OrdinalIgnoreCase));




        if (filteredEvents.Count == 0)
        {
            Console.Clear();
            StartScreen.DisplayAsciiArt();
            Console.WriteLine();
            Console.WriteLine($"Geen evenementen beschikbaar op deze locatie voor de film {Extras.gekozenFilm}.");
            Console.WriteLine("druk op een toets op terug te gaan...");
            Console.ReadKey();
            return false;
        }
        else
        {

            do
            {
                Console.Clear();
                StartScreen.DisplayAsciiArt();
                Console.WriteLine();
                Console.WriteLine($"Hoelaat Wilt u een reservering maken voor {Extras.gekozenFilm} op locatie {selectedLocation}");
                // Print control info
                filteredEvents[eventIndex].PrintControllInfo();

                // Display events and highlight selected event
                DisplayEvents(filteredEvents, eventIndex);

                // Capture user input
                choice = Console.ReadKey(true);

                // Update selected index based on arrow keys
                if (choice.Key == ConsoleKey.UpArrow && eventIndex > 0)
                {
                    eventIndex--;
                }
                else if (choice.Key == ConsoleKey.DownArrow && eventIndex < filteredEvents.Count - 1)
                {
                    eventIndex++;
                }
                else if (choice.Key == ConsoleKey.Enter)
                {
                    Extras.EventID = filteredEvents[eventIndex].EventID;
                    writeEventToBonnetje();
                }
                else if (choice.Key == ConsoleKey.Escape)
                {
                    //terug gaan naar film kiezen
                    return false;
                }

                // Move cursor up to overwrite previous events
                int linesToClear = (filteredEvents.Count + 1) * 4 + 5;
                ClearLinesAbove(Console.CursorTop, linesToClear);

            } while (choice.Key != ConsoleKey.Enter);
            return true;
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

    private static void DisplayEvents(List<Event> events, int eventIndex)
    {
        for (int i = 0; i < events.Count; i++)
        {
            events[i].PrintEvent(i == eventIndex);
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
        string filePath = "event.json";
        string jsonData = File.ReadAllText(filePath);
        Events = JsonConvert.DeserializeObject<List<Event>>(jsonData);
    }



    public static void writeEventToBonnetje()
    {
        DateTime startTijd;
        DateTime eindTijd;
        string locatie;

        for (int i = 0; i < Events.Count; i++)
        {
            if (Events[i].EventID == Extras.EventID)
            {
                startTijd = Events[i].StartTijd;
                eindTijd = Events[i].EindTijd;
                locatie = Events[i].Locatie;

                string bonnetjedata = File.ReadAllText("bonnetje.json");
                Bonnetje bonnetje = JsonConvert.DeserializeObject<Bonnetje>(bonnetjedata);

                bonnetje.starttijd = startTijd;
                bonnetje.eindtijd = eindTijd;
                bonnetje.Locatie = locatie;

                string updatedbonnetje = JsonConvert.SerializeObject(bonnetje, Formatting.Indented);
                File.WriteAllText("bonnetje.json", updatedbonnetje);
                break;
            }
        }
    }
}