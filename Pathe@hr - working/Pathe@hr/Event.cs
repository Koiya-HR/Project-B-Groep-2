public class Event
{
    public string? Locatie { get; set; }
    public int EventID { get; set; }
    public string? MovieID { get; set; }
    public DateTime StartTijd { get; set; }
    public DateTime EindTijd { get; set; }
    public List<EventChair>? Chairs { get; set; }

    private string Color { get; set; } = "\u001b[0m";
    private const string ResetColor = "\u001b[0m";
    private const string HighlightColor = "\u001b[38;2;250;156;55m";

    public void PrintControllInfo()
    {
        Console.WriteLine($"\u001b[38;2;250;156;55m=====================================================================================================================\u001b[0m");
        Console.WriteLine("Gebruik de \u001b[38;2;250;156;55mPIJL OMHOOG\u001b[0m en \u001b[38;2;250;156;55mOMLAAG\u001b[0m om door de lijst te gaan \ndruk \u001b[38;2;250;156;55mENTER\u001b[0m om te selecteren \ndruk \u001b[38;2;250;156;55mESCAPE\u001b[0m om terug te gaan");
        Console.WriteLine($"\u001b[38;2;250;156;55m=====================================================================================================================\u001b[0m");
    }

    public void PrintEvent(bool isHighlighted)
    {
        SetColor(isHighlighted);
        Console.WriteLine($"{Color}====================================================================================================================={ResetColor}");
        Console.WriteLine($"{HighlightColor}Start tijd:\u001b[0m {StartTijd:dd-MM-yyyy HH:mm:ss}");
        Console.WriteLine($"{HighlightColor}Eind tijd:\u001b[0m {EindTijd:dd-MM-yyyy HH:mm:ss}");
        Console.WriteLine($"{Color}====================================================================================================================={ResetColor}");
    }

    private void SetColor(bool isHighlighted)
    {
        if (isHighlighted)
        {
            Color = "\u001b[38;2;58;156;58m";
        }
        else
        {
            Color = ResetColor;
        }
    }
}

public class EventChair
{
    public int Row { get; set; }
    public int Col { get; set; }
    public bool Taken { get; set; }
}