


public class Zaal
{
    public int numRows;
    public int chairsInRow;
    public List<Stoel> chairs { get; } = new();
    Stoel[,] stoelArray;


    public Zaal(int numRows, int chairsInRow)
    {
        this.numRows = numRows;
        this.chairsInRow = chairsInRow;
        buildChairs();
        stoelArray = CreateStoelArray(numRows, chairsInRow);
    }

    public void chooseChairs()
    {
        bool chairsChosen = false;
        while (!chairsChosen)
        {
            PrintStoelArray();

        }
    }


    private Stoel[,] CreateStoelArray(int rows, int columns)
    {
        Stoel[,] array = new Stoel[rows, columns];

        int index = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                // Assign each chair from the list to the 2D array
                array[i, j] = chairs[index++];
            }
        }

        return array;
    }

    public void PrintStoelArray()
    {
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < chairsInRow; j++)
            {
                Console.Write(stoelArray[i, j].chairName);
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }


    public void buildChairs()
    {
        for (int j = 1; j < numRows + 1; j++)
        {
            for (int i = 1; i < chairsInRow + 1; i++)
            {
                if (i == 1 || i == chairsInRow)
                {
                    chairs.Add(new Stoel(j, i, true, true));
                }
                else
                {
                    chairs.Add(new Stoel(j, i, true, false));
                }
            }
        }
        Console.WriteLine("chairs built");
    }

    public void setStatusColour(List<Stoel> stoelen)
    {
        foreach (Stoel stoel in stoelen)
        {
            if (stoel.free)
            {
                stoel.changeColour("/Esc[100m", "Esc[0m");
            }
        }
    }
}
