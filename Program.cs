public class Program
{
    private static int selectedIndex = 0; 
    private static readonly string arrow = "=>"; 

    private static Zaal? zaal;
    private static Bioscoop? bios;

    public static void Main()
    {
        string[] menuOptions = { "Show seats", "Show movies", "Show tickets", "Koffie", "Cancel reservation", "Quit" };

        bool run = true;
        while (run)
        {
            Console.Clear();
            DisplayMenu(menuOptions);

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = (selectedIndex - 1 + menuOptions.Length) % menuOptions.Length;
                    break;
                case ConsoleKey.DownArrow:
                    selectedIndex = (selectedIndex + 1) % menuOptions.Length;
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    HandleMenuOption(menuOptions[selectedIndex]);
                    break;
                default:
                    Console.WriteLine("input not correct");
                    break;
            }
        }
    }

    private static void DisplayMenu(string[] options)
    {
        Console.WriteLine("Gebruik de \u001b[38;2;250;156;55mpijltjestoetsen\u001b[0m om te navigeren en druk op \u001b[38;2;250;156;55mEnter\u001b[0m om te selecteren:");
        for (int i = 0; i < options.Length; i++)
        {
            if (i == selectedIndex)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write($"{arrow} ");
            }
            else
            {
                Console.Write("   ");
            }
            Console.WriteLine($"{options[i]}");
            Console.ResetColor();
        }
    }

    public static void HandleMenuOption(string option)
    {
        switch (option)
        {
            case "Show seats":
                (new Zaal(10, 20)).chooseChairs();
                break;
            case "Show movies":
                bios.ChooseMovies(zaal);
                break;
            case "Show tickets":
                showTickets();
                break;
            case "Koffie":
                Koffie.Drank();
                break;
            case "Cancel reservation":
                Reservation.CancelReservation();
                break;
            case "Quit":
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Input not correct");
                break;
        }
    }


    public static void showTickets()
    {
        string jsonFilePath = "ticket_info.json";
        TicketController controller = new TicketController(jsonFilePath);

        int selectedOption = 1;
        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine("Welkom bij het ticketsysteem.");
            Console.WriteLine("Gebruik de \u001b[38;2;250;156;55mpijltjestoetsen\u001b[0m om te navigeren en druk op \u001b[38;2;250;156;55mEnter\u001b[0m om te selecteren:");
            for (int keuze = 1; keuze <= 3; keuze++)
            {
                if (keuze == selectedOption)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("=> ");
                }
                else
                {
                    Console.Write("   ");
                }
                switch (keuze)
                {
                    case 1:
                        Console.WriteLine("Toon ticketinformatie");
                        break;
                    case 2:
                        Console.WriteLine("Toon het bedrag van alle tickets");
                        break;
                    case 3:
                        Console.WriteLine("Exit");
                        break;
                }
                Console.ResetColor();
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                switch (selectedOption)
                {
                    case 1:
                        controller.ShowTicketInfo();
                        break;
                    case 2:
                        controller.ShowTotalAmount();
                        break;
                    case 3:
                        running = false;
                        break;
                }
                if (selectedOption != 3)
                {
                    Console.WriteLine("Druk op een toets om verder te gaan.");
                    Console.ReadKey(true);
                }
            }
            else if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                if (selectedOption > 1)
                    selectedOption--;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                if (selectedOption < 3)
                    selectedOption++;
            }
        }
        Console.Clear();
        return;
    }
}
