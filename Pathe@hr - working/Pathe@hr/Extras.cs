using Pathe_hr.obj;

public static class Extras
{
    public static bool isTimeLeft = true; // timer
    public static string? gekozenFilm; // filmnaam 
    public static int EventID; // current event
    public static Zaal zaal; // het zaal object
    public static Bioscoop bios; // het bios object
    public static PaymentSystem paymentSystem;
    public static bool chairsCompleted = false; // is er betaald
    public static double completePrijs; // prijs van tickets en drank
    public static double drankPrijs; // totale prijs van dranken

    public static double ticketPrijs; // prijs voor alle tickets samen
    public static int numTickets; // aantal tickets 
    public static double lidmaatschapPrijs; // prijs voor lidmaatschap, hoeft niet op bonnetje

    public static bool stopTimer = false;
    public static bool noShowTimer = false;
    public static bool lidmaatschapToegepast = false;

    public static void resetMoney()
    {
        ticketPrijs = 0;
        numTickets = 0;
        lidmaatschapPrijs = 0;
        drankPrijs = 0;
        completePrijs = 0;
    }


}