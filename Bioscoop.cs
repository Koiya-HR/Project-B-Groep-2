using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
// aangepast
public class Bioscoop
{
    public string Location;
    public List<Film>? allMovies { get; private set; }

    private bool movieChosen = false;

    public Bioscoop(string location)
    {
        Location = location;
        MakeMovies();
    }

    public void ChooseMovies(Zaal zaal)
    {
        int returnCode = ShowMovies();
        if (returnCode != -1 && returnCode != -2) // -1 is geen film gekozen en -2 is geen films aanwezig
        {
            zaal.chooseChairs();
        }

    }


    public int ShowMovies()
    {
        MakeMovies();

        if (allMovies != null)
        {
            int currentMovieID = 0;
            if (movieChosen)
            {
                Console.Clear();
                movieChosen = false;

                foreach (Film movie in allMovies)
                {
                    movie.isCurrentMovie = false;
                }
                allMovies[0].isCurrentMovie = true;
            }
            else
            {
                Console.Clear();
                allMovies[currentMovieID].isCurrentMovie = true;
            }

            allMovies[currentMovieID].PrintControllInfo();
            allMovies[currentMovieID].PrintMovie();

            if (currentMovieID + 1 < allMovies.Count)
            {
                allMovies[currentMovieID + 1].PrintMovie();
            }

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                Console.Clear();
                switch (keyInfo.Key)
                {
                    case ConsoleKey.W or ConsoleKey.UpArrow:
                        if (currentMovieID > 0)
                        {
                            allMovies[currentMovieID].isCurrentMovie = false;
                            currentMovieID--;
                            allMovies[currentMovieID].isCurrentMovie = true;
                        }
                        break;
                    case ConsoleKey.S or ConsoleKey.DownArrow:
                        if (currentMovieID < allMovies.Count - 1)
                        {
                            allMovies[currentMovieID].isCurrentMovie = false;
                            currentMovieID++;
                            allMovies[currentMovieID].isCurrentMovie = true;
                        }
                        break;
                    case ConsoleKey.Spacebar:
                        Console.Clear();
                        movieChosen = true;
                        Console.WriteLine($"Gekozen film is {allMovies[currentMovieID].Titel}");
                        return currentMovieID; // Index van de gekozen film
                    case ConsoleKey.Backspace:
                        Console.Clear();
                        movieChosen = false;
                        return -1; // Geen film gekozen
                }

                allMovies[currentMovieID].PrintControllInfo();

                if (currentMovieID < allMovies.Count)
                {
                    allMovies[currentMovieID].PrintMovie();

                    if (currentMovieID + 1 < allMovies.Count)
                    {
                        allMovies[currentMovieID + 1].PrintMovie();
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("Geen films beschikbaar.");
            return -2; // Geen films beschikbaar
        }
    }

    public void MakeMovies()
    {
        string filePath = "movies.json";
        if (File.Exists(filePath))
        {
            Console.WriteLine("right filepath");
            string jsonContent = File.ReadAllText(filePath);
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
            allMovies = JsonConvert.DeserializeObject<List<Film>>(jsonContent, settings)!;
        }
        else
        {
            Console.WriteLine("wrong filepath");
        }
    }
}
