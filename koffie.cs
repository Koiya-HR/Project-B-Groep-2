using System;
using System.Collections.Generic;

class Koffie
{
    public static void Drank()
    {
        ConsoleKeyInfo choice;
        int selectedIndex = 0;
        string[] options = { "Ja", "Nee" };
        string[] options_2 = { "Drank", "Snacks" };
        string[] options_3 = { "Koffie", "Cappuccino", "Espresso", "Latte", "Groene Thee", "Zwarte thee", "Kamillethee", "Muntthee", "Gemberthee", "Rooibosthee" };
        string[] options_4 = { "friet", "chips", "frikandel", "mexicano" };

        double drinkPrice = 2.0;
        double snackPrice = 3.0;
        double totalCost = 0.0;
        List<string> orderList = new List<string>();


        Console.WriteLine("Voordat u verder gaat met uw bestelling, wilt u nog iets te eten of te drinken bestellen?");

        do
        {

            for (int i = 0; i < options.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.WriteLine(options[i]);
                Console.ResetColor();
            }

            choice = Console.ReadKey(true);

            if (choice.Key == ConsoleKey.UpArrow && selectedIndex > 0)
            {
                selectedIndex--;
            }
            else if (choice.Key == ConsoleKey.DownArrow && selectedIndex < options.Length - 1)
            {
                selectedIndex++;
            }


            Console.SetCursorPosition(0, Console.CursorTop - options.Length);

        } while (choice.Key != ConsoleKey.Enter);

        if (selectedIndex == 0)
        {
            Console.Clear();
            selectedIndex = 0;


            do
            {
                for (int i = 0; i < options_2.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.WriteLine(options_2[i]);
                    Console.ResetColor();
                }

                choice = Console.ReadKey(true);

                if (choice.Key == ConsoleKey.UpArrow && selectedIndex > 0)
                {
                    selectedIndex--;
                }
                else if (choice.Key == ConsoleKey.DownArrow && selectedIndex < options_2.Length - 1)
                {
                    selectedIndex++;
                }


                Console.SetCursorPosition(0, Console.CursorTop - options_2.Length);

            } while (choice.Key != ConsoleKey.Enter);

            Console.Clear();


            if (selectedIndex == 0)
            {
                selectedIndex = 0;

                do
                {

                    for (int i = 0; i < options_3.Length; i++)
                    {
                        if (i == selectedIndex)
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        Console.WriteLine($"{options_3[i]} - {drinkPrice} euro");
                        Console.ResetColor();
                    }

                    choice = Console.ReadKey(true);

                    if (choice.Key == ConsoleKey.UpArrow && selectedIndex > 0)
                    {
                        selectedIndex--;
                    }
                    else if (choice.Key == ConsoleKey.DownArrow && selectedIndex < options_3.Length - 1)
                    {
                        selectedIndex++;
                    }


                    Console.SetCursorPosition(0, Console.CursorTop - options_3.Length);

                    if (choice.Key == ConsoleKey.Enter)
                    {
                        orderList.Add(options_3[selectedIndex]);
                        totalCost += drinkPrice;
                    }

                } while (choice.Key != ConsoleKey.Enter);
            }
            else
            {
                selectedIndex = 0;

                do
                {

                    for (int i = 0; i < options_4.Length; i++)
                    {
                        if (i == selectedIndex)
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        Console.WriteLine($"{options_4[i]} - {snackPrice} euro");
                        Console.ResetColor();
                    }

                    choice = Console.ReadKey(true);

                    if (choice.Key == ConsoleKey.UpArrow && selectedIndex > 0)
                    {
                        selectedIndex--;
                    }
                    else if (choice.Key == ConsoleKey.DownArrow && selectedIndex < options_4.Length - 1)
                    {
                        selectedIndex++;
                    }


                    Console.SetCursorPosition(0, Console.CursorTop - options_4.Length);

                    if (choice.Key == ConsoleKey.Enter)
                    {
                        orderList.Add(options_4[selectedIndex]);
                        totalCost += snackPrice;
                    }

                } while (choice.Key != ConsoleKey.Enter);
            }
        }
        else
        {
            Console.Clear();
        }


        Console.WriteLine("Uw bestelling:");
        foreach (string item in orderList)
        {
            Console.WriteLine(item);
        }
        Console.WriteLine($"Totaal: {totalCost}");
    }
}
