public class Film
{
    public string? Titel { get; set; }
    public List<string>? Genres { get; set; }

    public List<string>? Acteurs { get; set; }

    public string? Omschrijving { get; set; }


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


    public static void PrintControllInfo()
    {
        Console.WriteLine($"\u001b[38;2;250;156;55m=====================================================================================================================\u001b[0m");
        Console.WriteLine("Gebruik de \u001b[38;2;250;156;55mPIJLTJESTOETSEN\u001b[0m om te navigeren \ndruk \u001b[38;2;250;156;55mENTER\u001b[0m om te selecteren \ndruk \u001b[38;2;250;156;55mF\u001b[0m om te zoeken op acteur \ndruk \u001b[38;2;250;156;55mG\u001b[0m om te zoeken op genre \ndruk \u001b[38;2;250;156;55mESCAPE\u001b[0m om terug te gaan");
        Console.WriteLine($"\u001b[38;2;250;156;55m=====================================================================================================================\u001b[0m");
    }

    public static void PrintInfoFilterActor()
    {
        Console.WriteLine($"\u001b[38;2;250;156;55m=====================================================================================================================\u001b[0m");
        Console.WriteLine("Gebruik de \u001b[38;2;250;156;55mPIJLTJESTOETSEN\u001b[0m om te navigeren \ndruk \u001b[38;2;250;156;55mENTER\u001b[0m om te selecteren \ndruk \u001b[38;2;250;156;55mESCAPE\u001b[0m om terug te gaan");
        Console.WriteLine($"\u001b[38;2;250;156;55m=====================================================================================================================\u001b[0m");
    }
    public void PrintMovie()
    {
        setStatusColourMovie();
        Console.WriteLine($"{Color}====================================================================================================================={resetColorMovie}");
        Console.WriteLine("\u001b[38;2;250;156;55mTitel:\u001b[0m " + Titel);
        if (isCurrentMovie)
        {
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
            Console.WriteLine("\u001b[38;2;250;156;55mDuur(in minuten):\u001b[0m " + Duur);
            Console.WriteLine("\u001b[38;2;250;156;55mOmschrijving:\u001b[0m " + Omschrijving);
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }

        Console.WriteLine($"{Color}====================================================================================================================={resetColorMovie}");
    }
}



