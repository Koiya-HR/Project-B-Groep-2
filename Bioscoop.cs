using System.Text.Json; // toegevoegd om json te serialiseren en deserialiseren

public class Bioscoop
{
    private List<Film>? Movies { get; set; } // lijstje gemaakt van Films
    private bool isFilteringByActor = false; // bool om te kijken of er gefilterd wordt op acteur
    private bool isFilteringByGenre = false; // bool om te kijken of er gefilterd wordt op genre

    public string Name { get; set; } // bioscoop naam

    public Bioscoop(string name) // constructor maken
    {
        Name = name;
    }

    public void ChooseMovies(Zaal zaal) // methode om films te kiezen
    {
        SetMovies(); // films worden geladen uit json bestand
        if (Movies == null) return; // als er geen films zijn doe dan niets
        bool movieChosen = ShowMovies(); // laat de films zien en kijk of er een film is gekozen
        Console.Clear(); // scherm clearen
        if (movieChosen)
        {
            bool locChosen = Location.chooseLocation();
            if (locChosen)
            {
                zaal.chooseChairs(); // als er een film wordt gekozen dan kom je terecht in de stoelenselectie
            }

        }
    }

    public bool ShowMovies()
    {
        var currentMovieId = 0; // current selected movie id
        var filteredMovies = Movies; // initialize filteredMovies with all movies

        while (true)
        {
            if (filteredMovies == null || filteredMovies.Count == 0)
            {
                Console.WriteLine("No movies available.");
                return false;
            }

            PrintMovies(filteredMovies, currentMovieId);
            var keyInfo = Console.ReadKey();
            switch (keyInfo.Key)
            {
                case ConsoleKey.W or ConsoleKey.UpArrow:
                    if (currentMovieId > 0)
                        currentMovieId--; // move to the previous movie
                    break;
                case ConsoleKey.S or ConsoleKey.DownArrow:
                    if (currentMovieId < filteredMovies.Count - 1)
                        currentMovieId++; // move to the next movie
                    break;
                case ConsoleKey.Enter:
                    Console.WriteLine($"Selected movie is {filteredMovies[currentMovieId].Titel}");
                    Extras.gekozenFilm = filteredMovies[currentMovieId].Titel;

                    // Save the selected movie in the bonnetje.json file
                    SaveSelectedFilm(filteredMovies[currentMovieId].Titel);

                    return true;
                case ConsoleKey.Escape:
                    return false;
            }
        }
    }


    public bool ShowActors() // methode om acteurs te laten zien
    {
        var currentActorId = 0; // huidige geselecteerde acteur id
        var actors = Movies?.SelectMany(f => f.Acteurs).Distinct().ToList() ?? new List<string>(); // Lijst van acteurs

        if (actors.Count == 0)
        {
            Console.WriteLine("Geen acteurs beschikbaar."); // bericht als er geen acteurs zijn
            return false;
        }

        while (true)
        {
            Console.Clear(); // scherm clearen
            PrintActors(actors, currentActorId); // lijst van de acteurs printen
            var keyInfo = Console.ReadKey(); // toetsdruk lezen
            switch (keyInfo.Key)
            {
                case ConsoleKey.W or ConsoleKey.UpArrow:
                    if (currentActorId > 0)
                        currentActorId--; // ga naar de vorige acteur
                    break;
                case ConsoleKey.S or ConsoleKey.DownArrow:
                    if (currentActorId < actors.Count - 1)
                        currentActorId++; // ga naar de volgende acteur
                    break;
                case ConsoleKey.Enter:
                    if (FilterMoviesByActor(actors[currentActorId]))
                        return true; // filter films op geselecteerde acteur
                    break;
                case ConsoleKey.Escape:
                    return false; // uit het programma
            }
        }
    }

    public bool FilterMoviesByActor(string actor) // methode om te filteren op acteur
    {
        var filteredMovies = Movies?.Where(f => f.Acteurs.Contains(actor)).ToList() ?? new List<Film>(); // filter films op de geselecteerde acteur

        if (filteredMovies.Count == 0)
        {
            Console.Clear(); // scherm wordt gecleared
            Console.WriteLine("Geen films gevonden met de geselecteerde acteur."); // bericht als er geen films zijn met geselecteerde acteur
            Console.ReadKey(); // toetsdruk inlezen
            return false;
        }

        var currentMovieId = 0; // huidige geselecteerde film id
        isFilteringByActor = true; // zet de filtermodus op acteur aan

        while (true)
        {
            PrintMovies(filteredMovies, currentMovieId); // print de gefilterde lijst van films
            var keyInfo = Console.ReadKey(true); // toetsdruk inlezen
            switch (keyInfo.Key)
            {
                case ConsoleKey.W or ConsoleKey.UpArrow:
                    if (currentMovieId > 0)
                        currentMovieId--; // ga naar de vorige gefilterde film
                    break;
                case ConsoleKey.S or ConsoleKey.DownArrow:
                    if (currentMovieId < filteredMovies.Count - 1)
                        currentMovieId++; // ga naar de volgende gefilterde film
                    break;
                case ConsoleKey.Enter:
                    Console.WriteLine($"Gekozen film is {filteredMovies[currentMovieId].Titel}");
                    Extras.gekozenFilm = filteredMovies[currentMovieId].Titel; // gekozen film in extra zetten

                    isFilteringByActor = false;
                    return true;
                case ConsoleKey.Escape:
                    isFilteringByActor = false; // zet de filtermodus op acteur uit
                    return false;
            }
        }
    }

    public bool ShowGenres() // methode om genres te laten zien
    {
        var currentGenreId = 0; // huidige geselecteerde genre id
        var genres = Movies?.SelectMany(f => f.Genres).Distinct().ToList() ?? new List<string>(); // lijst van genres

        if (genres.Count == 0)
        {
            Console.WriteLine("Geen genres beschikbaar."); // bericht als er geen genres zijn
            return false;
        }

        while (true)
        {
            Console.Clear(); // scherm clearen
            PrintGenres(genres, currentGenreId); // lijst van genres wordt geprint
            var keyInfo = Console.ReadKey(); // toetsdruk inlezen
            switch (keyInfo.Key)
            {
                case ConsoleKey.W or ConsoleKey.UpArrow:
                    if (currentGenreId > 0)
                        currentGenreId--; // ga naar het vorige genre
                    break;
                case ConsoleKey.S or ConsoleKey.DownArrow:
                    if (currentGenreId < genres.Count - 1)
                        currentGenreId++; // ga naar het volgende genre
                    break;
                case ConsoleKey.Enter:
                    if (FilterMoviesByGenre(genres[currentGenreId]))
                        return true; // filter films op geselecteerde genre
                    break;
                case ConsoleKey.Escape:
                    return false; // uit het programma
            }
        }
    }

    public bool FilterMoviesByGenre(string genre) // methode om te filteren op genre
    {
        var filteredMovies = Movies?.Where(f => f.Genres.Contains(genre)).ToList() ?? new List<Film>(); // filter films op het geselecteerde genre

        if (filteredMovies.Count == 0)
        {
            Console.Clear(); // scherm clearen
            Console.WriteLine("Geen films gevonden met het geselecteerde genre."); // bericht als er geen films zijn met geselecteerde genre
            Console.ReadKey(); // toetsdruk inlezen
            return false;
        }

        var currentMovieId = 0; // huidige geselecteerde film id
        isFilteringByGenre = true; // zet de filtermodus op genre aan

        while (true)
        {
            PrintMovies(filteredMovies, currentMovieId); // print de gefilterde lijst van films
            var keyInfo = Console.ReadKey(true); // toetsdruk inlezen
            switch (keyInfo.Key)
            {
                case ConsoleKey.W or ConsoleKey.UpArrow:
                    if (currentMovieId > 0)
                        currentMovieId--; // ga naar de vorige gefilterde film
                    break;
                case ConsoleKey.S or ConsoleKey.DownArrow:
                    if (currentMovieId < filteredMovies.Count - 1)
                        currentMovieId++; // ga naar de volgende gefilterde film
                    break;
                case ConsoleKey.Enter:
                    Console.WriteLine($"Gekozen film is {filteredMovies[currentMovieId].Titel}");
                    Extras.gekozenFilm = filteredMovies[currentMovieId].Titel; // gekozen film in extra zetten

                    isFilteringByGenre = false;
                    return true;
                case ConsoleKey.Escape:
                    isFilteringByGenre = false; // zet de filtermodus op genre uit
                    return false;
            }
        }
    }

    public void PrintMovies(List<Film> movies, int currentMovieID) // methode gemaakt om films te printen
    {
        Console.Clear(); // scherm clearen
        if (isFilteringByActor || isFilteringByGenre)
        {
            Film.PrintInfoFilterActor(); // print informatie zonder aanwijzingen om te filteren omdat je al filtert
        }
        else
        {
            Film.PrintControllInfo(); // print de normale informatie als in de filmlijst bent
        }

        currentMovieID = Math.Max(0, Math.Min(currentMovieID, movies.Count - 1)); // zorgt ervoor dat het huidige film id binnen de geldige grenzen ligt

        foreach (var movie in movies)
        {
            movie.isCurrentMovie = false; // zet alle films op niet actief
        }

        if (currentMovieID >= 0 && currentMovieID < movies.Count)
        {
            movies[currentMovieID].isCurrentMovie = true; // zet de huidige film op actief
        }

        for (int i = 0; i < movies.Count; i++)
        {
            if (movies[i].isCurrentMovie && i != 0 && i < movies.Count - 1)
            {
                movies[i - 1].PrintMovie(); // print de vorige film
                movies[i].PrintMovie(); // print de huidige film
                movies[i + 1].PrintMovie(); // print de volgende film
            }
            else if (movies[i].isCurrentMovie && i != 0 && i == movies.Count - 1)
            {
                if (i - 2 >= 0) movies[i - 2].PrintMovie(); // print de film twee plaatsen terug als het kan
                movies[i - 1].PrintMovie(); // print de vorige film
                movies[i].PrintMovie(); // print de huidige film
            }
            else if (movies[i].isCurrentMovie && i == 0 && i < movies.Count - 1)
            {
                movies[i].PrintMovie(); // print de huidige film
                movies[i + 1].PrintMovie(); // print de volgende film
                if (i + 2 < movies.Count)
                {
                    movies[i + 2].PrintMovie(); // print de film twee plaatsen vooruit als het kan
                }
            }
            else if (movies[i].isCurrentMovie && i == 0 && i == movies.Count - 1)
            {
                movies[i].PrintMovie(); // print de enige film in de lijst als die er is
            }
        }
    }

    public void PrintActors(List<string> actors, int currentActorId) // methode om acteurs te printen
    {
        Console.Clear(); // scherm clearen

        Console.WriteLine("\nGebruik de \u001b[38;2;250;156;55mpijltjestoetsen\u001b[0m om te navigeren en druk op \u001b[38;2;250;156;55mEnter\u001b[0m om te selecteren:");
        Console.WriteLine("Druk \u001b[38;2;250;156;55mESCAPE\u001b[0m om terug te gaan");
        Console.WriteLine("Selecteer een acteur:\n");

        for (var i = 0; i < actors.Count; i++)
        {
            if (i == currentActorId)
            {
                Console.BackgroundColor = ConsoleColor.Gray; // markeer de huidige acteur
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($"{actors[i]}"); // print de huidige acteur
            }
            else
            {
                Console.WriteLine($"{actors[i]}"); // print de andere acteurs
            }
            Console.ResetColor(); // kleuren reseten
        }
    }

    public void PrintGenres(List<string> genres, int currentGenreId) // methode om genres te printen
    {
        Console.Clear(); // scherm clearen

        Console.WriteLine("\nGebruik de \u001b[38;2;250;156;55mpijltjestoetsen\u001b[0m om te navigeren en druk op \u001b[38;2;250;156;55mEnter\u001b[0m om te selecteren:");
        Console.WriteLine("Druk \u001b[38;2;250;156;55mESCAPE\u001b[0m om terug te gaan");
        Console.WriteLine("Selecteer een genre:\n");

        for (var i = 0; i < genres.Count; i++)
        {
            if (i == currentGenreId)
            {
                Console.BackgroundColor = ConsoleColor.Gray; // markeer het huidige genre
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($"{genres[i]}"); // print het huidige genre
            }
            else
            {
                Console.WriteLine($"{genres[i]}"); // print de andere genres
            }
            Console.ResetColor(); // kleuren reseten
        }
    }

    public void SetMovies() // methode om de films die in het json bestand zitten te laden
    {
        var filePath = "films.json"; // naam van json file

        if (File.Exists(filePath))
        {
            Movies = JsonSerializer.Deserialize<List<Film>>(File.ReadAllText(filePath)); // films uitladen uit json bestand
        }
        else
        {
            Console.WriteLine("wrong filepath, films.json not found"); // bericht als de json file niet gevonden is
        }
    }

    private void SaveSelectedFilm(string filmnaam)
    {
        var bonnetjePath = "bonnetje.json"; // pad naar bonnetje.json

        if (File.Exists(bonnetjePath))
        {
            // Laad het bonnetje uit het bestand
            var bonnetjeJson = File.ReadAllText(bonnetjePath);
            var bonnetje = JsonSerializer.Deserialize<Dictionary<string, object>>(bonnetjeJson);

            // Update de filmnaam
            bonnetje["Filmnaam"] = filmnaam;

            // Sla het bijgewerkte bonnetje terug op
            var updatedBonnetjeJson = JsonSerializer.Serialize(bonnetje, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(bonnetjePath, updatedBonnetjeJson);
        }
        else
        {
            Console.WriteLine("Bonnetje niet gevonden.");
        }
    }
}


// using Newtonsoft.Json;
// using Newtonsoft.Json.Linq;
// using System;
// using System.Collections.Generic;
// using System.IO;

// public class Bioscoop
// {
//     public string Location;
//     public List<Film>? allMovies { get; private set; }

//     private bool movieChosen = false;

//     public Bioscoop(string location)
//     {
//         Location = location;
//         MakeMovies();
//     }

//     public void ChooseMovies(Zaal zaal)
//     {
//         FilmsFilterenOpGenre.Filteren();
//         int returnCode = ShowMovies();
//         if (returnCode != -1 && returnCode != -2) // -1 is geen film gekozen en -2 is geen films aanwezig
//         {
//             zaal.chooseChairs();
//         }

//     }


//     public int ShowMovies()
//     {
//         MakeMovies();
//         if (allMovies != null)
//         {
//             int currentMovieID = 0;
//             if (movieChosen)
//             {
//                 Console.Clear();
//                 movieChosen = false;

//                 foreach (Film movie in allMovies)
//                 {
//                     movie.isCurrentMovie = false;
//                 }
//                 allMovies[0].isCurrentMovie = true;
//             }
//             else
//             {
//                 Console.Clear();
//                 //currentMovieID = 0;
//                 allMovies[currentMovieID].isCurrentMovie = true;

//             }
//             Film.PrintControllInfo();
//             allMovies[currentMovieID].PrintMovie();
//             if (currentMovieID + 2 < allMovies.Count)
//             {
//                 allMovies[currentMovieID + 1].PrintMovie();
//                 allMovies[currentMovieID + 2].PrintMovie();
//             }
//             else if (currentMovieID + 1 < allMovies.Count)
//             {
//                 allMovies[currentMovieID + 1].PrintMovie();
//             }
//             while (true)
//             {
//                 ConsoleKeyInfo keyInfo = Console.ReadKey();
//                 Console.Clear();
//                 switch (keyInfo.Key)
//                 {
//                     case ConsoleKey.W or ConsoleKey.UpArrow:
//                         if (currentMovieID > 0)
//                         {
//                             allMovies[currentMovieID].isCurrentMovie = false;
//                             currentMovieID--;
//                             allMovies[currentMovieID].isCurrentMovie = true;
//                         }
//                         break;
//                     case ConsoleKey.S or ConsoleKey.DownArrow:
//                         if (currentMovieID < allMovies.Count - 1)
//                         {
//                             allMovies[currentMovieID].isCurrentMovie = false;
//                             currentMovieID++;
//                             allMovies[currentMovieID].isCurrentMovie = true;
//                         }
//                         break;
//                     case ConsoleKey.Enter:
//                         Console.Clear();
//                         movieChosen = true;
//                         Console.WriteLine($"Gekozen film is {allMovies[currentMovieID].Titel}");
//                         Extras.gekozenFilm = allMovies[currentMovieID].Titel;
//                         Extras.EventID = 1;
//                         return currentMovieID; // Index van de gekozen film
//                     case ConsoleKey.Escape:
//                         Console.Clear();
//                         movieChosen = false;
//                         return -1; //no movie chosen
//                 }
//                 Film.PrintControllInfo();
//                 for (int i = 0; i < allMovies.Count; i++)
//                 {
//                     if (allMovies[i].isCurrentMovie && i != 0 && i < allMovies.Count - 1)
//                     {
//                         allMovies[i - 1].PrintMovie();
//                         allMovies[i].PrintMovie();
//                         allMovies[i + 1].PrintMovie();
//                     }
//                     else if (allMovies[i].isCurrentMovie && i != 0 && i == allMovies.Count - 1)
//                     {
//                         allMovies[i - 2].PrintMovie();
//                         allMovies[i - 1].PrintMovie();
//                         allMovies[i].PrintMovie();
//                     }
//                     else if (allMovies[i].isCurrentMovie && i == 0 && i < allMovies.Count - 1)
//                     {
//                         allMovies[i].PrintMovie();
//                         allMovies[i + 1].PrintMovie();
//                         allMovies[i + 2].PrintMovie();
//                     }
//                 }
//             }
//         }
//         else
//         {
//             Console.WriteLine("Geen films beschikbaar.");
//             return -2;// no movies 
//         }

//     }


//     public void MakeMovies()
//     {
//         string filePath = "films.json";
//         if (File.Exists(filePath))
//         {
//             string jsonContent = File.ReadAllText(filePath);
//             var settings = new JsonSerializerSettings
//             {
//                 TypeNameHandling = TypeNameHandling.Auto
//             };
//             allMovies = JsonConvert.DeserializeObject<List<Film>>(jsonContent, settings)!;
//         }
//         else
//         {
//             Console.WriteLine("wrong filepath, films.json not found");
//         }



//     }
// }

// using Newtonsoft.Json;
// using Newtonsoft.Json.Linq;
// using System;
// using System.Collections.Generic;
// using System.IO;

// public class Bioscoop
// {
//     public string Location;
//     public List<Film>? allMovies { get; private set; }

//     private bool movieChosen = false;

//     public Bioscoop(string location)
//     {
//         Location = location;
//         MakeMovies();
//     }

//     public void ChooseMovies(Zaal zaal)
//     {
//         FilmsFilterenOpGenre.Filteren();
//         int returnCode = ShowMovies();
//         if (returnCode != -1 && returnCode != -2) // -1 is geen film gekozen en -2 is geen films aanwezig
//         {
//             zaal.chooseChairs();
//         }

//     }


//     public int ShowMovies()
//     {
//         MakeMovies();
//         if (allMovies != null)
//         {
//             int currentMovieID = 0;
//             if (movieChosen)
//             {
//                 Console.Clear();
//                 movieChosen = false;

//                 foreach (Film movie in allMovies)
//                 {
//                     movie.isCurrentMovie = false;
//                 }
//                 allMovies[0].isCurrentMovie = true;
//             }
//             else
//             {
//                 Console.Clear();
//                 //currentMovieID = 0;
//                 allMovies[currentMovieID].isCurrentMovie = true;

//             }
//             Film.PrintControllInfo();
//             allMovies[currentMovieID].PrintMovie();
//             if (currentMovieID + 2 < allMovies.Count)
//             {
//                 allMovies[currentMovieID + 1].PrintMovie();
//                 allMovies[currentMovieID + 2].PrintMovie();
//             }
//             else if (currentMovieID + 1 < allMovies.Count)
//             {
//                 allMovies[currentMovieID + 1].PrintMovie();
//             }
//             while (true)
//             {
//                 ConsoleKeyInfo keyInfo = Console.ReadKey();
//                 Console.Clear();
//                 switch (keyInfo.Key)
//                 {
//                     case ConsoleKey.W or ConsoleKey.UpArrow:
//                         if (currentMovieID > 0)
//                         {
//                             allMovies[currentMovieID].isCurrentMovie = false;
//                             currentMovieID--;
//                             allMovies[currentMovieID].isCurrentMovie = true;
//                         }
//                         break;
//                     case ConsoleKey.S or ConsoleKey.DownArrow:
//                         if (currentMovieID < allMovies.Count - 1)
//                         {
//                             allMovies[currentMovieID].isCurrentMovie = false;
//                             currentMovieID++;
//                             allMovies[currentMovieID].isCurrentMovie = true;
//                         }
//                         break;
//                     case ConsoleKey.Enter:
//                         Console.Clear();
//                         movieChosen = true;
//                         Console.WriteLine($"Gekozen film is {allMovies[currentMovieID].Titel}");
//                         Extras.gekozenFilm = allMovies[currentMovieID].Titel;
//                         Extras.EventID = 1;
//                         return currentMovieID; // Index van de gekozen film
//                     case ConsoleKey.Escape:
//                         Console.Clear();
//                         movieChosen = false;
//                         return -1; //no movie chosen
//                 }
//                 Film.PrintControllInfo();
//                 for (int i = 0; i < allMovies.Count; i++)
//                 {
//                     if (allMovies[i].isCurrentMovie && i != 0 && i < allMovies.Count - 1)
//                     {
//                         allMovies[i - 1].PrintMovie();
//                         allMovies[i].PrintMovie();
//                         allMovies[i + 1].PrintMovie();
//                     }
//                     else if (allMovies[i].isCurrentMovie && i != 0 && i == allMovies.Count - 1)
//                     {
//                         allMovies[i - 2].PrintMovie();
//                         allMovies[i - 1].PrintMovie();
//                         allMovies[i].PrintMovie();
//                     }
//                     else if (allMovies[i].isCurrentMovie && i == 0 && i < allMovies.Count - 1)
//                     {
//                         allMovies[i].PrintMovie();
//                         allMovies[i + 1].PrintMovie();
//                         allMovies[i + 2].PrintMovie();
//                     }
//                 }
//             }
//         }
//         else
//         {
//             Console.WriteLine("Geen films beschikbaar.");
//             return -2;// no movies 
//         }

//     }


//     public void MakeMovies()
//     {
//         string filePath = "films.json";
//         if (File.Exists(filePath))
//         {
//             string jsonContent = File.ReadAllText(filePath);
//             var settings = new JsonSerializerSettings
//             {
//                 TypeNameHandling = TypeNameHandling.Auto
//             };
//             allMovies = JsonConvert.DeserializeObject<List<Film>>(jsonContent, settings)!;
//         }
//         else
//         {
//             Console.WriteLine("wrong filepath, films.json not found");
//         }



//     }
// }

// using Newtonsoft.Json;
// using Newtonsoft.Json.Linq;
// using System;
// using System.Collections.Generic;
// using System.IO;

// public class Bioscoop
// {
//     public string Location;
//     public List<Film>? allMovies { get; private set; }

//     private bool movieChosen = false;

//     public Bioscoop(string location)
//     {
//         Location = location;
//         MakeMovies();
//     }

//     public void ChooseMovies(Zaal zaal)
//     {
//         int returnCode = ShowMovies();
//         if (returnCode != -1 && returnCode != -2) // -1 is geen film gekozen en -2 is geen films aanwezig
//         {
//             zaal.chooseChairs();
//         }
//     }

//     public int ShowMovies()
//     {
//         MakeMovies();
//         if (allMovies != null)
//         {
//             int currentMovieID = 0;
//             if (movieChosen)
//             {
//                 Console.Clear();
//                 movieChosen = false;

//                 foreach (Film movie in allMovies)
//                 {
//                     movie.isCurrentMovie = false;
//                 }
//                 allMovies[0].isCurrentMovie = true;
//             }
//             else
//             {
//                 Console.Clear();
//                 allMovies[currentMovieID].isCurrentMovie = true;
//             }
//             allMovies[currentMovieID].PrintControllInfo();
//             allMovies[currentMovieID].PrintMovie();
//             if (currentMovieID + 2 < allMovies.Count)
//             {
//                 allMovies[currentMovieID + 1].PrintMovie();
//                 allMovies[currentMovieID + 2].PrintMovie();
//             }
//             else if (currentMovieID + 1 < allMovies.Count)
//             {
//                 allMovies[currentMovieID + 1].PrintMovie();
//             }
//             while (true)
//             {
//                 ConsoleKeyInfo keyInfo = Console.ReadKey();
//                 Console.Clear();
//                 switch (keyInfo.Key)
//                 {
//                     case ConsoleKey.W or ConsoleKey.UpArrow:
//                         if (currentMovieID > 0)
//                         {
//                             allMovies[currentMovieID].isCurrentMovie = false;
//                             currentMovieID--;
//                             allMovies[currentMovieID].isCurrentMovie = true;
//                         }
//                         break;
//                     case ConsoleKey.S or ConsoleKey.DownArrow:
//                         if (currentMovieID < allMovies.Count - 1)
//                         {
//                             allMovies[currentMovieID].isCurrentMovie = false;
//                             currentMovieID++;
//                             allMovies[currentMovieID].isCurrentMovie = true;
//                         }
//                         break;
//                     case ConsoleKey.Spacebar:
//                         Console.Clear();
//                         movieChosen = true;
//                         Console.WriteLine($"Gekozen film is {allMovies[currentMovieID].Titel}");
//                         UpdateBonnetjeJson(allMovies[currentMovieID].Titel); // Update bonnetje.json with the chosen movie name
//                         return currentMovieID; // Index van de gekozen film
//                     case ConsoleKey.Backspace:
//                         Console.Clear();
//                         movieChosen = false;
//                         return -1; //no movie chosen
//                 }
//                 allMovies[currentMovieID].PrintControllInfo();
//                 for (int i = 0; i < allMovies.Count; i++)
//                 {
//                     if (allMovies[i].isCurrentMovie && i != 0 && i < allMovies.Count - 1)
//                     {
//                         allMovies[i - 1].PrintMovie();
//                         allMovies[i].PrintMovie();
//                         allMovies[i + 1].PrintMovie();
//                     }
//                     else if (allMovies[i].isCurrentMovie && i != 0 && i == allMovies.Count - 1)
//                     {
//                         allMovies[i - 2].PrintMovie();
//                         allMovies[i - 1].PrintMovie();
//                         allMovies[i].PrintMovie();
//                     }
//                     else if (allMovies[i].isCurrentMovie && i == 0 && i < allMovies.Count - 1)
//                     {
//                         allMovies[i].PrintMovie();
//                         allMovies[i + 1].PrintMovie();
//                         allMovies[i + 2].PrintMovie();
//                     }
//                 }
//             }
//         }
//         else
//         {
//             Console.WriteLine("Geen films beschikbaar.");
//             return -2; // no movies 
//         }
//     }

//     public void MakeMovies()
//     {
//         string filePath = "films.json";
//         if (File.Exists(filePath))
//         {
//             string jsonContent = File.ReadAllText(filePath);
//             var settings = new JsonSerializerSettings
//             {
//                 TypeNameHandling = TypeNameHandling.Auto
//             };
//             allMovies = JsonConvert.DeserializeObject<List<Film>>(jsonContent, settings)!;
//         }
//         else
//         {
//             Console.WriteLine("wrong filepath, films.json not found");
//         }
//     }

//     private void UpdateBonnetjeJson(string chosenMovieName)
//     {
//         string bonnetjeFilePath = "bonnetje.json";
//         if (File.Exists(bonnetjeFilePath))
//         {
//             string jsonContent = File.ReadAllText(bonnetjeFilePath);
//             var jsonObject = JObject.Parse(jsonContent);

//             // Update the JSON data with the chosen movie name
//             jsonObject["Filmnaam"] = chosenMovieName;

//             // Save the updated JSON data back to the file
//             File.WriteAllText(bonnetjeFilePath, jsonObject.ToString());
//         }
//         else
//         {
//             Console.WriteLine("wrong filepath, bonnetje.json not found");
//         }
//     }
// }