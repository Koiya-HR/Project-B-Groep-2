

public class Film
{

    public int ID { get; set; }
    public string? Title { get; set; }
    public List<string> Actors { get; private set; } = new();
    public List<string> Genres { get; private set; } = new();

    public string? Description { get; set; }

    public DateTime Date { get; set; }

    private int _length;

    public int Length
    {
        get { return _length; }
        set { _length = value <= 0 ? 0 : value; }
    }

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
        Console.WriteLine("\u001b[38;2;250;156;55mtitel:\u001b[0m " + Title);
        Console.WriteLine("\u001b[38;2;250;156;55mgenres:\u001b[0m " + Genres[0] + ", " + Genres[1]);
        Console.WriteLine("\u001b[38;2;250;156;55macteurs:\u001b[0m " + Actors[0] + ", " + Actors[1]);
        Console.WriteLine("\u001b[38;2;250;156;55mdatum:\u001b[0m " + Date.ToString());
        Console.WriteLine("\u001b[38;2;250;156;55mlengte:\u001b[0m " + Length);
        Console.WriteLine("\u001b[38;2;250;156;55momschrijving:\u001b[0m " + Description);
        Console.WriteLine($"{Color}====================================================================================================================={resetColorMovie}");

    }


}