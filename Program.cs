using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem
{
    class Program
    {
        static void Main(string[] args)
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
            Environment.Exit(0);
        }
    }
}
