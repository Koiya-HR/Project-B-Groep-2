
public class Program
{
    private static int selectedIndex = 0;
    private static readonly string arrow = "=>";


    public static void Main()
    {
        Zaal zaal = new(10, 20);
        Menu lidmaatschapMenu = new();
        Bioscoop bios = new("schouwbeurgplein");
        StartScreen.Screen(zaal, bios, lidmaatschapMenu); // startscherm van alihan
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
