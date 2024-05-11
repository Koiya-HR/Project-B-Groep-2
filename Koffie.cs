using System;
using System.Collections.Generic;

public class Koffie
{
    public static void Drank()
    {
        ConsoleKeyInfo choice;
        int selectedIndex = 0;
        string[] options = { "Ja", "Nee" };
        string[] options_3 = { "Koffie", "Cappuccino", "Espresso", "Latte", "Groene Thee", "Zwarte thee", "Kamillethee", "Muntthee", "Gemberthee", "Rooibosthee" };

        double drinkPrice = 2.0;
        double totalCost = 0.0;
        List<string> orderList = new List<string>();

        Console.WriteLine("Voordat u verder gaat met uw bestelling, wilt u nog iets te drinken bestellen?");
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

        Console.Clear();
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

            if (choice.Key == ConsoleKey.Spacebar)
            {
                orderList.Add(options_3[selectedIndex]);
                totalCost += drinkPrice;
            }
        } while (choice.Key != ConsoleKey.Enter);

        Console.Clear();


        Console.WriteLine("Uw bestelling:");
        foreach (string item in orderList)
        {
            Console.WriteLine(item);
        }
        Console.WriteLine($"Totaal: {totalCost} Euro");
    }
}
