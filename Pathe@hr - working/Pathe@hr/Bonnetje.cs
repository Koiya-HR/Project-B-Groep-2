public class Bonnetje
{
    public string Filmnaam { get; set; }
    public List<Drankjes>? Drankjes { get; set; }

    public double Price { get; set; }

    public DateTime starttijd { get; set; }

    public DateTime eindtijd { get; set; }

    public string Locatie { get; set; }

    public List<string>? Stoelen { get; set; }
}

public class Drankjes
{
    public string Name { get; set; }

    public int Count { get; set; }
}