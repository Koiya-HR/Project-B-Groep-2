using System;

public class StartScreen
{
    public static void Screen(Zaal zaal, Bioscoop bios)
    {
        while (true)
        {
            DisplayAsciiArt();

            string[] options = { "Film lijst bekijken", "Tickets bekijken", "Lidmaatschap kopen", "Quit" };
            int currentOptionIndex = 0;

            DisplayOptions(options, currentOptionIndex);

            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    currentOptionIndex = (currentOptionIndex == 0) ? options.Length - 1 : currentOptionIndex - 1;
                    DisplayOptions(options, currentOptionIndex);
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    currentOptionIndex = (currentOptionIndex == options.Length - 1) ? 0 : currentOptionIndex + 1;
                    DisplayOptions(options, currentOptionIndex);
                }

            } while (keyInfo.Key != ConsoleKey.Enter);

            switch (currentOptionIndex)
            {
                case 0:
                    bios.ChooseMovies(zaal);
                    break;
                case 1:
                    Program.showTickets();
                    break;
                case 2:
                    Console.WriteLine("Lidmaatschap kopen");
                    break;
                case 3:
                    return;
            }
        }
    }

    private static void DisplayAsciiArt()
    {
        Console.WriteLine(@"
             _     _     _            
            | |   | |   (_)           
   __ _ _ __| |_  | |__  _  ___  ___  
  / _` | '__| __| | '_ \| |/ _ \/ __| 
 | (_| | |  | |_  | |_) | | (_) \__ \ 
  \__,_|_|   \__| |_.__/|_|\___/|___/ 
                                      
                                      
");
    }

    private static void DisplayOptions(string[] options, int currentOptionIndex)
    {
        Console.Clear();
        DisplayAsciiArt();

        Console.WriteLine("Choose an option:");

        for (int i = 0; i < options.Length; i++)
        {
            if (i == currentOptionIndex)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            Console.WriteLine($"{i + 1}. {options[i]}");
            Console.ResetColor();
        }
    }
}