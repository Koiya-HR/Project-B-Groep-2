using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class FilmsFilterenOpGenre
{
    public static void FilmsFilteren()
    {
        string jsonFile = File.ReadAllText("ticket_info.json");
        JArray jsonData = JArray.Parse(jsonFile);
        List<string> genres = new() {};
       
       foreach (JObject film in jsonData)
        {
            if (!genres.Contains(film["Genre"].ToString()))
            {
                genres.Add(film["Genre"].ToString());
            }
        }
        
        genres.Add("Opties opslaan");
        foreach (string genre in genres)
        { 
            Console.WriteLine(genre);
        }

        string[] genreOpties = genres.ToArray();
        List<string> geselecteerdeGenres = GenreOpties(genreOpties);
        GefilterdeFilms(geselecteerdeGenres,jsonData);

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
        foreach(JObject film in jsonData)
        {
            if (geselecteerdeGenres.Contains(film["Genre"].ToString()))
            {
                Console.WriteLine($"-{film["Film"].ToString()}");
            }
        }
    }
}
