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

        // json data wordt in een array (collectie dictionaries) gestopt
        JArray jsonArray = JArray.Parse(jsonData);

        Console.Write("Ticket nummer: ");
        int intTicketNumber;
        string ticketNumber;
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out intTicketNumber))
            {
                ticketNumber = intTicketNumber.ToString();
                break;
            }
            else
            {
                Console.WriteLine("not correct input");
            }
        }


        RemoveReservationOutOfJSON(jsonArray, ticketNumber);

        // geupdate json data
        string updatedJsonData = jsonArray.ToString();

        // schrijft terug naar de json file
        File.WriteAllText(jsonPath, updatedJsonData);



    }
    public static void RemoveReservationOutOfJSON(JArray jsonArray, string ticketNumber)
    {
        bool ticketRemoved = false;
        for (int index = jsonArray.Count - 1; index >= 0; index--)
        {
            JObject reservation = (JObject)jsonArray[index];
            if (reservation["Ticket nummer"] != null)
            {
                if (reservation["Ticket nummer"]?.ToString() == ticketNumber)
                {

                    if (Convert.ToBoolean(reservation["verkocht"]) == false)
                    {
                        // als er nog niet betaald is en de gebruiker nog steeds in het systeem zit,
                        // wordt het totaal op 0 gezet
                        reservation["prijs"] = 0;
                    }
                    else if (Convert.ToBoolean(reservation["verkocht"]) == true)
                    {
                        // als er eerder gereserveerd is en al wel is betaald 
                        // en de gebruiker dit wilt annuleren, krijgt ie het bedrag teruggestort.

                        Console.WriteLine($"het bedrag: €{reservation["prijs"]} wordt naar u teruggestort.");
                    }
                    jsonArray.RemoveAt(index);
                    ticketRemoved = true;
                }
            }



        }
        if (!ticketRemoved)
        {
            Console.WriteLine($"No ticket found with ticket number {ticketNumber}");
        }
        else
        {
            Console.WriteLine($"Ticket with number: {ticketNumber} removed");
        }

    }
}
