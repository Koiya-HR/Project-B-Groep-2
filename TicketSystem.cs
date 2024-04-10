using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace MiniProj
{
    class Program
    {
        static List<Film> films = new List<Film>();
        static string path = "films.json";

        static void Main(string[] args)
        {
            // Admin inloggegevens
            string adminEmail = "admin12@gmail.com";
            string adminPassword = "admin12";
            string adminemail = "admin13@gmail.com";
            string adminpassword = "admin13";

            Console.WriteLine("Welkom bij het filmsysteem");

            bool isLoggedIn = false;
            while (!isLoggedIn)
            {
                // Inloggen als admin
                Console.Write("Voer uw e-mailadres in: ");
                string? inputEmail = Console.ReadLine();
                Console.Write("Voer uw wachtwoord in: ");
                string? inputPassword = Console.ReadLine();

                if (inputEmail == adminEmail && inputPassword == adminPassword || inputEmail == adminemail && inputPassword == adminpassword)
                {
                    Console.WriteLine("\nU bent nu ingelogd!");
                    Thread.Sleep(1000);
                    isLoggedIn = true;

                }
                else
                {
                    Console.WriteLine("\nOngeldige inloggegevens. Probeer het opnieuw.");
                }
            }

            // Als de gebruiker is ingelogd, toon het menu
            if (isLoggedIn)
            {
                ToonMenu();
            }
        }

        static void ToonMenu()
        {
            int selectedIndex = 0;
            bool doorgaan = true;
            string[] menuOptions = { "Film toevoegen", "Film bewerken", "Film verwijderen", "Uitloggen" };

            while (doorgaan)
            {
                Console.Clear();
                Console.WriteLine("\nKies een optie door middel van pijltjes en druk op Enter:");

                // Toon het menu met pijltjestoetsen
                for (int i = 0; i < menuOptions.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.Write("=> ");
                    }
                    else
                    {
                        Console.Write("   ");
                    }
                    Console.WriteLine(menuOptions[i]);
                }

                // Lees de toetsaanslag van de gebruiker
                ConsoleKeyInfo key = Console.ReadKey();

                // Verwerk de toetsaanslag
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = Math.Max(0, selectedIndex - 1);
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = Math.Min(menuOptions.Length - 1, selectedIndex + 1);
                        break;
                    case ConsoleKey.Enter:
                        Console.WriteLine("\nGeselecteerde optie: " + menuOptions[selectedIndex]);
                        switch (selectedIndex)
                        {
                            case 0:
                                VoegFilmToe();
                                break;
                            case 1:
                                BewerkFilm();
                                break;
                            case 2:
                                VerwijderFilm();
                                break;
                            case 3:
                                Console.WriteLine("\nU bent nu uitgelogd!");
                                doorgaan = false;
                                break;
                            default:
                                Console.WriteLine("\nOngeldige keuze. Probeer het opnieuw.");
                                break;
                        }
                        break;
                }
            }
        }

        static void VoegFilmToe()
        {
            Console.WriteLine("\nFilm toevoegen:");

            // Filmgegevens invoeren
            Console.Write("Naam van de film: ");
            string? naam = Console.ReadLine();
            Console.Write("Genre: ");
            string? genre = Console.ReadLine();
            Console.Write("Acteurs: ");
            string? acteurs = Console.ReadLine();
            Console.Write("Omschrijving: ");
            string? omschrijving = Console.ReadLine();
            Console.Write("Release datum: ");
            string? releaseDatum = Console.ReadLine();
            
        int duur;
        while (true)
        {
            Console.Write("Duur (in minuten): ");
            string inputDuur = Console.ReadLine();
            try
            {
                duur = Convert.ToInt32(inputDuur);
                break; // Als conversie succesvol is, stop de lus
            }
            catch (FormatException)
            {
                Console.WriteLine("Ongeldige invoer. Voer een getal in voor de duur.");
            }
        }

            // Nieuw filmobject maken
            Film nieuweFilm = new Film
            {
                Naam = naam,
                Genre = genre,
                Acteurs = acteurs,
                Omschrijving = omschrijving,
                ReleaseDatum = releaseDatum,
                Duur = duur
            };

            // Laden van bestaande films uit JSON-bestand
            if (File.Exists(path))
            {
                try
                {
                    string json = File.ReadAllText(path);
                    films = JsonConvert.DeserializeObject<List<Film>>(json) ?? new List<Film>(); // Initialiseer films indien deserialisatie null retourneert
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading films from JSON: {ex.Message}");
                    films = new List<Film>(); // Initialiseer films indien er een fout optreedt tijdens deserialisatie
                }
            }

            // Voeg de nieuwe film toe aan de lijst
            films.Add(nieuweFilm);

            // Converteer de lijst van films naar JSON
            string jsonFilms = JsonConvert.SerializeObject(films, Formatting.Indented);

            // Schrijf de JSON naar het bestand
            File.WriteAllText(path, jsonFilms);

            Console.WriteLine("\nFilm succesvol toegevoegd!");
            Thread.Sleep(1000);
        }

        static void BewerkFilm()
        {
            Console.WriteLine("\nFilm bewerken:");

            // Laden van films uit JSON-bestand
            if (File.Exists(path))
            {
                try
                {
                    string json = File.ReadAllText(path);
                    films = JsonConvert.DeserializeObject<List<Film>>(json) ?? new List<Film>(); // Initialiseer films indien deserialisatie null retourneert
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading films from JSON: {ex.Message}");
                    films = new List<Film>(); // Initialiseer films indien er een fout optreedt tijdens deserialisatie
                }
            }

            if (films.Count == 0)
            {
                Console.WriteLine("Er zijn geen films om te bewerken.");
                Thread.Sleep(1000);
                return;
            }

            int selectedIndex = 0; // Houdt de geselecteerde index van de film bij

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Kies de index van de film die u wilt bewerken:");
                for (int i = 0; i < films.Count; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.Write("=> ");
                    }
                    else
                    {
                        Console.Write("   ");
                    }
                    Console.WriteLine($"{i + 1}. {films[i].Naam}");
                }

                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.UpArrow)
                {
                    selectedIndex = Math.Max(0, selectedIndex - 1); // Verplaats de selectie omhoog, maar niet onder 0
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    selectedIndex = Math.Min(films.Count - 1, selectedIndex + 1); // Verplaats de selectie omlaag, maar niet voorbij het einde van de lijst
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    break; // Stop de lus zodra de gebruiker een film heeft geselecteerd
                }
            }

            // Geselecteerde film om te bewerken
            Film selectedFilm = films[selectedIndex];

            // Vraag de gebruiker welk veld ze willen bewerken
            Console.WriteLine("\nWat wilt u bewerken?");
            Console.WriteLine("Selecteer het nummer van het veld dat u wilt bewerken (1-5) en druk op Enter:");

            int fieldToEditIndex = 1; // Begin met het eerste veld geselecteerd

            while (true)
            {
                Console.Clear();
                Console.WriteLine("\nHuidige gegevens:");
                Console.WriteLine($"1. Naam: {selectedFilm.Naam}");
                Console.WriteLine($"2. Genre: {selectedFilm.Genre}");
                Console.WriteLine($"3. Acteurs: {selectedFilm.Acteurs}");
                Console.WriteLine($"4. Omschrijving: {selectedFilm.Omschrijving}");
                Console.WriteLine($"5. Release datum: {selectedFilm.ReleaseDatum}");
                Console.WriteLine($"6. Duur: {selectedFilm.Duur}");

                // Toon de pijltjes voor de geselecteerde index
                for (int i = 1; i <= 6; i++)
                {
                    if (i == fieldToEditIndex)
                    {
                        Console.Write("=> ");
                    }
                    else
                    {
                        Console.Write("   ");
                    }
                    Console.WriteLine(GetFieldName(i));
                }

                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.UpArrow)
                {
                    fieldToEditIndex = Math.Max(1, fieldToEditIndex - 1); // Verplaats de selectie omhoog, maar niet onder 1
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    fieldToEditIndex = Math.Min(6, fieldToEditIndex + 1); // Verplaats de selectie omlaag, maar niet voorbij 5
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    break; // Stop de lus zodra de gebruiker een veld heeft geselecteerd
                }
            }

            // Bewerk het geselecteerde veld van de geselecteerde film
            switch (fieldToEditIndex)
            {
                case 1:
                    Console.Write("Voer de nieuwe naam van de film in: ");
                    selectedFilm.Naam = Console.ReadLine();
                    break;
                case 2:
                    Console.Write("Voer het nieuwe genre van de film in: ");
                    selectedFilm.Genre = Console.ReadLine();
                    break;
                case 3:
                    Console.Write("Voer de nieuwe acteurs van de film in: ");
                    selectedFilm.Acteurs = Console.ReadLine();
                    break;
                case 4:
                    Console.Write("Voer de nieuwe omschrijving van de film in: ");
                    selectedFilm.Omschrijving = Console.ReadLine();
                    break;
                case 5:
                    Console.Write("Voer de nieuwe release datum van de film in: ");
                    selectedFilm.ReleaseDatum = Console.ReadLine();
                    break;
                case 6:
                    Console.Write("Voer de nieuwe duur van de film in (in minuten): ");
                    selectedFilm.Duur = Convert.ToInt32(Console.ReadLine()); // Bewerk duur
                    break;
            }

            // Converteer de bijgewerkte lijst van films naar JSON
            string updatedJsonFilms = JsonConvert.SerializeObject(films, Formatting.Indented);

            // Schrijf de bijgewerkte JSON naar het bestand
            File.WriteAllText(path, updatedJsonFilms);

            Console.WriteLine("\nFilm succesvol bijgewerkt!");
            Thread.Sleep(1000);
        }

        static void VerwijderFilm()
        {
            Console.WriteLine("\nFilm verwijderen:");

            if (File.Exists(path))
            {
                try
                {
                    string json = File.ReadAllText(path);
                    films = JsonConvert.DeserializeObject<List<Film>>(json) ?? new List<Film>(); // Initialiseer films indien deserialisatie null retourneert
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading films from JSON: {ex.Message}");
                    films = new List<Film>(); // Initialiseer films indien er een fout optreedt tijdens deserialisatie
                }
            }

            if (films.Count == 0)
            {
                Console.WriteLine("Er zijn geen films om te verwijderen.");
                Thread.Sleep(1000);
                return;
            }

            int selectedIndex = 0; // Houdt de geselecteerde index van de film bij

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Kies de index van de film die u wilt verwijderen:");
                for (int i = 0; i < films.Count; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.Write("=> ");
                    }
                    else
                    {
                        Console.Write("   ");
                    }
                    Console.WriteLine($"{i + 1}. {films[i].Naam}");
                }

                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.UpArrow)
                {
                    selectedIndex = Math.Max(0, selectedIndex - 1); // Verplaats de selectie omhoog, maar niet onder 0
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    selectedIndex = Math.Min(films.Count - 1, selectedIndex + 1); // Verplaats de selectie omlaag, maar niet voorbij het einde van de lijst
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    break; // Stop de lus zodra de gebruiker een film heeft geselecteerd
                }
            }

            int indexToRemove = selectedIndex;

            // Verwijder de geselecteerde film uit de lijst
            if (indexToRemove >= 0 && indexToRemove < films.Count)
            {
                films.RemoveAt(indexToRemove);

                // Converteer de bijgewerkte lijst van films naar JSON
                string updatedJsonFilms = JsonConvert.SerializeObject(films, Formatting.Indented);

                // Schrijf de bijgewerkte JSON naar het bestand
                File.WriteAllText(path, updatedJsonFilms);

                Console.WriteLine("\nFilm succesvol verwijderd!");
                Thread.Sleep(1000);
            }
            else
            {
                Console.WriteLine("\nOngeldige index. Film niet verwijderd.");
            }
        }

        static string GetFieldName(int index)
        {
            switch (index)
            {
                case 1:
                    return "Naam";
                case 2:
                    return "Genre";
                case 3:
                    return "Acteurs";
                case 4:
                    return "Omschrijving";
                case 5:
                    return "Release datum";
                case 6:
                    return "Duur";
                default:
                    return "";
            }
        }
    }
}
