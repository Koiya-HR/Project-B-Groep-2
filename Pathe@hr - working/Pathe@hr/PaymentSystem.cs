namespace Pathe_hr.obj
{
    public class PaymentSystem
    {
        public Action _onPaymentSuccess;
        private Func<int> _getSecondsRemaining;

        private List<(int, int)> _selectedChairs;

        public void DeselectChairs2(List<(int row, int col)> selectedChairs, Stoel[,] stoelArray)
        {
            foreach (var chair in selectedChairs)
            {
                stoelArray[chair.row, chair.col].selected = false;
                stoelArray[chair.row, chair.col].free = true;
            }
            selectedChairs.Clear();
        }
        public PaymentSystem(Action onPaymentSuccess, Func<int> getSecondsRemaining, List<(int, int)> selectedChairs)
        {
            _onPaymentSuccess = onPaymentSuccess;
            _getSecondsRemaining = getSecondsRemaining;
            _selectedChairs = selectedChairs;
        }

        public void SelectPaymentMethodAndConfirm(List<(int row, int col)> selectedChairs, Stoel[,] stoelArray)
        {
            string[] paymentOptions = { "iDEAL", "PayPal", "Credit/Debit", "Cash (op locatie)", "Bestelling annuleren" };
            int selectedIndex = DisplayMenu(paymentOptions);

            // Betaal bevestiging afdrukken

            if (!Extras.isTimeLeft)
                return;

            switch (paymentOptions[selectedIndex])
            {
                case "iDEAL":
                    Console.WriteLine("U heeft betaald met iDEAL");
                    Console.WriteLine("Druk op een toets om terug te keren naar het hoofdmenu.");
                    break;
                case "PayPal":
                    Console.WriteLine("U heeft betaald met PayPal");
                    Console.WriteLine("Druk op een toets om terug te keren naar het hoofdmenu.");
                    break;
                case "Credit/Debit":
                    Console.WriteLine("U heeft betaald met Credit/Debit");
                    Console.WriteLine("Druk op een toets om terug te keren naar het hoofdmenu.");
                    break;
                case "Cash (op locatie)":
                    Console.WriteLine("U gaat met Cash op locatie betalen");
                    Console.WriteLine("Druk op een toets om terug te keren naar het hoofdmenu.");
                    break;
                case "Bestelling annuleren":
                    Reservation.CancelReservation();
                    DeselectChairs2(selectedChairs, stoelArray);
                    Console.WriteLine("bestelling geannuleerd");
                    Console.WriteLine("Druk op een toets om terug te keren naar het hoofdmenu.");
                    break;
                default:
                    Console.WriteLine("Onbekende betaalmethode");
                    break;
            }
            Extras.zaal.setChairsToTaken();
            _onPaymentSuccess.Invoke();
            Console.ReadKey();
        }

        private int DisplayMenu(string[] options)
        {
            int selectedIndex = 0;

            while (Extras.isTimeLeft)
            {
                Console.Clear();
                /*
                Console.WriteLine("Informatie:");
                foreach (var chair in _selectedChairs)
                {
                    Console.WriteLine($"- Rij: {chair.Item1 + 1}, Stoel: {chair.Item2 + 1}");
                }
                */
                StartScreen.DisplayAsciiArt();
                Console.WriteLine();
                Console.WriteLine("\u001b[38;2;250;156;55m=====================================================================================================================\u001b[0m");
                Console.WriteLine("--> Kies een betaalmethode:");
                Console.WriteLine($"\nGebruik de \u001b[38;2;250;156;55mPIJLTOETSEN\u001b[0m om te navigeren door dit menu \nDruk \u001b[38;2;250;156;55mENTER\u001b[0m om te selecteren\nDruk \u001b[38;2;250;156;55mESCAPE\u001b[0m om terug te gaan naar het hoofdmenu");
                Console.WriteLine("\u001b[38;2;250;156;55m=====================================================================================================================\u001b[0m");
                for (int i = 0; i < options.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("=> ");
                    }
                    else
                    {
                        Console.Write("   ");
                    }
                    Console.WriteLine(options[i]);
                    Console.ResetColor();
                }

                var seconds = _getSecondsRemaining.Invoke();
                Console.Write($"Resterende tijd: {seconds / 60:00}:{seconds % 60:00}   "); // Schrijf de nieuwe timerwaarde

                ConsoleKeyInfo key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex == 0) ? options.Length - 1 : selectedIndex - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex == options.Length - 1) ? 0 : selectedIndex + 1;
                        break;
                    case ConsoleKey.F:
                        Extras.zaal.chooseChairs();
                        break;
                    case ConsoleKey.Enter:
                        Extras.zaal.fillChairs();
                        return selectedIndex;
                }
            }
            return 0;
        }
    }
}



// namespace Pathe_hr.obj
// {
//     public class PaymentSystem
//     {
//         public Action _onPaymentSuccess;
//         private Func<int> _getSecondsRemaining;

//         private List<(int, int)> _selectedChairs;

//         public PaymentSystem(Action onPaymentSuccess, Func<int> getSecondsRemaining, List<(int, int)> selectedChairs)
//         {
//             _onPaymentSuccess = onPaymentSuccess;
//             _getSecondsRemaining = getSecondsRemaining;
//             _selectedChairs = selectedChairs;
//         }

//         public void SelectPaymentMethodAndConfirm()
//         {
//             StartScreen.DisplayAsciiArt();
//             Console.WriteLine("Kies een betaalmethode:");
//             string[] paymentOptions = { "iDEAL", "PayPal", "Credit/Debit", "Cash (op locatie)", "Bestelling annuleren" };
//             int selectedIndex = DisplayMenu(paymentOptions);

//             // Betaal bevestiging afdrukken

//             if (!Extras.isTimeLeft)
//                 return;

//             switch (paymentOptions[selectedIndex])
//             {
//                 case "iDEAL":
//                     Console.WriteLine("U heeft betaald met iDEAL");
//                     Console.WriteLine("Druk op een toets om terug te keren naar het hoofdmenu.");
//                     break;
//                 case "PayPal":
//                     Console.WriteLine("U heeft betaald met PayPal");
//                     Console.WriteLine("Druk op een toets om terug te keren naar het hoofdmenu.");
//                     break;
//                 case "Credit/Debit":
//                     Console.WriteLine("U heeft betaald met Credit/Debit");
//                     Console.WriteLine("Druk op een toets om terug te keren naar het hoofdmenu.");
//                     break;
//                 case "Cash (op locatie)":
//                     Console.WriteLine("U gaat met Cash op locatie betalen");
//                     Console.WriteLine("Druk op een toets om terug te keren naar het hoofdmenu.");
//                     break;
//                 case "Bestelling annuleren":
//                     Reservation.CancelReservation();
//                     Console.WriteLine("bestelling geannuleerd");
//                     Console.WriteLine("Druk op een toets om terug te keren naar het hoofdmenu.");
//                     break;
//                 default:
//                     Console.WriteLine("Onbekende betaalmethode");
//                     break;
//             }
//             Extras.zaal.setChairsToTaken();
//             _onPaymentSuccess.Invoke();
//             Console.ReadKey();
//         }

//         private int DisplayMenu(string[] options)
//         {
//             int selectedIndex = 0;

//             while (Extras.isTimeLeft)
//             {
//                 Console.Clear();
//                 /*
//                 Console.WriteLine("Informatie:");
//                 foreach (var chair in _selectedChairs)
//                 {
//                     Console.WriteLine($"- Rij: {chair.Item1 + 1}, Stoel: {chair.Item2 + 1}");
//                 }
//                 */
//                 Console.WriteLine("Gebruik de \u001b[38;2;250;156;55mpijltjestoetsen\u001b[0m om te navigeren en druk op \u001b[38;2;250;156;55mEnter\u001b[0m om te selecteren:");
//                 for (int i = 0; i < options.Length; i++)
//                 {
//                     if (i == selectedIndex)
//                     {
//                         Console.BackgroundColor = ConsoleColor.Gray;
//                         Console.ForegroundColor = ConsoleColor.Black;
//                         Console.Write("=> ");
//                     }
//                     else
//                     {
//                         Console.Write("   ");
//                     }
//                     Console.WriteLine(options[i]);
//                     Console.ResetColor();
//                 }

//                 var seconds = _getSecondsRemaining.Invoke();
//                 Console.Write($"Resterende tijd: {seconds / 60:00}:{seconds % 60:00}   "); // Schrijf de nieuwe timerwaarde

//                 ConsoleKeyInfo key = Console.ReadKey();

//                 switch (key.Key)
//                 {
//                     case ConsoleKey.UpArrow:
//                         selectedIndex = (selectedIndex == 0) ? options.Length - 1 : selectedIndex - 1;
//                         break;
//                     case ConsoleKey.DownArrow:
//                         selectedIndex = (selectedIndex == options.Length - 1) ? 0 : selectedIndex + 1;
//                         break;
//                     case ConsoleKey.F:
//                         Extras.zaal.chooseChairs();
//                         break;
//                     case ConsoleKey.Enter:
//                         Extras.zaal.fillChairs();
//                         return selectedIndex;
//                 }
//             }
//             return 0;
//         }
//     }
// }