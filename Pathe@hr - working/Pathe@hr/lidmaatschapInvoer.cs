
public class LidmaatschapInvoer
{
    public static double prijsZonderKorting = 0;
    public static double prijsMetKorting = 0;

    public static bool LidmaatschapInvoeren()
    {
        string invoerenLidmaatschap;

        bool inloggen = false;
        bool registreren = false;

        List<string> korting;

        Account account = new Account();
        bool lidmaatschapToegepast = false;

        do
        {
            do
            {

                Account.accountOptie = LidmaatschapAanvraag.Selecteren(new string[] { "Inloggen", "Account aanmaken", "Terug" }, "Log in om uw lidmaatschap te kunnen toepassen");

                switch (Account.accountOptie)
                {
                    case "Inloggen":
                        account.emailInvoeren();
                        if (Account.emailGevalideerd)
                        {
                            inloggen = account.Inloggen();
                        }
                        break;

                    case "Account aanmaken":
                        account.emailInvoeren();
                        if (Account.emailGevalideerd)
                        {
                            registreren = account.Registreren();
                        }
                        break;

                    case "Terug":
                        StartScreen.DisplayAsciiArt();
                        return false;
                }
            }
            while (inloggen == false && registreren == false && Account.accountOptie != "Terug");

            korting = account.HuidigeLidmaatschap();
            if (korting.Count == 0)
            {
                string lidmaanvraag = LidmaatschapAanvraag.Selecteren(new string[] { "Ja", "Nee" }, "U heeft geen bestaande lidmaatschap, wilt u er een aanvragen?:");
                if (lidmaanvraag == "Ja")
                {
                    account.LidmaatschapAanmaken();
                    korting = account.HuidigeLidmaatschap();
                }


            }
            if (korting.Count != 0)
            {
                prijsZonderKorting = Extras.completePrijs;
                foreach (string kortingsType in korting)
                {
                    if (kortingsType == "max 2 gratis tickets per aankoop")
                    {
                        double hoeveelheidKorting = 0;
                        if (Extras.numTickets == 1)
                        {
                            hoeveelheidKorting = 18.0;
                        }
                        else if (Extras.numTickets >= 2)
                        {
                            hoeveelheidKorting = 36.0;
                        }
                        Extras.ticketPrijs -= hoeveelheidKorting;
                    }

                    if (kortingsType == "max 2 gratis drankjes per ticket")
                    {
                        double hoeveelheidKortingDrankjes = 0;
                        int aantalGratisDrankjes = Extras.numTickets * 2;
                        double prijsGratisDrankjes = aantalGratisDrankjes * 2.0;

                        Extras.drankPrijs = Math.Max((Extras.drankPrijs - prijsGratisDrankjes), 0);
                    }

                    if (kortingsType == "max 2 gratis drankjes per bestelling")
                    {
                        double kortingDrankjes = 0;

                        if (Extras.drankPrijs > 4.0)
                        {
                            kortingDrankjes = 4.0;
                            Extras.drankPrijs -= kortingDrankjes;
                        }
                        else if (Extras.drankPrijs == 2.0)
                        {
                            kortingDrankjes = 2.0;
                            Extras.drankPrijs -= kortingDrankjes;
                        }
                    }
                    if (kortingsType == "50% korting op tickets")
                    {
                        double ticketPrijsMetKorting = Extras.ticketPrijs * 0.5;
                        Extras.ticketPrijs = ticketPrijsMetKorting;
                    }

                    if (kortingsType == "25% korting op tickets")
                    {
                        double ticketPrijsMetKorting = Extras.ticketPrijs - (Extras.ticketPrijs * 0.25);
                        Extras.ticketPrijs = ticketPrijsMetKorting;
                    }
                }
                Extras.completePrijs = Extras.drankPrijs + Extras.ticketPrijs;

                prijsMetKorting = Extras.completePrijs;

                lidmaatschapToegepast = true;

                return true;
            }
        }
        while (lidmaatschapToegepast != true);

        return false;
    }
}
