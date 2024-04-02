

public class Stoel
{
    public bool invalide;
    public bool free;
    public bool selected;
    public bool isCurrentChair;
    public int row;
    public int col;
    public int maxChairs;
    public string spaces { get; private set; }
    public string chairName { get; private set; }
    public string chairColor = "\u001b[0m";

    public string coloredChair { get; private set; }
    private const string resetColor = "\u001b[0m";
    public Stoel(int row, int chairNum, bool free, bool invalide, int maxChairs)
    {
        this.row = row;
        this.col = chairNum;
        this.free = free;
        this.invalide = invalide;
        this.maxChairs = maxChairs;
        spaces = setSpaces(maxChairs);
        chairName = makeChairName();
        coloredChair = $"{chairColor}{chairName}{resetColor}";
    }

    private string makeChairName()
    {
        string name = ((row + 1).ToString().Length, (col + 1).ToString().Length) switch
        {
            (2, 2) => $"{row + 1}-{col + 1}",
            (1, 1) => $"0{row + 1}-0{col + 1}",
            (1, 2) => $"0{row + 1}-{col + 1}",
            (2, 1) => $"{row + 1}-0{col + 1}",
            _ => $"{row + 1}-{col + 1}"
        };
        return name;
    }

    private string setSpaces(int chairsInRow)
    {
        string numSpaces;
        if (chairsInRow > 0 && chairsInRow <= 15)
        {
            numSpaces = "    ";
        }
        else if (chairsInRow > 15 && chairsInRow <= 23)
        {
            numSpaces = "  ";
        }
        else if (chairsInRow > 23 && chairsInRow <= 27)
        {
            numSpaces = " ";
        }
        else
        {
            numSpaces = " ";
        }
        return numSpaces;
    }

    public void changeColour(string colorName)
    {
        switch (colorName)
        {
            case "red":
                chairColor = "\u001b[48;2;247;104;96m";
                break;
            case "green":
                chairColor = "\u001b[48;2;105;212;99m";
                break;
            case "yellow":
                chairColor = "\u001b[48;2;230;214;76m";
                break;
            case "gray":
                chairColor = "\u001b[48;2;110;110;110m";
                break;
            case "blue":
                chairColor = "\u001b[48;2;32;85;245m";
                break;
            case "orange":
                chairColor = "\u001b[48;2;247;121;42m";
                break;
            case "darkBlue":
                chairColor = "\u001b[48;2;2;15;191m";
                break;
            case "problem 1":
                chairColor = "\u001b[48;2;0;255;179m";
                break;
            case "problem 2":
                chairColor = "\u001b[48;2;126;15;245m";
                break;
        }
        coloredChair = $"{spaces}{resetColor}{chairColor}{chairName}{resetColor}";
    }

}
