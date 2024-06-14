using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using System.Globalization;
// aangepast
public class AdminSystem
{
    static List<Film> films = new List<Film>();
    static string path = "films.json";

    public static void Inlogscherm()
    {
        string adminEmail = "admin12@gmail.com";
        string adminPassword = "admin12";

        bool isLoggedIn = false;
        Console.Clear();
        int selectedIndex = 0;
        string[] options = { "Doorgaan met inloggen", "Terug naar het hoofdmenu" };

        while (true)
        {
            Console.Clear();
            StartScreen.DisplayAsciiArt();
            Console.WriteLine("\u001b[38;2;250;156;55mAdministator systeem\u001b[0m");
            Console.WriteLine("Gebruik de \u001b[38;2;250;156;55mpijltjestoetsen\u001b[0m om te navigeren en druk op \u001b[38;2;250;156;55mEnter\u001b[0m om te selecteren:");
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

            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.UpArrow)
            {
                selectedIndex = Math.Max(0, selectedIndex - 1);
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                selectedIndex = Math.Min(options.Length - 1, selectedIndex + 1);
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                if (selectedIndex == 0)
                {
                    break;
                }
                else if (selectedIndex == 1)
                {
                    return;
                }
            }
        }

        Console.Clear();

        while (true)
        {
            StartScreen.DisplayAsciiArt();
            Console.WriteLine("\u001b[38;2;250;156;55mAdministator systeem\u001b[0m");
            Console.WriteLine("Welkom bij het Adminsysteem!");
            Console.WriteLine("Voer uw e-mailadres in en druk op \u001b[38;2;250;156;55mEnter\u001b[0m om uw wachtwoord in te voeren: ");
            string? inputEmail = Console.ReadLine();

            // Controleer of email niet leeg is 
            if (string.IsNullOrWhiteSpace(inputEmail))
            {
                Console.Write("E-mailadres mag niet leeg zijn. Druk op een toets om opnieuw te proberen.");
                Console.ReadKey();
                Console.Clear();
                continue; // Lus wordt opnieuw geroepen
            }

            Console.WriteLine("Voer uw wachtwoord in en druk op \u001b[38;2;250;156;55mEnter\u001b[0m om door te gaan: ");
            string? inputPassword = Console.ReadLine();

            // Controleer of wachtwoord niet leeg is
            if (string.IsNullOrWhiteSpace(inputPassword))
            {
                Console.Write("Wachtwoord mag niet leeg zijn. Druk op een toets om opnieuw te proberen.");
                Console.ReadKey();
                Console.Clear();
                continue; // Lus wordt opnieuw geroepen
            }

            if (inputEmail == adminEmail && inputPassword == adminPassword)
            {
                Console.WriteLine("\nU bent nu ingelogd!");
                Thread.Sleep(1000);
                isLoggedIn = true;
                break; // Uit de lus als inloggegevens correct zijn
            }
            else
            {
                Console.WriteLine("\nOngeldige inloggegevens. Wat wilt u doen?");
                int retrySelectedIndex = 0;
                string[] retryOptions = { "Opnieuw inloggen", "Terug naar het hoofdmenu" };

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Ongeldige inloggegevens. Wat wilt u doen?");
                    Console.WriteLine("Gebruik de \u001b[38;2;250;156;55mpijltjestoetsen\u001b[0m om te navigeren en druk op \u001b[38;2;250;156;55mEnter\u001b[0m om te selecteren:");
                    for (int i = 0; i < retryOptions.Length; i++)
                    {
                        if (i == retrySelectedIndex)
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        Console.WriteLine(retryOptions[i]);
                        Console.ResetColor();
                    }

                    ConsoleKeyInfo key2 = Console.ReadKey(true);
                    if (key2.Key == ConsoleKey.UpArrow)
                    {
                        retrySelectedIndex = Math.Max(0, retrySelectedIndex - 1);
                    }
                    else if (key2.Key == ConsoleKey.DownArrow)
                    {
                        retrySelectedIndex = Math.Min(retryOptions.Length - 1, retrySelectedIndex + 1);
                    }
                    else if (key2.Key == ConsoleKey.Enter)
                    {
                        if (retrySelectedIndex == 0)
                        {
                            Console.Clear();
                            break; // Opnieuw inloggen
                        }
                        else
                        {
                            return; // Terug naar het hoofdmenu
                        }
                    }
                }
            }
        }

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
            films = LoadFilms();

            Console.Clear();
            StartScreen.DisplayAsciiArt();
            Console.WriteLine("\u001b[38;2;250;156;55mAdministator systeem\u001b[0m");
            Console.WriteLine("Gebruik de \u001b[38;2;250;156;55mpijltjestoetsen\u001b[0m om te navigeren en druk op \u001b[38;2;250;156;55mEnter\u001b[0m om te selecteren:");
            for (int i = 0; i < menuOptions.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("=> ");
                }
                else
                {
                    Console.Write("   ");
                }
                Console.WriteLine(menuOptions[i]);
                Console.ResetColor();
            }

            ConsoleKeyInfo key = Console.ReadKey();

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
                            Thread.Sleep(1000);
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
        Console.Clear();
        StartScreen.DisplayAsciiArt();
        Console.WriteLine("\u001b[38;2;250;156;55mAdministator systeem\u001b[0m");
        int selectedIndex = 0;
        string[] options = { "Doorgaan met toevoegen van een film", "Terug naar het hoofdmenu" };

        while (true)

        {
            Console.Clear();
            StartScreen.DisplayAsciiArt();
            Console.WriteLine("\u001b[38;2;250;156;55mAdministator systeem\u001b[0m");
            Console.WriteLine("Wat wilt u doen?");
            Console.WriteLine("Gebruik de \u001b[38;2;250;156;55mpijltjestoetsen\u001b[0m om te navigeren en druk op \u001b[38;2;250;156;55mEnter\u001b[0m om te selecteren:");
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

            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.UpArrow)
            {
                selectedIndex = Math.Max(0, selectedIndex - 1);
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                selectedIndex = Math.Min(options.Length - 1, selectedIndex + 1);
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                if (selectedIndex == 0)
                {
                    break;
                }
                else if (selectedIndex == 1)
                {
                    return;
                }
            }
            Console.Clear();
            StartScreen.DisplayAsciiArt();
            Console.WriteLine("\u001b[38;2;250;156;55mAdministator systeem\u001b[0m");
        }

        Console.WriteLine("\nFilm toevoegen:");

        Console.Write("\u001b[38;2;250;156;55mVOER\u001b[0m de titel van de film in en druk op \u001b[38;2;250;156;55mEnter\u001b[0m: ");
        string? naam = Console.ReadLine();

        // Genres invoeren
        List<string> genres;
        while (true)
        {
            Console.Write("\u001b[38;2;250;156;55mVOER\u001b[0m het genre (gescheiden door komma's) en druk op \u001b[38;2;250;156;55mEnter\u001b[0m: ");
            Console.WriteLine("Voorbeeld: Drama, Actie");
            string? genreInput = Console.ReadLine();
            genres = genreInput.Split(',').Select(g => g.Trim()).ToList();

            // Controleren of er minimaal twee genres zijn ingevoerd
            if (genres.Count >= 2)
            {
                break;
            }
            else
            {
                Console.WriteLine("\u001b[38;2;250;156;55mVOER\u001b[0m minimaal twee genres gescheiden door komma's in en druk op \u001b[38;2;250;156;55mEnter\u001b[0m: ");
            }
        }

        // Acteurs invoeren
        List<string> acteurs;
        while (true)
        {
            Console.Write("\u001b[38;2;250;156;55mVOER\u001b[0m de acteurs (gescheiden door komma's) en druk op \u001b[38;2;250;156;55mEnter\u001b[0m: ");
            Console.WriteLine("Voorbeeld: Dwayne Johnson, Sylvester Stallone");
            string acteursInput = Console.ReadLine();
            acteurs = acteursInput.Split(',').Select(a => a.Trim()).ToList();

            // Controleren of er minimaal twee acteurs zijn ingevoerd
            if (acteurs.Count >= 2)
            {
                break;
            }
            else
            {
                Console.WriteLine("\u001b[38;2;250;156;55mVOER\u001b[0m minimaal twee acteurs gescheiden door komma's in en druk op \u001b[38;2;250;156;55mEnter\u001b[0m: ");
            }
        }
        Console.Write("\u001b[38;2;250;156;55mVOER\u001b[0m de omschrijving van de film in en druk op \u001b[38;2;250;156;55mEnter\u001b[0m: ");
        string? omschrijving = Console.ReadLine();

        int duur;
        while (true)
        {
            Console.Write("\u001b[38;2;250;156;55mVOER\u001b[0m de duur (in minuten en getallen) en druk op \u001b[38;2;250;156;55mEnter\u001b[0m: ");
            Console.WriteLine("Voorbeeld: 120");
            string inputDuur = Console.ReadLine();
            try
            {
                duur = Convert.ToInt32(inputDuur);
                break;
            }
            catch (FormatException)
            {
                Console.WriteLine("Ongeldige invoer. \u001b[38;2;250;156;55mVOER\u001b[0m een getal in voor de duur (in minuten en getallen) en druk op \u001b[38;2;250;156;55mEnter\u001b[0m: ");
            }
        }

        Film nieuweFilm = new Film
        {
            Titel = naam,
            Genres = genres,
            Acteurs = acteurs,
            Omschrijving = omschrijving,
            Duur = duur
        };

        films.Add(nieuweFilm);
        SaveFilms(films);
        Console.WriteLine("\nFilm succesvol toegevoegd!");
        Thread.Sleep(1000);
    }



    static void BewerkFilm()
    {
        Console.Clear();
        StartScreen.DisplayAsciiArt();
        Console.WriteLine("\u001b[38;2;250;156;55mAdministator systeem\u001b[0m");
        int Index = 0; // Correcte naam: selectedIndex
        string[] options = { "Doorgaan met bewerken van een film", "Terug naar het hoofdmenu" };

        while (true)
        {
            Console.Clear();
            StartScreen.DisplayAsciiArt();
            Console.WriteLine("\u001b[38;2;250;156;55mAdministator systeem\u001b[0m");
            Console.WriteLine("Wat wilt u doen?");
            Console.WriteLine("Gebruik de \u001b[38;2;250;156;55mpijltjestoetsen\u001b[0m om te navigeren en druk op \u001b[38;2;250;156;55mEnter\u001b[0m om te selecteren:");
            for (int i = 0; i < options.Length; i++)
            {
                if (i == Index)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.WriteLine(options[i]);
                Console.ResetColor();
            }

            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.UpArrow)
            {
                Index = Math.Max(0, Index - 1);
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                Index = Math.Min(options.Length - 1, Index + 1);
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                if (Index == 0)
                {
                    break;
                }
                else if (Index == 1)
                {
                    return;
                }
            }
            Console.Clear();
            StartScreen.DisplayAsciiArt();
            Console.WriteLine("\u001b[38;2;250;156;55mAdministator systeem\u001b[0m");
        }

        Console.WriteLine("\nFilm bewerken:");

        if (File.Exists(path))
        {
            try
            {
                string json = File.ReadAllText(path);
                films = JsonConvert.DeserializeObject<List<Film>>(json) ?? new List<Film>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading films from JSON: {ex.Message}");
                films = new List<Film>();
            }
        }

        if (films.Count == 0)
        {
            Console.WriteLine("Er zijn geen films om te bewerken.");
            Thread.Sleep(1000);
            return;
        }

        int selectedIndex = 0;

        while (true)
        {
            Console.Clear();
            StartScreen.DisplayAsciiArt();
            Console.WriteLine("\u001b[38;2;250;156;55mAdministator systeem\u001b[0m");
            Console.WriteLine("Kies de index van de film die u wilt bewerken:");
            for (int i = 0; i < films.Count; i++)
            {
                if (i == selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("=> ");
                }
                else
                {
                    Console.Write("   ");
                }
                Console.WriteLine($"{i + 1}. {films[i].Titel}");
                Console.ResetColor();
            }

            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.UpArrow)
            {
                selectedIndex = Math.Max(0, selectedIndex - 1);
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                selectedIndex = Math.Min(films.Count - 1, selectedIndex + 1);
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                break;
            }
        }

        Film selectedFilm = films[selectedIndex];
        int fieldToEditIndex = 1;

        while (true)
        {
            Console.Clear();
            StartScreen.DisplayAsciiArt();
            Console.WriteLine("\u001b[38;2;250;156;55mAdministator systeem\u001b[0m");
            Console.WriteLine("\nHuidige gegevens:");
            for (int i = 1; i <= 6; i++)
            {
                if (i == fieldToEditIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("=> ");
                }
                else
                {
                    Console.Write("   ");
                }
                Console.WriteLine(GetFieldName(i, selectedFilm));
                Console.ResetColor();
            }

            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.UpArrow)
            {
                fieldToEditIndex = Math.Max(1, fieldToEditIndex - 1);
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                fieldToEditIndex = Math.Min(6, fieldToEditIndex + 1);
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                break;
            }
        }

        switch (fieldToEditIndex)
        {
            case 1:
                Console.Write("\u001b[38;2;250;156;55mVOER\u001b[0m de nieuwe titel van de film in en druk op \u001b[38;2;250;156;55mEnter\u001b[0m: ");
                selectedFilm.Titel = Console.ReadLine();
                break;
            case 2:
                // Bewerken van genres
                List<string> genres;
                while (true)
                {
                    Console.Write("\u001b[38;2;250;156;55mVOER\u001b[0m het nieuwe genre van de film in (gescheiden door komma's) en druk op \u001b[38;2;250;156;55mEnter\u001b[0m: ");
                    Console.WriteLine("Voorbeeld: Drama, Actie");
                    string genreInput = Console.ReadLine();
                    genres = genreInput.Split(',').Select(g => g.Trim()).ToList();

                    // Controleren of er minimaal twee genres zijn ingevoerd
                    if (genres.Count >= 2)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("\u001b[38;2;250;156;55mVOER\u001b[0m minimaal twee genres gescheiden door komma's in en druk op \u001b[38;2;250;156;55mEnter\u001b[0m:");
                    }
                }
                selectedFilm.Genres = genres;
                break;
            case 3:
                // Bewerken van acteurs
                List<string> acteurs;
                while (true)
                {
                    Console.Write("\u001b[38;2;250;156;55mVOER\u001b[0m de nieuwe acteurs van de film in (gescheiden door komma's) en druk op \u001b[38;2;250;156;55mEnter\u001b[0m: ");
                    Console.WriteLine("Voorbeeld: Dwayne Johnson, Sylvester Stallone");
                    string acteurInput = Console.ReadLine();
                    acteurs = acteurInput.Split(',').Select(a => a.Trim()).ToList();

                    // Controleren of er minimaal twee acteurs zijn ingevoerd
                    if (acteurs.Count >= 2)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("\u001b[38;2;250;156;55mVOER\u001b[0m minimaal twee acteurs gescheiden door komma's in en druk op \u001b[38;2;250;156;55mEnter\u001b[0m: ");
                    }
                }
                selectedFilm.Acteurs = acteurs;
                break;
            case 4:
                Console.Write("\u001b[38;2;250;156;55mVOER\u001b[0m de nieuwe omschrijving van de film in en druk op \u001b[38;2;250;156;55mEnter\u001b[0m: ");
                selectedFilm.Omschrijving = Console.ReadLine();
                break;
            case 5:
                int duur;
                while (true)
                {
                    Console.Write("\u001b[38;2;250;156;55mVOER\u001b[0m de nieuwe duur van de film in (in minuten en getallen) en druk op \u001b[38;2;250;156;55mEnter\u001b[0m: ");
                    Console.WriteLine("Voorbeeld: 120");
                    string inputDuur = Console.ReadLine();
                    if (int.TryParse(inputDuur, out duur))
                    {
                        selectedFilm.Duur = duur;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Ongeldige invoer. \u001b[38;2;250;156;55mVOER\u001b[0m een geheel getal in voor de duur (in minuten en getallen) en druk op \u001b[38;2;250;156;55mEnter\u001b[0m: ");
                    }
                }
                break;
        }

        string updatedJsonFilms = JsonConvert.SerializeObject(films, Formatting.Indented);
        File.WriteAllText(path, updatedJsonFilms);

        SaveFilms(films);
        Console.WriteLine("\nFilm succesvol bijgewerkt!");
        Thread.Sleep(1000);
    }
    static void VerwijderFilm()
    {
        Console.Clear();
        StartScreen.DisplayAsciiArt();
        Console.WriteLine("\u001b[38;2;250;156;55mAdministator systeem\u001b[0m");
        int Index = 0; // Correcte naam: selectedIndex
        string[] options = { "Doorgaan met verwijderen van een film", "Terug naar het hoofdmenu" };

        while (true)
        {
            Console.Clear();
            StartScreen.DisplayAsciiArt();
            Console.WriteLine("\u001b[38;2;250;156;55mAdministator systeem\u001b[0m");
            Console.WriteLine("Wat wilt u doen?");
            Console.WriteLine("Gebruik de \u001b[38;2;250;156;55mpijltjestoetsen\u001b[0m om te navigeren en druk op \u001b[38;2;250;156;55mEnter\u001b[0m om te selecteren:");
            for (int i = 0; i < options.Length; i++)
            {
                if (i == Index)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.WriteLine(options[i]);
                Console.ResetColor();
            }

            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.UpArrow)
            {
                Index = Math.Max(0, Index - 1);
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                Index = Math.Min(options.Length - 1, Index + 1);
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                if (Index == 0)
                {
                    break;
                }
                else if (Index == 1)
                {
                    return;
                }
            }
            Console.Clear();
            StartScreen.DisplayAsciiArt();
            Console.WriteLine("\u001b[38;2;250;156;55mAdministator systeem\u001b[0m");
        }

        Console.WriteLine("\nFilm verwijderen:");

        if (File.Exists(path))
        {
            try
            {
                string json = File.ReadAllText(path);
                films = JsonConvert.DeserializeObject<List<Film>>(json) ?? new List<Film>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading films from JSON: {ex.Message}");
                films = new List<Film>();
            }
        }

        if (films.Count == 0)
        {
            Console.WriteLine("Er zijn geen films om te verwijderen.");
            Thread.Sleep(1000);
            return;
        }

        int selectedIndex = 0;

        while (true)
        {
            Console.Clear();
            StartScreen.DisplayAsciiArt();
            Console.WriteLine("\u001b[38;2;250;156;55mAdministator systeem\u001b[0m");
            Console.WriteLine("Gebruik de \u001b[38;2;250;156;55mpijltjestoetsen\u001b[0m om te navigeren en druk op \u001b[38;2;250;156;55mEnter\u001b[0m op de index van de film die u wilt verwijderen:");
            for (int i = 0; i < films.Count; i++)
            {
                if (i == selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("=> ");
                }
                else
                {
                    Console.Write("   ");
                }
                Console.WriteLine($"{i + 1}. {films[i].Titel}");
                Console.ResetColor();
            }

            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.UpArrow)
            {
                selectedIndex = Math.Max(0, selectedIndex - 1);
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                selectedIndex = Math.Min(films.Count - 1, selectedIndex + 1);
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                break;
            }
        }

        int indexToRemove = selectedIndex;

        if (indexToRemove >= 0 && indexToRemove < films.Count)
        {
            films.RemoveAt(indexToRemove);
            string updatedJsonFilms = JsonConvert.SerializeObject(films, Formatting.Indented);
            File.WriteAllText(path, updatedJsonFilms);

            SaveFilms(films);
            Console.WriteLine("\nFilm succesvol verwijderd!");
            Thread.Sleep(1000);
        }
        else
        {
            Console.WriteLine("\nOngeldige index. Film niet verwijderd.");
        }
    }

    static string GetFieldName(int index, Film selectedFilm)
    {
        switch (index)
        {
            case 1:
                return $"1. Titel: {selectedFilm.Titel}";
            case 2:
                return $"2. Genre: {string.Join(", ", selectedFilm.Genres)}";
            case 3:
                return $"3. Acteurs: {string.Join(", ", selectedFilm.Acteurs)}";
            case 4:
                return $"4. Omschrijving: {selectedFilm.Omschrijving}";
            case 5:
                return $"6. Duur: {selectedFilm.Duur} minuten";
            default:
                return "";
        }
    }

    public static bool Login(string email, string password)
    {
        string adminEmail = "admin12@gmail.com";
        string adminPassword = "admin12";

        if (email == adminEmail && password == adminPassword)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    static void SaveFilms(List<Film> filmsToSave)
    {
        string jsonFilms = JsonConvert.SerializeObject(filmsToSave, Formatting.Indented);
        File.WriteAllText(path, jsonFilms);
    }

    static List<Film> LoadFilms()
    {
        List<Film> loadedFilms = new List<Film>();

        if (File.Exists(path))
        {
            try
            {
                string json = File.ReadAllText(path);
                loadedFilms = JsonConvert.DeserializeObject<List<Film>>(json) ?? new List<Film>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading films from JSON: {ex.Message}");
                loadedFilms = new List<Film>();
            }
        }

        return loadedFilms;
    }

    public static Film VoegFilmToe(string naam, string genresInput, string acteursInput, string omschrijving, string inputDuur)
    {
        List<string> genres = genresInput.Split(',').Select(g => g.Trim()).ToList();
        if (genres.Count < 2)
        {
            throw new ArgumentException("Minimaal twee genres zijn vereist.");
        }

        List<string> acteurs = acteursInput.Split(',').Select(a => a.Trim()).ToList();
        if (acteurs.Count < 2)
        {
            throw new ArgumentException("Minimaal twee acteurs zijn vereist.");
        }

        if (!int.TryParse(inputDuur, out int duur))
        {
            throw new ArgumentException("Duur moet een geldig getal zijn.");
        }

        Film nieuweFilm = new Film
        {
            Titel = naam,
            Genres = genres,
            Acteurs = acteurs,
            Omschrijving = omschrijving,
            Duur = duur
        };

        films.Add(nieuweFilm);
        SaveFilms(films);

        return nieuweFilm;
    }
}