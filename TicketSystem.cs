using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;


public class Ticket
{
    public string Film { get; set; }
    public string Genre { get; set; }
    public int Stoelen { get; set; }
    public double Prijs { get; set; }
}

public class TicketController
{
    private List<Ticket> tickets = new List<Ticket>();

    public TicketController(string jsonFilePath)
    {
        string jsonData = File.ReadAllText(jsonFilePath);
        tickets = JsonConvert.DeserializeObject<List<Ticket>>(jsonData);
    }

    public void ShowTicketInfo()
    {
        Console.WriteLine("Ticket informatie:");
        foreach (Ticket ticket in tickets)
        {
            Console.WriteLine($"Film: {ticket.Film}");
            Console.WriteLine($"Genre: {ticket.Genre}");
            Console.WriteLine($"Stoelen: {ticket.Stoelen}");
            Console.WriteLine($"Prijs: {ticket.Prijs:C}");
            Console.WriteLine();
        }
    }

    public void ShowTotalAmount()
    {
        foreach (Ticket ticket in tickets)
        {
            Console.WriteLine($"Totaalbedrag voor alle tickets: {ticket.Prijs:C}");
        }
    }
}
