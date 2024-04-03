

public class Zaal
{
    public int numRows;
    public int chairsInRow;
    public List<Stoel> chairs { get; } = new();
    Stoel[,] stoelArray;
    List<string> messages = new();

    public int numInvalideTickets;
    public int numNormaleTickets;
    public const int maxChairsPerOrder = 12;


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
        chooseTickets();
        printInfo();
        PrintStoelArray();
        while (!chairsChosen)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            Console.Clear();
            (int oldRow, int oldCol) = findSelectedChair();
            int newRow = oldRow;
            int newCol = oldCol;

            switch (keyInfo.Key)
            {
                case ConsoleKey.Enter:
                    return;
                case ConsoleKey.W:
                    newRow = (oldRow == 0) ? oldRow : oldRow - 1;
                    break;
                case ConsoleKey.S:
                    newRow = (oldRow >= stoelArray.GetLength(0) - 1) ? oldRow : oldRow + 1;
                    break;
                case ConsoleKey.A:
                    newCol = (oldCol == 0) ? oldCol : oldCol - 1;
                    break;
                case ConsoleKey.D:
                    newCol = (oldCol >= stoelArray.GetLength(1) - 1) ? oldCol : oldCol + 1;
                    break;
                case ConsoleKey.Spacebar:
                    // bool select = canBeSelectedCheck(oldRow, oldCol);
                    // if (select)
                    // {
                    //     stoelArray[oldRow, oldCol].selected = true;
                    //     stoelArray[oldRow, oldCol].free = false;
                    // }
                    canBeSelectedCheck(newRow, newCol);
                    if (!stoelArray[oldRow, oldCol].selected && stoelArray[oldRow, oldCol].free)
                    {
                        stoelArray[oldRow, oldCol].selected = true;
                        //stoelArray[oldRow, oldCol].free = false;
                    }
                    else if (stoelArray[oldRow, oldCol].selected)
                    {
                        stoelArray[oldRow, oldCol].selected = false;
                        //dstoelArray[oldRow, oldCol].free = true;
                    }
                    break;
                case ConsoleKey.F:
                    if (stoelArray[oldRow, oldCol].free && !stoelArray[oldRow, oldCol].selected)
                        stoelArray[oldRow, oldCol].free = false;
                    else if (!stoelArray[oldRow, oldCol].free && !stoelArray[oldRow, oldCol].selected)
                        stoelArray[oldRow, oldCol].free = true;
                    break;

            }

            stoelArray[oldRow, oldCol].isCurrentChair = false;
            stoelArray[newRow, newCol].isCurrentChair = true;



            printInfo();
            PrintStoelArray();
            printNotifications();
        }
    }

    private void chooseTickets()
    {
        while (true)
        {
            Console.WriteLine("num invalide stoelen");
            if (int.TryParse(Console.ReadLine(), out numInvalideTickets) && numInvalideTickets >= 0)
            {

                Console.WriteLine("num normale stoelen");
                if (int.TryParse(Console.ReadLine(), out numNormaleTickets) && numNormaleTickets >= 0)
                {
                    return;
                }
            }

            Console.WriteLine("not valid input, type in a number");


        }
    }

    private void canBeSelectedCheck(int rowToCheck, int colToCheck)
    {
        bool isFirstChair = true;
        int firstRow = 0;
        int firstCol = 0;
        for (int i = 0; i < stoelArray.GetLength(0); i++)
        {
            for (int j = 0; j < stoelArray.GetLength(1); j++)
            {
                if (stoelArray[i, j].selected)
                {
                    isFirstChair = false;
                    firstRow = i;
                    firstCol = j;
                }
            }
        }
        if (!isFirstChair)
        {
            Console.WriteLine($"is first chair: {firstRow}-{firstCol}");
        }
    }
    // {
    //     if (stoelArray[rowToCheck, colToCheck].selected)
    //     {
    //         if (stoelArray[rowToCheck, colToCheck].invalide)
    //         {
    //             if (colToCheck == stoelArray.GetLength(1))
    //             {
    //                 if (stoelArray[rowToCheck, colToCheck - 1].free)
    //                 {
    //                     stoelArray[rowToCheck, colToCheck].selected = false;
    //                     stoelArray[rowToCheck, colToCheck].free = true;
    //                     return false;
    //                 }
    //             }
    //             else if (stoelArray[rowToCheck, colToCheck + 1].free)
    //             {
    //                 stoelArray[rowToCheck, colToCheck].selected = false;
    //                 stoelArray[rowToCheck, colToCheck].free = true;
    //                 return false;
    //             }
    //         }
    //         if (stoelArray[rowToCheck, colToCheck - 1].free || stoelArray[rowToCheck, colToCheck + 1].free)
    //         {
    //             stoelArray[rowToCheck, colToCheck].selected = false;
    //             stoelArray[rowToCheck, colToCheck].free = true;
    //             return false;
    //         }
    //         return false;
    //     }
    //     bool isFirstChair = true;
    //     int selectingRow = 9999;
    //     int optionLeft = 9999;
    //     int optionRight = 9999;
    //     foreach (Stoel rij in stoelArray)
    //     {
    //         foreach (Stoel stoel in stoelArray)
    //         {
    //             if (stoel.selected && isFirstChair)
    //             {
    //                 selectingRow = stoel.row;
    //                 isFirstChair = false;
    //                 if (stoel.col == 0)
    //                 {
    //                     optionLeft = 9999;
    //                 }
    //                 else if (stoel.col == stoel.maxChairs)
    //                 {
    //                     optionRight = 9999;
    //                 }
    //                 else
    //                 {
    //                     optionLeft = stoel.col - 1;
    //                 }
    //             }
    //             else if (stoel.selected && !isFirstChair)
    //             {
    //                 if (stoel.col < stoel.maxChairs)
    //                 {
    //                     if (stoelArray[stoel.row, stoel.col + 1].free == true)
    //                     {
    //                         optionRight = stoel.col;
    //                         break;
    //                     }
    //                 }
    //             }
    //         }
    //     }

    //     if (stoelArray[rowToCheck, colToCheck].free)
    //     {
    //         if (isFirstChair)
    //         {
    //             if (stoelArray[rowToCheck, colToCheck].invalide)
    //             {
    //                 if (numInvalideTickets > 0)
    //                 {
    //                     if (colToCheck == stoelArray.GetLength(1))
    //                     {
    //                         if (stoelArray[rowToCheck, colToCheck - 1].free &&
    //                             stoelArray[rowToCheck, colToCheck - 2].free ||
    //                             !stoelArray[rowToCheck, colToCheck - 1].free)
    //                         {
    //                             return true;
    //                         }
    //                         return false;
    //                     }
    //                     else
    //                     {
    //                         if (stoelArray[rowToCheck, colToCheck + 1].free &&
    //                             stoelArray[rowToCheck, colToCheck + 2].free ||
    //                             !stoelArray[rowToCheck, colToCheck + 1].free)
    //                         {
    //                             return true;
    //                         }
    //                         return false;
    //                     }
    //                 }
    //                 return false;
    //             }
    //             else
    //             {
    //                 Console.WriteLine($"row {rowToCheck}, col {colToCheck}");
    //                 if (stoelArray[rowToCheck, colToCheck + 1].free &&
    //                     stoelArray[rowToCheck, colToCheck + 2].free ||
    //                     !stoelArray[rowToCheck, colToCheck + 1].free)
    //                 {
    //                     if (stoelArray[rowToCheck, colToCheck - 1].free &&
    //                         stoelArray[rowToCheck, colToCheck - 2].free ||
    //                         !stoelArray[rowToCheck, colToCheck - 1].free)
    //                     {
    //                         return true;
    //                     }
    //                     return false;
    //                 }
    //                 return false;
    //             }
    //         }
    //         else if (!isFirstChair)
    //         {
    //             if (selectingRow != 9999)
    //             {
    //                 if (rowToCheck == selectingRow)
    //                 {
    //                     if (colToCheck == optionLeft)
    //                     {
    //                         if (stoelArray[rowToCheck, colToCheck].invalide)
    //                         {
    //                             if (numInvalideTickets > 0)
    //                             {
    //                                 return true;
    //                             }
    //                             return false;
    //                         }
    //                     }
    //                     else if (colToCheck == optionRight)
    //                     {
    //                         if (stoelArray[rowToCheck, colToCheck].invalide)
    //                         {
    //                             if (numInvalideTickets > 0)
    //                             {
    //                                 return true;
    //                             }
    //                             return false;
    //                         }
    //                     }
    //                 }
    //             }
    //             return false;
    //         }
    //         return false;
    //     }



    //     return false;

    // }


    private void printInfo()
    {
        Console.WriteLine("Use W A S D to move, Press SPACEBAR to select and deselect and press ENTER to continue");
        Console.WriteLine("\u001b[48;2;230;214;76m   \u001b[0m : Free \n" +
                            "\u001b[48;2;110;110;110m   \u001b[0m : Taken \n" +
                            "\u001b[48;2;32;85;245m   \u001b[0m : Selected \n" +
                            "\u001b[48;2;105;212;99m   \u001b[0m : Current location");
        Console.WriteLine();
        Console.WriteLine("Debug keys: \nF : Set chair to taken");
        Console.WriteLine();
    }

    private void printNotifications()
    {
        Console.WriteLine("Notifications:");
        foreach (string msg in messages)
        {
            Console.WriteLine($"\u001b[48;2;242;68;77m{msg}\u001b[0m \n");
            messages.Remove(msg);
        }
    }

    private (int, int) findSelectedChair()
    {

        int rows = stoelArray.GetLength(0);
        int columns = stoelArray.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (stoelArray[i, j].isCurrentChair)
                {
                    return (i, j);
                }
            }
        }
        return (0, 0);
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

    public void buildChairs()
    {
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < chairsInRow; j++)
            {
                if (j == 1 || j == chairsInRow)
                {
                    chairs.Add(new Stoel(i, j, true, true, chairsInRow));
                }
                else
                {
                    chairs.Add(new Stoel(i, j, true, false, chairsInRow));
                }
            }
        }
        Console.WriteLine("chairs built");
    }

    public void PrintStoelArray()
    {
        setStatusColour(stoelArray);
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < chairsInRow; j++)
            {
                Console.Write(stoelArray[i, j].coloredChair);
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }




    public void setStatusColour(Stoel[,] stoelen)
    {
        foreach (Stoel stoel in stoelen)
        {
            string result = (stoel.free, stoel.isCurrentChair, stoel.selected) switch
            {
                (true, true, false) => "green",
                (true, false, false) => "yellow",
                (false, false, false) => "gray",
                (true, false, true) => "blue",
                (true, true, true) => "darkBlue",
                (false, true, false) => "red",
                (false, true, true) => "problem 1",
                (false, false, true) => "problem 2",

            };
            // stoel.chairColor = result;
            stoel.changeColour(result);

        }
    }
}
