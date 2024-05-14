// aangepast
public class Film
{
    public string? Titel { get; set; }
    public List<string>? Genres { get; set; }

    public List<string>? Acteurs { get; set; }

    public string? Omschrijving { get; set; }

    public string? ReleaseDatum { get; set; }

    public int Duur { get; set; }

    public string Color { get; private set; } = "\u001b[0m";
    public const string resetColorMovie = "\u001b[0m";
    public bool isCurrentMovie = false;

    public void setStatusColourMovie()
    {
        if (isCurrentMovie)
        {
            Color = "\u001b[38;2;58;156;58m";
        }
        else
        {
            Color = resetColorMovie;
        }
    }

    public string GetFormattedReleaseDatum()
    {
        if (DateTime.TryParse(ReleaseDatum, out DateTime releaseDate))
        {
            return releaseDate.ToString("d-M-yyyy HH:mm:ss");
        }
        return ReleaseDatum;
    }

    public void PrintControllInfo()
    {
        Console.WriteLine($"\u001b[38;2;250;156;55m=====================================================================================================================\u001b[0m");
        Console.WriteLine("Gebruik de \u001b[38;2;250;156;55mPIJL OMHOOG\u001b[0m en \u001b[38;2;250;156;55mOMLAAG\u001b[0m om door de lijst te gaan \ndruk \u001b[38;2;250;156;55mSPATIE\u001b[0m om te selecteren \ndruk \u001b[38;2;250;156;55mBACKSPACE\u001b[0m om terug te gaan");
        Console.WriteLine($"\u001b[38;2;250;156;55m=====================================================================================================================\u001b[0m");
    }

    public void PrintMovie()
    {
        setStatusColourMovie();
        Console.WriteLine($"{Color}====================================================================================================================={resetColorMovie}");
        Console.WriteLine("\u001b[38;2;250;156;55mTitel:\u001b[0m " + Titel);
    
        if (Genres != null && Genres.Any())
        {
            Console.WriteLine("\u001b[38;2;250;156;55mGenres:\u001b[0m " + string.Join(", ", Genres));
        }
        else
        {
            Console.WriteLine("\u001b[38;2;250;156;55mGenres:\u001b[0m ");
        }

        if (Acteurs != null && Acteurs.Any())
        {
            Console.WriteLine("\u001b[38;2;250;156;55mActeurs:\u001b[0m " + string.Join(", ", Acteurs));
        }
        else
        {
            Console.WriteLine("\u001b[38;2;250;156;55mActeurs:\u001b[0m ");
        }

        Console.WriteLine("\u001b[38;2;250;156;55mDatum:\u001b[0m " + GetFormattedReleaseDatum());
        Console.WriteLine("\u001b[38;2;250;156;55mDuur(in minuten):\u001b[0m " + Duur);
        Console.WriteLine("\u001b[38;2;250;156;55mOmschrijving:\u001b[0m " + Omschrijving);
        Console.WriteLine($"{Color}====================================================================================================================={resetColorMovie}");
    }
}
