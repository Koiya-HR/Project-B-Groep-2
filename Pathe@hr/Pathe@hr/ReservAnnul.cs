using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/*

format tickets

[
    {
        "FilmID":1,
        "StoelRow":5,
        "StoelCol":14,
        "EventID":1,
        "StoelNaam": "05-14",
        "Prijs": 18.00
    },
    {
        "FilmID":2,
        "StoelRow":3,
        "StoelCol":10,
        "EventID":2,
        "StoelNaam": "03-10",
        "Prijs": 18.00
    }
]

*/



static class Reservation
{
    public static void CancelReservation()
    {
        string jsonPath = "tickets.json";

        //de data uit het json bestand wordt ingelezen
        string jsonData = File.ReadAllText(jsonPath);

        List<Ticket> allTickets = JsonConvert.DeserializeObject<List<Ticket>>(jsonData);
        allTickets = [];
        string json = JsonConvert.SerializeObject(allTickets, Formatting.Indented);
        File.WriteAllText("tickets.json", json);
        Console.WriteLine("bestelling verwijdert");
        Thread.Sleep(3000);

    }
}