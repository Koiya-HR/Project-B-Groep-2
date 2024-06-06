using Pathe_hr.obj;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
//resetChairs(); om zaal stoelen te deselecteren=======================================================================
public class Zaal
{
    private bool isPaymentComplete;
    public PaymentSystem paymentSystem;

    private bool stopTimer = false;
    public int remainingTime = 20; // Total number of seconds for the countdown timer
    public const int maxRemainingTime = 20;
    public int numRows;
    public int chairsInRow;
    public List<Stoel> chairs { get; } = new();
    Stoel[,] stoelArray;
    List<string> messages = new();

    private int _numInvalideTickets;
    public int numInvalideTickets
    {
        get { return _numInvalideTickets; }
        set { _numInvalideTickets = (value > 1) ? 1 : value; }
    }
    private int _numNormaleTickets;
    public int numNormaleTickets
    {
        get { return _numNormaleTickets; }
        set { _numNormaleTickets = (value > 12) ? 12 : value; }
    }
    public const int maxChairsPerOrder = 12;

    private static int timerCursorPositionTop;
    private int ticketPrijs = 18;

    public List<(int row, int col)> selectedChairs { get; } = new List<(int row, int col)>();

    public Zaal(int numRows, int chairsInRow)
    {
        this.numRows = numRows;
        this.chairsInRow = chairsInRow;
        buildChairs();
        stoelArray = CreateStoelArray(numRows, chairsInRow);
        paymentSystem = new PaymentSystem(() => isPaymentComplete = true, () => remainingTime, selectedChairs);
        Extras.paymentSystem = paymentSystem;
    }
    // vullen van events========================================================================================


    public void fillChairs()
    {
        string filePath = "event.json";

        // Read the JSON file
        string json = File.ReadAllText(filePath);
        List<Event> events = JsonConvert.DeserializeObject<List<Event>>(json)!;

        for (int i = 0; i < events.Count; i++)
        {
            if (events[i].EventID == Extras.EventID)
            {
                foreach (EventChair stoel in events[i].Chairs!)
                {
                    if (stoel.Taken)
                    {
                        stoelArray[stoel.Row, stoel.Col].free = false;
                        stoelArray[stoel.Row, stoel.Col].selected = false;
                    }
                }
            }
        }
    }

    public void resetChairs()// 
    {
        for (int i = 0; i < stoelArray.GetLength(0); i++)
        {
            for (int j = 0; j < stoelArray.GetLength(0); j++)
            {
                stoelArray[i, j].free = true;
                stoelArray[i, j].selected = false;
            }
        }
    }

    public void makeEventChairs()
    {
        string filePath = "event.json";

        // Read the JSON file
        string json = File.ReadAllText(filePath);
        List<Event> events = JsonConvert.DeserializeObject<List<Event>>(json)!;

        // Fill the "Chairs" list for each event
        foreach (Event ev in events)
        {
            ev.Chairs = GenerateChairs();
        }

        // Serialize the updated events back to JSON
        string updatedJson = JsonConvert.SerializeObject(events, Formatting.Indented);

        // Write the updated JSON back to the file
        File.WriteAllText(filePath, updatedJson);
    }

    public static List<EventChair> GenerateChairs()
    {
        List<EventChair> chairs = new List<EventChair>();

        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 20; col++)
            {
                chairs.Add(new EventChair
                {
                    Row = row,
                    Col = col,
                    Taken = false
                });
            }
        }

        return chairs;
    }

    // vullen van events========================================================================================

    public void chooseChairs(bool onlyChooseChairs = false)
    {
        isPaymentComplete = false;
        stopTimer = false;
        bool chairsChosen = false;
        remainingTime = maxRemainingTime;
        //makeEventChairs(); //fill the json with chairs (only do this if new events are made)
        chooseTickets();
        resetChairs();// maak stoelen weer allemaal vrij
        fillChairs();// laad in de stoelen van gekozen event
        printInfo();
        PrintStoelArray();// print de gevulde stoelen
        // Start the countdown timer in a separate thread
        timerCursorPositionTop = Console.CursorTop;
        Thread timerThread = new Thread(() => ShowCountdownTimer(remainingTime));
        timerThread.Start();

        while (!chairsChosen && remainingTime > 0)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();

            if (remainingTime == 0)
                return;

            (int oldRow, int oldCol) = findSelectedChair();
            int newRow = oldRow;
            int newCol = oldCol;

            switch (keyInfo.Key)
            {
                case ConsoleKey.Enter:
                    if (selectedChairs.Count != 0)
                    {
                        chairsChosen = true;
                        makeTickets();
                        //setChairsToTaken();
                        break;
                    }
                    else
                    {
                        messages.Add("Selecteer eerst een stoel voordat u doorgaat.");
                    }
                    break;
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    newRow = (oldRow == 0) ? oldRow : oldRow - 1;
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    newRow = (oldRow >= stoelArray.GetLength(0) - 1) ? oldRow : oldRow + 1;
                    break;
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    newCol = (oldCol == 0) ? oldCol : oldCol - 1;
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    newCol = (oldCol >= stoelArray.GetLength(1) - 1) ? oldCol : oldCol + 1;
                    break;
                case ConsoleKey.Spacebar:
                    canBeSelectedCheck(newRow, newCol);
                    break;
                case ConsoleKey.F:
                    if (stoelArray[oldRow, oldCol].free && !stoelArray[oldRow, oldCol].selected)
                        stoelArray[oldRow, oldCol].free = false;
                    else if (!stoelArray[oldRow, oldCol].free && !stoelArray[oldRow, oldCol].selected)
                        stoelArray[oldRow, oldCol].free = true;
                    break;
                case ConsoleKey.Escape:
                    deselectChairs();
                    StopTimer();
                    return;

            }

            stoelArray[oldRow, oldCol].isCurrentChair = false;
            stoelArray[newRow, newCol].isCurrentChair = true;

            // Print necessary information
            Console.Clear();
            printInfo();
            PrintStoelArray();
            printNotifications();
            Console.Write($"Resterende tijd: {remainingTime / 60:00}:{remainingTime % 60:00}   ");
        }

        // If chairs were chosen and timer has not expired, proceed to payment menu
        if (chairsChosen)
        {
            // Wait for the payment confirmation
            Koffie.Drank();
            TicketBonSystem.betaalSysteem(selectedChairs, stoelArray);
            //paymentSystem.SelectPaymentMethodAndConfirm();
        }
    }

    public void StopTimer()
    {
        stopTimer = true;
    }

    public void deselectChairs()
    {
        foreach (var chair in selectedChairs)
        {
            stoelArray[chair.row, chair.col].selected = false;
            stoelArray[chair.row, chair.col].free = true;
        }
        selectedChairs.Clear();
    }
    public void setChairsToTaken()
    {
        string filePath = "event.json";

        // Read the JSON file
        string json = File.ReadAllText(filePath);
        List<Event> events = JsonConvert.DeserializeObject<List<Event>>(json)!;
        int index = events.FindIndex(e => e.EventID == Extras.EventID);
        if (index == -1)
        {
            Console.WriteLine("Event not found.");
            return;
        }
        foreach ((int row, int col) stoel in selectedChairs)
        {
            EventChair? chairToUpdate = events[index].Chairs.Find(c => c.Row == stoel.row && c.Col == stoel.col);
            if (chairToUpdate != null)
            {
                chairToUpdate.Taken = true;
            }
        }
        string updatedJson = JsonConvert.SerializeObject(events, Formatting.Indented);
        File.WriteAllText(filePath, updatedJson);
        selectedChairs.Clear();

    }


    private void makeTickets()
    {
        List<Ticket> tickets = new();
        string bonnetjedata = File.ReadAllText("bonnetje.json");
        Bonnetje bonnetje = JsonConvert.DeserializeObject<Bonnetje>(bonnetjedata);
        bonnetje.Stoelen = [];
        foreach ((int row, int col) stoel in selectedChairs)
        {
            string stoelNaam = Stoel.makeChairName(stoel.row, stoel.col);
            tickets.Add(new Ticket
            (
                Extras.gekozenFilm,
                stoelNaam,
                stoel.row,
                stoel.col,
                Extras.EventID,
                ticketPrijs
            ));
            bonnetje.Stoelen.Add(stoelNaam);
        }


        string ticketsJson = JsonConvert.SerializeObject(tickets, Formatting.Indented);
        File.WriteAllText("tickets.json", ticketsJson);


        string bonnetjesJson = JsonConvert.SerializeObject(bonnetje, Formatting.Indented);
        File.WriteAllText("bonnetje.json", bonnetjesJson);
    }


    public void checkTicketExists()
    {
        string jsonData = File.ReadAllText("tickets.json");

        List<Ticket> tickets = JsonConvert.DeserializeObject<List<Ticket>>(jsonData);
        if (tickets.Count == 0)
        {
            remainingTime = maxRemainingTime;
            return;
        }
    }

    private void chooseTickets()
    {
        int invalideSelectedOption = 2;
        bool invalideRunning = true;
        int normalSelectedOption = 2;
        bool normalRunning = true;
        while (invalideRunning)
        {
            Console.Clear();
            StartScreen.DisplayAsciiArt();
            Console.WriteLine("Heeft u een zijkant stoel kaartje nodig (deze zijn bestemd voor minder valide)");
            Console.WriteLine("Gebruik de \u001b[38;2;250;156;55mPIJL TOETSEN\u001b[0m om te bewegen, druk \u001b[38;2;250;156;55mENTER\u001b[0m om te selecteren en door te gaan");
            for (int choise = 1; choise <= 2; choise++)
            {
                if (choise == invalideSelectedOption)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }

                switch (choise)
                {
                    case 1:
                        Console.WriteLine("Ja");
                        break;
                    case 2:
                        Console.WriteLine("Nee");
                        break;
                }
                Console.ResetColor();
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                switch (invalideSelectedOption)
                {
                    case 1:
                        Console.WriteLine($"invalide: {numInvalideTickets}");
                        numInvalideTickets = 1;
                        invalideRunning = false;
                        break;
                    case 2:
                        Console.WriteLine($"normale: {numNormaleTickets}");
                        numInvalideTickets = 0;
                        invalideRunning = false;
                        break;
                }
            }
            else if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                if (invalideSelectedOption > 1)
                    invalideSelectedOption--;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                if (invalideSelectedOption < 2)
                    invalideSelectedOption++;
            }
        }
        Console.Clear();
        StartScreen.DisplayAsciiArt();

        while (normalRunning)
        {
            int tempNor;
            Console.WriteLine("hoeveel kaartjes naast mogelijk gekozen zijkant kaartjes wilt u, er is een maximum van 12 per aankoop");
            if (int.TryParse(Console.ReadLine(), out tempNor) && tempNor >= 0)
            {
                if (tempNor > 12)
                {
                    bool askYesOrNo = true;
                    while (askYesOrNo)
                    {
                        Console.Clear();
                        Console.WriteLine("U heeft meer dan 12 kaartjes geselecteerd, u mag maximaal 12 kaartjes selecteren. wilt u doorgaan met 12 kaartjes?");
                        for (int choise = 1; choise <= 2; choise++)
                        {
                            if (choise == normalSelectedOption)
                            {
                                Console.BackgroundColor = ConsoleColor.Gray;
                                Console.ForegroundColor = ConsoleColor.Black;
                            }
                            switch (choise)
                            {
                                case 1:
                                    Console.WriteLine("Ja");
                                    break;
                                case 2:
                                    Console.WriteLine("Nee");
                                    break;
                            }
                            Console.ResetColor();
                        }

                        ConsoleKeyInfo keyInfo = Console.ReadKey();

                        if (keyInfo.Key == ConsoleKey.Enter)
                        {
                            switch (normalSelectedOption)
                            {
                                case 1:
                                    tempNor = 12;
                                    askYesOrNo = false;
                                    normalRunning = false;
                                    break;
                                case 2:
                                    //normalRunning = true;//hier gebleven
                                    askYesOrNo = false;
                                    //normalRunning = true;
                                    break;
                            }
                        }
                        else if (keyInfo.Key == ConsoleKey.UpArrow)
                        {
                            if (normalSelectedOption > 1)
                                normalSelectedOption--;
                        }
                        else if (keyInfo.Key == ConsoleKey.DownArrow)
                        {
                            if (normalSelectedOption < 2)
                                normalSelectedOption++;
                        }
                    }
                }
                numNormaleTickets = tempNor;
                Console.Clear();
            }
            else if (tempNor < 0)
            {
                Console.WriteLine("Alleen positieve waarden graag");
            }
            if (tempNor > 0 && tempNor <= 12)
            {
                normalRunning = false;
            }

        }
    }



    private void canBeSelectedCheck(int rowToCheck, int colToCheck)
    {
        bool isFirstChair = true;
        int firstRow = 0;
        int firstCol = 0;
        int lastCol = 9999;
        bool canSelect = true;
        for (int i = 0; i < stoelArray.GetLength(0); i++)
        {
            for (int j = 0; j < stoelArray.GetLength(1); j++)
            {
                if (stoelArray[i, j].selected)  // per definitie dan de tweede of meerdere geselecteerde stoelen...
                {
                    if (isFirstChair)
                    {
                        firstRow = i;
                        firstCol = j;
                        lastCol = j;
                        isFirstChair = false;
                    }
                    else
                    {
                        lastCol = j;
                    }
                }
            }

        }

        if ((rowToCheck == firstRow && (colToCheck == firstCol - 1 || colToCheck == lastCol + 1)) || isFirstChair)
        // vinden van de meest linkse en meest rechtse geselecteerde stoel. Daarnaast zijn de enige twee stoelen ter evaluatie
        {
            /*to do
                - aangeven illegale stoelen en niet door laten gaan als niet is opgelost
                - maak een scherm
            */
            // als currentChair een randstoel is
            if (colToCheck == stoelArray.GetLength(1) || colToCheck == 0)
            {
                // als er niet genoeg invalide tickets zijn
                if (numInvalideTickets <= 0)
                {
                    messages.Add("Niet genoeg zijkant kaartjes geselecteerd, ga terug en selecteer meer kaartjes");
                    canSelect = false;
                }

            }
            // als er niet genoeg normale tickets zijn
            if (numNormaleTickets <= 0 && !stoelArray[rowToCheck, colToCheck].invalide)
            {
                messages.Add("Niet genoeg kaartjes geselecteerd, ga terug en selecteer meer kaartjes");
                canSelect = false;
            }
            // als de stoel in ieder geval 2 stoelen van de rand is zodat er kan worden gecheckt of er 2 stoelen naast vrij zijn
            else if (colToCheck >= 2 && colToCheck < stoelArray.GetLength(1) - 2)
            {
                if ((stoelArray[rowToCheck, colToCheck - 1].free && stoelArray[rowToCheck, colToCheck - 2].free) ||
                !stoelArray[rowToCheck, colToCheck - 1].free)
                {
                    //dit mag
                }
                else
                {
                    messages.Add("geen aftand van minimaal 2 lege stoelen aan de linker kant, selecteer ernaast of verderweg");
                    //stoelArray[rowToCheck, colToCheck].isIllegal = true;
                    canSelect = false;
                    // hier moet de stoel illegaal worden-----------------------------------------------------------------------------------------
                }
                if ((stoelArray[rowToCheck, colToCheck + 1].free && stoelArray[rowToCheck, colToCheck + 2].free) ||
                !stoelArray[rowToCheck, colToCheck + 1].free)
                {
                    //dit mag
                }
                else
                {
                    messages.Add("geen aftand van minimaal 2 lege stoelen aan de rechter kant, selecteer ernaast of verderweg");
                    canSelect = false;
                }
            }
            // als stoel aan de linker rand zit en 2 stoelen ernaast zijn vrij of 1 ernaast is bezet
            else if (colToCheck == 0)
            {
                if ((stoelArray[rowToCheck, colToCheck + 1].free && stoelArray[rowToCheck, colToCheck + 2].free) ||
                !stoelArray[rowToCheck, colToCheck + 1].free)
                {
                    //dit mag
                }
                else
                {
                    messages.Add("geen aftand van minimaal 2 lege stoelen aan de rechter kant, selecteer ernaast of verderweg");
                    canSelect = false;
                }
            }
            else if (colToCheck == stoelArray.GetLength(1))
            {
                if ((stoelArray[rowToCheck, colToCheck - 1].free && stoelArray[rowToCheck, colToCheck - 2].free) ||
                !stoelArray[rowToCheck, colToCheck - 1].free)
                {
                    //dit mag
                }
                else
                {
                    messages.Add("U mag geen aftand van minimaal 2 lege stoelen aan de linker kant over laten, selecteer ernaast of verderweg");
                    canSelect = false;
                }
            }
            // als stoel niet vrij is mag niet geselecteerd worden
            if (!stoelArray[rowToCheck, colToCheck].free)
            {
                messages.Add("deze stoel is al bezet");
                canSelect = false;
            }
            // als blijkt dat mag worden geselecteerd, selecteren
            if (canSelect)
            {
                selectedChairs.Add((rowToCheck, colToCheck));
                stoelArray[rowToCheck, colToCheck].selected = true;
                stoelArray[rowToCheck, colToCheck].free = false;
                if (stoelArray[rowToCheck, colToCheck].invalide)
                {
                    numInvalideTickets--;
                }
                else
                {
                    numNormaleTickets--;
                }
            }


        }

        // als stoel geselecteerd is en invalide
        else if (stoelArray[rowToCheck, colToCheck].selected && stoelArray[rowToCheck, colToCheck].invalide)
        {
            // als aan de linker kant zit en stoel ernaast is niet geselecteerd, deselecteer
            if (colToCheck == 0)
            {
                if (!stoelArray[rowToCheck, colToCheck + 1].selected)
                {
                    selectedChairs.Remove((rowToCheck, colToCheck));
                    stoelArray[rowToCheck, colToCheck].selected = false;
                    stoelArray[rowToCheck, colToCheck].free = true;
                    numInvalideTickets++;
                }
                else
                {
                    messages.Add("U kunt de zijkant stoel niet deselecteren als de stoel ernaast is geselecteerd");
                }
            }
            // als aan de rechter kant zit en stoel ernaast is niet geselecteerd, deselecteer
            else if (colToCheck == stoelArray.GetLength(1) - 1)
            {
                if (!stoelArray[rowToCheck, colToCheck - 1].selected)
                {
                    selectedChairs.Remove((rowToCheck, colToCheck));
                    stoelArray[rowToCheck, colToCheck].selected = false;
                    stoelArray[rowToCheck, colToCheck].free = true;
                    numInvalideTickets++;
                }
                else
                {
                    messages.Add("U kunt de zijkant stoel niet deselecteren als de stoel ernaast is geselecteerd");
                }
            }
        }
        // als stoel geselecteerd is en niet aan de randen zit
        else if (stoelArray[rowToCheck, colToCheck].selected && !stoelArray[rowToCheck, colToCheck].invalide)
        {

            if ((stoelArray[rowToCheck, colToCheck + 1].selected && !stoelArray[rowToCheck, colToCheck - 1].free && !stoelArray[rowToCheck, colToCheck - 1].selected) ||
            (stoelArray[rowToCheck, colToCheck - 1].selected && !stoelArray[rowToCheck, colToCheck + 1].free && !stoelArray[rowToCheck, colToCheck + 1].selected))
            {
                //stoelArray[rowToCheck, colToCheck].isIllegal = true;
                // hier moet de stoel illegaal worden-----------------------------------------------------------------------------------------
                messages.Add("U kunt geen enkele stoelen open laten");
            }
            // als een van de stoelen ernaast niet geselecteerd is, deselecteren
            else if (!stoelArray[rowToCheck, colToCheck + 1].selected || !stoelArray[rowToCheck, colToCheck - 1].selected)
            {
                selectedChairs.Remove((rowToCheck, colToCheck));
                stoelArray[rowToCheck, colToCheck].selected = false;
                stoelArray[rowToCheck, colToCheck].free = true;
                numNormaleTickets++;
            }
            else
            {
                messages.Add("Alle stoelen moeten verbonden zijn, u kunt stoelen tussen andere geselecteerde stoelen niet deselecteren");
            }
        }
        else
        {
            messages.Add("De stoelen zijn niet verbonden, selecteer stoelen naast elkaar");
        }
    }

    private void printInfo()
    {
        Console.WriteLine("Gebruik \u001b[38;2;250;156;55mW A S D\u001b[0m of de \u001b[38;2;250;156;55mPIJL KNOPPEN\u001b[0m om te bewegen, druk \u001b[38;2;250;156;55mSPATIE\u001b[0m om te selecteren en deselecteren en druk \u001b[38;2;250;156;55mENTER\u001b[0m om door te gaan");
        Console.WriteLine((numInvalideTickets == 1) ? $"U heeft \u001b[38;2;250;156;55m{numInvalideTickets}\u001b[0m zijkant stoel om te selecteren" : $"U heeft \u001b[38;2;250;156;55m{numInvalideTickets}\u001b[0m zijkant stoelen om te selecteren");
        Console.WriteLine((numNormaleTickets == 1) ? $"U heeft nog \u001b[38;2;250;156;55m{numNormaleTickets}\u001b[0m stoel om te selecteren" : $"U heeft nog \u001b[38;2;250;156;55m{numNormaleTickets}\u001b[0m stoelen om te selecteren");
        Console.WriteLine("\u001b[48;2;230;214;76m   \u001b[0m : vrij \n" +
                            "\u001b[48;2;110;110;110m   \u001b[0m : bezet \n" +
                            "\u001b[48;2;32;85;245m   \u001b[0m : geselecteerd \n" +
                            "\u001b[48;2;105;212;99m   \u001b[0m : U bent hier\n" +
                            "\u001b[48;2;217;164;17m   \u001b[0m : zijkant stoel");

        Console.WriteLine("                " + "==============================================================================================================");
        Console.WriteLine("                " + "                                                 BEELDSCHERM");
        Console.WriteLine("                " + "==============================================================================================================");
        Console.WriteLine();
    }

    private void printNotifications()
    {
        Console.WriteLine("Berichten:");
        for (int i = 0; i < messages.Count; i++)
        {
            Console.WriteLine($"\u001b[38;2;242;68;77m{messages[i]}\u001b[0m \n");
            messages.Remove(messages[i]);
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
                if (j == 0 || j == chairsInRow - 1)
                {
                    chairs.Add(new Stoel(i, j, true, true, chairsInRow));
                }
                else
                {
                    chairs.Add(new Stoel(i, j, true, false, chairsInRow));
                }
            }
        }
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
            string result = (stoel.isIllegal, stoel.invalide, stoel.free, stoel.isCurrentChair, stoel.selected) switch
            {
                (false, false, true, true, false) => "green",
                (false, true, true, true, false) => "green",
                (false, false, true, false, false) => "yellow",
                (false, true, true, false, false) => "orange",
                (false, false, false, false, false) => "gray",
                (false, true, false, false, false) => "gray",
                (false, false, false, false, true) => "blue",
                (false, true, false, false, true) => "blue",
                (false, false, false, true, true) => "darkBlue",
                (false, true, false, true, true) => "darkBlue",
                (false, false, false, true, false) => "red",
                (false, true, false, true, false) => "red",
                (true, false, true, true, true) => "light red",
                (true, false, true, false, true) => "red",
                _ => "problem 1"
            };
            // stoel.chairColor = result;
            stoel.changeColour(result);

        }
    }

    public void ShowCountdownTimer(int seconds)
    {
        Console.WriteLine("De timer is gestart...");
        while (seconds >= 0) // Blijf de timer uitvoeren totdat de tijd op is
        {
            if (isPaymentComplete)
                return;

            remainingTime = seconds;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write($"Resterende tijd: {seconds / 60:00}:{seconds % 60:00}   ");
            if (seconds == 10)
            {
                Console.WriteLine("Waarschuwing: U heeft nog 10 seconden!");
            }
            Thread.Sleep(1000);
            seconds--;

            // Controleer of de stopTimer vlag is ingesteld om de timer-thread te stoppen
            if (stopTimer || Extras.stopTimer)
            {
                // Reset de stopTimer vlag voordat de methode wordt verlaten
                stopTimer = false;
                Extras.stopTimer = false;
                // Verlaat de methode
                return;
            }
        }
        Console.WriteLine();

        // Reset de stopTimer vlag nadat de loop is gestopt
        stopTimer = false;
        deselectChairs();
        Console.WriteLine("De timer is gestopt.");
        Console.WriteLine("Druk op een toets om terug te keren naar het hoofdmenu.");
        Extras.isTimeLeft = false;
    }
}
