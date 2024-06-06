using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class FilmsFilterenOpGenre
{
    public static void Filteren()
    {
        string jsonFile = File.ReadAllText("films.json");
        JArray jsonData = JArray.Parse(jsonFile);
        List<string> genres = new() { };

        foreach (JObject film in jsonData)
        {
            JArray filmGenres = (JArray)film["Genres"];
            foreach (string genre in filmGenres)
            {
                if (!genres.Contains(genre))
                {
                    genres.Add(genre);
                }
            }
        }

        genres.Add("Opties opslaan");
        foreach (string genre in genres)
        {
            Console.WriteLine(genre);
        }

        string[] genreOpties = genres.ToArray();
        List<string> geselecteerdeGenres = GenreOpties(genreOpties);
        GefilterdeFilms(geselecteerdeGenres, jsonData);

    }

    public static List<string> GenreOpties(string[] opties)
    {
        List<string> geselecteerdeOpties = new List<string>();
        int geselecteerdeIndex = 0;

        while (true)
        {
            Console.Clear();

            for (int i = 0; i < opties.Length; i++)
            {
                if (geselecteerdeOpties.Contains(opties[i]))
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else if (i == geselecteerdeIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }

                Console.WriteLine(opties[i]);
                Console.ResetColor();
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    geselecteerdeIndex = (geselecteerdeIndex - 1 + opties.Length) % opties.Length;
                    break;
                case ConsoleKey.DownArrow:
                    geselecteerdeIndex = (geselecteerdeIndex + 1) % opties.Length;
                    break;
                case ConsoleKey.Enter:
                    if (opties[geselecteerdeIndex] == "Opties opslaan")
                    {
                        return geselecteerdeOpties;
                    }
                    else
                    {
                        string selectedOption = opties[geselecteerdeIndex];
                        if (!geselecteerdeOpties.Contains(selectedOption))
                            geselecteerdeOpties.Add(selectedOption);
                        else
                            geselecteerdeOpties.Remove(selectedOption);
                    }
                    break;
            }
        }
    }
    public static void GefilterdeFilms(List<string> geselecteerdeGenres, JArray jsonData)
    {
        Console.Clear();
        Console.WriteLine("Geselecteerde films:");
        foreach (JObject film in jsonData)
        {
            if (geselecteerdeGenres.Contains(film["Genres"].ToString()))
            {
                Console.WriteLine($"-{film["Titel"]}");
            }
        }
    }
}