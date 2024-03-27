

public class Stoel
{
    public bool invalide;
    public bool free;
    public int row;
    public int chairNum;

    public string chairName { get; private set; }

    public Stoel(int row, int chairNum, bool free, bool invalide)
    {
        this.row = row;
        this.chairNum = chairNum;
        this.free = free;
        this.invalide = invalide;
        string name = (row, chairNum) switch
        {
            ( > 9 and < 100, > 9 and < 99) => $"     {row}-{chairNum}",
            ( > 0 and < 10, > 0 and < 10) => $"      {row}-{chairNum}",
            _ => $"      {row}-{chairNum}"
        };
        this.chairName = name;
    }

    public void changeColour(string colour, string endColour)
    {
        chairName = $"{colour}{chairName}{endColour}";
    }


}
