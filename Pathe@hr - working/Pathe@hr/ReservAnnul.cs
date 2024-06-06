using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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