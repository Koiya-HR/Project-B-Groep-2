public class StartScreen
{
    public static void Screen(Menu lidmaatschapMenu)
    {
        Zaal zaal = Extras.zaal;
        Bioscoop bios = Extras.bios;
        while (true)
        {
            DisplayAsciiArt();

            string[] options = { "Film lijst bekijken", "Lidmaatschap kopen", "Quit" };
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
                else if (keyInfo.Key == ConsoleKey.A) // Admin functie met a aanroepen
                {
                    AdminSystem.Inlogscherm(); // Als er op a wordt gedrukt dan gaat de adminfunctie in 
                    DisplayOptions(options, currentOptionIndex); // Opties met scherm opnieuw aanroepen
                }
                else if (keyInfo.Key == ConsoleKey.F)//tijdelijk=======================================================================
                {
                    Program.showTickets();
                    DisplayOptions(options, currentOptionIndex);
                }

            } while (keyInfo.Key != ConsoleKey.Enter);

            switch (currentOptionIndex)
            {
                case 0:
                    bios.ChooseMovies(zaal);
                    break;
                case 1:
                    lidmaatschapMenu.LidmaatschapSysteem();
                    break;
                case 2:
                    return;
            }
        }
    }

    public static void DisplayAsciiArt()
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
        Console.WriteLine("Gebruik de \u001b[38;2;250;156;55mpijltjestoetsen\u001b[0m om te navigeren en druk op \u001b[38;2;250;156;55mEnter\u001b[0m om te selecteren:");

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


// public class StartScreen
// {
//     public static void Screen(Zaal zaal, Bioscoop bios, Menu lidmaatschapMenu)
//     {
//         while (true)
//         {
//             zaal.checkTicketExists();
//             DisplayAsciiArt();

//             string[] options = { "Film lijst bekijken", "Lidmaatschap kopen", "Admin", "Quit" };
//             int currentOptionIndex = 0;

//             DisplayOptions(options, currentOptionIndex);

//             ConsoleKeyInfo keyInfo;
//             do
//             {
//                 keyInfo = Console.ReadKey(true);

//                 if (keyInfo.Key == ConsoleKey.UpArrow)
//                 {
//                     currentOptionIndex = (currentOptionIndex == 0) ? options.Length - 1 : currentOptionIndex - 1;
//                     DisplayOptions(options, currentOptionIndex);
//                 }
//                 else if (keyInfo.Key == ConsoleKey.DownArrow)
//                 {
//                     currentOptionIndex = (currentOptionIndex == options.Length - 1) ? 0 : currentOptionIndex + 1;
//                     DisplayOptions(options, currentOptionIndex);
//                 }

//             } while (keyInfo.Key != ConsoleKey.Enter);

//             switch (currentOptionIndex)
//             {
//                 case 0:
//                     bios.ChooseMovies(zaal);
//                     break;
//                 case 1:
//                     lidmaatschapMenu.LidmaatschapSysteem();
//                     break;
//                 case 2:
//                     AdminSystem.Inlogscherm();
//                     break;
//                 case 3:
//                     return;
//             }
//         }
//     }

//     public static void DisplayAsciiArt()
//     {
//         Console.WriteLine(@"
//              _     _     _            
//             | |   | |   (_)           
//    __ _ _ __| |_  | |__  _  ___  ___  
//   / _` | '__| __| | '_ \| |/ _ \/ __| 
//  | (_| | |  | |_  | |_) | | (_) \__ \ 
//   \__,_|_|   \__| |_.__/|_|\___/|___/ 


// ");
//     }

//     private static void DisplayOptions(string[] options, int currentOptionIndex)
//     {
//         Console.Clear();
//         DisplayAsciiArt();

//         Console.WriteLine("Gebruik de \u001b[38;2;250;156;55mpijltjestoetsen\u001b[0m om te navigeren en druk op \u001b[38;2;250;156;55mEnter\u001b[0m om te selecteren:");

//         for (int i = 0; i < options.Length; i++)
//         {
//             if (i == currentOptionIndex)
//             {
//                 Console.BackgroundColor = ConsoleColor.Gray;
//                 Console.ForegroundColor = ConsoleColor.Black;
//             }
//             Console.WriteLine($"{i + 1}. {options[i]}");
//             Console.ResetColor();
//         }
//     }
// }