public class Ticket
{
    public string Film { get; set; }
    public int StoelRow { get; set; }
    public int StoelCol { get; set; }
    public int EventID { get; set; }
    public string StoelNaam { get; set; }
    public double Prijs { get; set; }

    public Ticket(string film, string name, int row, int col, int eventID, double prijs)
    {
        Film = film;
        StoelNaam = name;
        StoelRow = row;
        StoelCol = col;
        EventID = eventID;
        Prijs = prijs;
    }
}