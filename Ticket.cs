public class Ticket
{
    public int FilmID { get; set; }
    public int StoelRow { get; set; }
    public int StoelCol { get; set; }
    public int EventID { get; set; }
    public string StoelNaam { get; set; }
    public double Prijs { get; set; }

    public Ticket(int filmID, string name, int row, int col, int eventID, double prijs)
    {
        FilmID = filmID;
        StoelNaam = name;
        StoelRow = row;
        StoelCol = col;
        EventID = eventID;
        Prijs = prijs;
    }
}
