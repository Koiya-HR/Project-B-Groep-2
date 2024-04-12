

using System.Runtime.Intrinsics.Arm;

public class Program
{
    public static void Main()
    {


        Zaal zaal = new(10, 20);
        Bioscoop bios = new("schouwbeurgplein");
        //StartScreen.Screen(zaal, bios);

        bool run = true;
        while (run)
        {
            Console.WriteLine("1: show seats \n2: show movies \n3: show tickets \n4: koffie \n5: cancel reservation \n6: quit");
            string? input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    zaal.chooseChairs();
                    break;
                case "2":
                    bios.ChooseMovies(zaal);
                    break;
                case "3":
                    showTickets();
                    break;
                case "4":
                    Koffie.Drank();
                    break;
                case "5":
                    Reservation.CancelReservation();
                    break;
                case "6":
                    run = false;
                    break;
                default:
                    Console.WriteLine("input not correct");
                    break;
            }
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
            Console.WriteLine("Gebruik de pijltjestoetsen om een optie te selecteren en druk op Enter om door te gaan:");
            for (int keuze = 1; keuze <= 3; keuze++)
            {
                if (keuze == selectedOption)
                {
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
