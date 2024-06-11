using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

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

        public void SelectPaymentMethodAndConfirm(string selectedOption, List<(int row, int col)> selectedChairs, Stoel[,] stoelArray, string filmnaam, DateTime starttijd, DateTime eindtijd, string locatie, string stoelen, string drankjes, double totalPrice, int numTickets, double ticketPrijs, double completePrijs)
        {
            string[] paymentOptions = { "iDEAL", "PayPal", "Credit/Debit", "Cash (op locatie)", "Bestelling annuleren" };
            int selectedIndex = DisplayMenu(paymentOptions);

            // Betaal bevestiging afdrukken

            if (!Extras.isTimeLeft)
            {
                return;
            }
            Console.Clear();
            StartScreen.DisplayAsciiArt();
            Console.WriteLine();
            switch (paymentOptions[selectedIndex])
            {
                case "iDEAL":
                    Console.WriteLine("\u001b[38;2;105;212;99mU heeft betaald met iDEAL\u001b[0m");
                    break;
                case "PayPal":
                    Console.WriteLine("\u001b[38;2;105;212;99mU heeft betaald met PayPal\u001b[0m");
                    break;
                case "Credit/Debit":
                    Console.WriteLine("\u001b[38;2;105;212;99mU heeft betaald met Credit/Debit\u001b[0m");
                    break;
                case "Cash (op locatie)":
                    Console.WriteLine("\u001b[38;2;105;212;99mU gaat met Cash op locatie betalen\u001b[0m");
                    break;
                case "Bestelling annuleren":
                    Reservation.CancelReservation();
                    DeselectChairs2(selectedChairs, stoelArray);
                    Extras.completePrijs = 0.0;
                    Extras.numTickets = 0;
                    Extras.ticketPrijs = 0.0;
                    Console.WriteLine("\u001b[38;2;230;214;76mBestelling geannuleerd\u001b[0m");
                    break;
                default:
                    Console.WriteLine("Onbekende betaalmethode");
                    break;
            }
            Extras.zaal.setChairsToTaken();
            _onPaymentSuccess.Invoke();
            Extras.resetMoney();
            string selectedPaymentMethod = paymentOptions[selectedIndex];

            // Vraag om e-mailadres en verzend het bonnetje per e-mail
            string emailAddress = GetEmailAddressFromUser();
            // Verstuur het e-mailbericht
            SendReceiptByEmail(emailAddress, filmnaam, starttijd, eindtijd, locatie, stoelen, drankjes, completePrijs, numTickets, ticketPrijs, selectedPaymentMethod);
        }

        public static void SendReceiptByEmail(string emailAddress, string filmnaam, DateTime starttijd, DateTime eindtijd, string locatie, string stoelen, string drankjes, double totalPrice, int numTickets, double ticketPrijs, string paymentMethod)
    {
        try
        {
            // SMTP-instellingen voor de e-mailserver van Gmail
            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
            {
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential("artbios.booking@gmail.com", "pluhosralpulbkzg");
                smtpClient.EnableSsl = true;

                // E-mailbericht samenstellen
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress("pathehr.booking@gmail.com");
                    mailMessage.To.Add(emailAddress);
                    mailMessage.Subject = "Uw bonnetje van ArtBios";

                    // Format the seats to ensure each is on a new line

                    mailMessage.Body = $"Beste klant,\n\nHierbij ontvangt u het bonnetje van uw recente aankoop bij ArtBios:\n\n" +
                        $"Film: {filmnaam}\nDatum & Tijd: {starttijd}\nEind Tijd: {eindtijd}\nLocatie: {locatie}\nStoelen:\n    {stoelen}" +
                        $"\nAantal Tickets: {numTickets}\nPrijs Tickets: €{ticketPrijs:F2}" +
                        $"\nDrankjes:\n{drankjes}\nBetalingswijze: {paymentMethod}\nTotale Prijs: €{totalPrice:F2}\nBedankt voor uw aankoop bij ArtBios!\nWij wensen u een fijne voorstelling bij de Film: {filmnaam}";


                    // Verstuur het e-mailbericht
                    smtpClient.Send(mailMessage);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fout bij het versturen van het bonnetje via e-mail: {ex.Message}");
        }
    }


    private static bool ConfirmEmailAddress(string emailAddress)
    {
        Console.Clear();
        StartScreen.DisplayAsciiArt();
        Console.WriteLine($"Weet u zeker dat \u001b[38;2;105;212;99m{emailAddress}\u001b[0m uw e-mailadres is?");
        Console.WriteLine("Gebruik de \u001b[38;2;250;156;55mPIJLTJESTOETSEN\u001b[0m om te navigeren en druk \u001b[38;2;250;156;55mENTER\u001b[0m om te selecteren");
    
        int selectedIndex = 0;
        ConsoleKeyInfo choice;
    
        do
        {
            // Toon opties en markeer de geselecteerde optie
            for (int i = 0; i < 2; i++)
            {
                if (i == selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.WriteLine(i == 0 ? "Ja" : "Nee");
                Console.ResetColor();
            }

            // Gebruikersinvoer vastleggen
            choice = Console.ReadKey(true);

            // Geselecteerd index bijwerken op basis van pijltoetsen
            if (choice.Key == ConsoleKey.UpArrow && selectedIndex > 0)
            {
                selectedIndex--;
            }
            else if (choice.Key == ConsoleKey.DownArrow && selectedIndex < 1)
            {
                selectedIndex++;
            }

            // Cursor omhoog verplaatsen om vorige opties te overschrijven
            Console.SetCursorPosition(0, Console.CursorTop - 2);

        } while (choice.Key != ConsoleKey.Enter);

        // Als 'Ja' is geselecteerd, toon de melding en wacht op toetsaanslag om terug te keren naar het hoofdmenu
        if (selectedIndex == 0)
        {
            Console.Clear();
            StartScreen.DisplayAsciiArt();
            Console.WriteLine($"Het bonnetje wordt naar uw e-mailadres \u001b[38;2;105;212;99m{emailAddress}\u001b[0m gestuurd.");
            Console.WriteLine("Wij wensen u een fijne voorstelling bij ArtBios");
            Console.WriteLine("U wordt nu teruggebracht naar het hoofdmenu");
            Thread.Sleep(5000);
        }

        // Geef true terug als 'Ja' is geselecteerd, anders false
        return selectedIndex == 0;
    }
    private int DisplayMenu(string[] options)
    {
        int selectedIndex = 0;

        while (Extras.isTimeLeft)
        {
            Console.Clear();
            StartScreen.DisplayAsciiArt();
            Console.WriteLine();
            Console.WriteLine("\u001b[38;2;250;156;55m=====================================================================================================================\u001b[0m");
            Console.WriteLine("--> Kies een betaalmethode:");
            Console.WriteLine($"\nGebruik de \u001b[38;2;250;156;55mPIJLTOETSEN\u001b[0m om te navigeren door dit menu \nDruk \u001b[38;2;250;156;55mENTER\u001b[0m om te selecteren");
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

    private static string GetEmailAddressFromUser()
    {
        string emailAddress;
        bool confirmed = false;

        do
        {
            Console.WriteLine("\u001b[38;2;250;156;55mVOER\u001b[0m alstublieft uw e-mailadres in om uw bonnetje te ontvangen:");
            emailAddress = Console.ReadLine();
            confirmed = ConfirmEmailAddress(emailAddress);

            if (!confirmed)
            {
                Console.Clear();
                StartScreen.DisplayAsciiArt();
                Console.WriteLine("Probeer opnieuw uw e-mailadres in te voeren.");
            }

        } while (!confirmed);

        return emailAddress;
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
