using Newtonsoft.Json.Linq;

static class Reservation
{
    public static void CancelReservation()
    {
        string jsonPath = "reserveringen.json";

        //de data uit het json bestand wordt ingelezen
        string jsonData = File.ReadAllText(jsonPath);

        // json data wordt in een array (collectie dictionaries) gestopt
        JArray jsonArray = JArray.Parse(jsonData);

        Console.Write("Ticket nummer: ");
        string ticketNumber = Console.ReadLine();

        RemoveReservationOutOfJSON(jsonArray, ticketNumber);

        // geupdate json data
        string updatedJsonData = jsonArray.ToString();

        // schrijft terug naar de json file
        File.WriteAllText(jsonPath, updatedJsonData);



    }
    public static void RemoveReservationOutOfJSON(JArray jsonArray, string ticketNumber)
    {
        for (int index = jsonArray.Count - 1; index >= 0; index--)
        {
            JObject reservation = (JObject)jsonArray[index];

            if (reservation["Ticket nummer"].ToString() == ticketNumber)
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
            }

        }


    }
}
