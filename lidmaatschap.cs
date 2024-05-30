using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class AccountGegevens
{
    public string Achternaam;
    public string Email;
    public string Wachtwoord;
    public int Leeftijd;
    public int Klantnummer;
    public bool Lidmaatschap;
    public string TypeLidmaatschap;
    public string DatumAanschaffing;
    public double PrijsLidmaatschap;
    public List<string> KortingsType;

    public AccountGegevens(string achternaam, string email, string wachtwoord, int leeftijd, int klantnummer, bool lidmaatschap, string typeLidmaatschap, string datumAanschaffing, double prijsLidmaatschap, List<string> kortingstype)
    {
        this.Achternaam = achternaam;
        this.Email = email;
        this.Wachtwoord = wachtwoord;
        this.Leeftijd = leeftijd;
        this.Klantnummer = klantnummer;
        this.Lidmaatschap = lidmaatschap;
        this.TypeLidmaatschap = typeLidmaatschap;
        this.DatumAanschaffing = datumAanschaffing;
        this.PrijsLidmaatschap = prijsLidmaatschap;
        this.KortingsType = kortingstype;
    }
}

public static class JsonDataManager
{
    public static AccountGegevens lidmaatschapObject;
    private static string jsonPath = "lidmaatschap.json";

    // account wordt toegevoegd aan het json bestand
    public static void AppendAccountToJson()
    {
        string jsonData = File.ReadAllText(jsonPath);

        JObject newLidmaatschap = JObject.FromObject(lidmaatschapObject);
        JArray jsonKlantLidmaatschappen = JArray.Parse(jsonData);
        jsonKlantLidmaatschappen.Add(newLidmaatschap);
        string updatedJsonData = jsonKlantLidmaatschappen.ToString();
        File.WriteAllText(jsonPath, updatedJsonData);
    }

    // account wordt verwijderd van json
    public static void DeleteAccountFromJson(string email)
    {
        string jsonData = File.ReadAllText(jsonPath);

        JArray jsonKlantLidmaatschappen = JArray.Parse(jsonData);
        for (int index = jsonKlantLidmaatschappen.Count - 1; index >= 0; index--)
        {
            JObject klantLidmaatschap = (JObject)jsonKlantLidmaatschappen[index];
            if (klantLidmaatschap["Email"].ToString() == email)
            {
                jsonKlantLidmaatschappen.RemoveAt(index);
            }
        }
        string updatedJsonData = jsonKlantLidmaatschappen.ToString();
        File.WriteAllText(jsonPath, updatedJsonData);
    }

    // lidmaatschap wordt toegevoegd of verwijderd in deze functie
    public static void lidmaatschapBijwerkenJson(string email, bool lidmaatschap, string typeLidmaatschap, string datumAanschaffing, double prijsLidmaatschap, List<string> kortingstype)
    {
        string jsonData = File.ReadAllText(jsonPath);
        JArray jsonKlantLidmaatschappen = JArray.Parse(jsonData);

        foreach (JObject klant in jsonKlantLidmaatschappen)
        {
            if (klant["Email"].ToString() == email)
            {
                // update lidmaatschap details
                klant["Lidmaatschap"] = lidmaatschap;
                klant["TypeLidmaatschap"] = typeLidmaatschap;
                klant["DatumAanschaffing"] = datumAanschaffing;
                klant["PrijsLidmaatschap"] = prijsLidmaatschap;
                klant["KortingsType"] = new JArray(kortingstype);
                break;
            }
        }
        string updatedJsonData = jsonKlantLidmaatschappen.ToString(Formatting.Indented);

        File.WriteAllText(jsonPath, updatedJsonData);
    }
}

public static class LidmaatschapAanvraag
{
    static List<string> KortingsType = new List<string>();
    static AccountGegevens lidmaatschapObject;
    static string typeLidmaatschap = "";
    static double prijsLidmaatschap;
    static string aanschafdatum;
    static string keuzeTypeLidmaatschap;
    static bool betaald;

    // lidmaatschapAanvragen():
    // hierin de opties van de lidmaatschappen en het toevoegen aan de gegevens ervan
    public static void LidmaatschapAanvragen(string email, int leeftijd)
    {
        if (leeftijd < 67)
        {
            keuzeTypeLidmaatschap = Selecteren(new string[] { "Unlimited: max 2 gratis tickets per aankoop", "Drinks on me: max 2 gratis drankjes per ticket", "Half price: 50% korting op tickets", "Quit" });
        }
        else
        {
            keuzeTypeLidmaatschap = Selecteren(new string[] { "Unlimited: max 2 gratis tickets per aankoop", "Drinks on me: max 2 gratis drankjes per ticket", "Half price: 50% korting op tickets", "Senior: 25% korting op tickets, max 2 gratis drankjes per bestelling", "Quit" });
        }

        DateTime currentDateTime = DateTime.Now;

        aanschafdatum = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");

        switch (keuzeTypeLidmaatschap)
        {
            case "Unlimited: max 2 gratis tickets per aankoop":
                typeLidmaatschap = "Unlimited";
                prijsLidmaatschap = 30.00;
                KortingsType.Add("max 2 gratis tickets per aankoop");
                break;
            case "Drinks on me: max 2 gratis drankjes per ticket":
                typeLidmaatschap = "Drinks on me";
                prijsLidmaatschap = 10.00;
                KortingsType.Add("max 2 gratis drankjes per ticket");
                break;
            case "Half price: 50% korting op tickets":
                typeLidmaatschap = "Half price";
                prijsLidmaatschap = 20.50;
                KortingsType.Add("50% korting op tickets");
                break;
            case "Senior: 25% korting op tickets, max 2 gratis drankjes per bestelling":
                typeLidmaatschap = "Senior";
                prijsLidmaatschap = 15.00;
                KortingsType.Add("25% korting op tickets");
                KortingsType.Add("max 2 gratis drankjes per bestelling");
                break;
            case "Quit":
                break;
        }

        if (keuzeTypeLidmaatschap != "Quit")

        // if zetten met als deze lidmaatschap al niet bestaat
        {
            StartScreen.DisplayAsciiArt();

            Console.WriteLine($"Gekozen Lidmaatschap: {typeLidmaatschap}\nBijbehorende korting: {string.Join(", ", KortingsType)}\nPrijs: €{prijsLidmaatschap}");
        }
        else
        {
            return;
        }

        // alleen als er is betaald kan de lidmaatschap worden toegevoegd
        betaald = Betalen();
        if (betaald)
        {
            JsonDataManager.lidmaatschapBijwerkenJson(email, true, typeLidmaatschap, aanschafdatum, prijsLidmaatschap, KortingsType);
            Prompt("Lidmaatschap succesvol aangevraagd!");
        }

        KortingsType.Clear();
    }

    public static bool Betalen()
    {
        string bevestigen = "";
        Console.WriteLine($"Te betalen: €{prijsLidmaatschap}");
        Thread.Sleep(5000);
        string betaalOptie = Selecteren(new string[] { "Betalen", "Quit" });

        if (betaalOptie == "Betalen")
        {
            string bank = Selecteren(new string[] { "American Express", "Mastercard", "Google Pay", "Apple Pay", "IDEAL", "VISA", "PayPal", "Quit" });

            if (bank != "Quit")
            {
                bevestigen = Selecteren(new string[] { "Betaling bevestigen", "Quit" });
            }

        }

        if (bevestigen == "Betaling bevestigen")
        {
            Prompt("Betaling is voltooid!");
            return true;
        }
        else
        {
            return false;
        }
    }

    // Selecteren():
    // in deze functie krijgt de gebruiker de optie om dingen te selecteren
    public static string Selecteren(string[] Opties, string message = "")
    {

        string[] opties = Opties;
        int geselecteerdeIndex = 0;

        while (true)
        {
            Console.Clear();
            StartScreen.DisplayAsciiArt();
            if (message != "")
            {
                Console.WriteLine(message);
            }
            for (int i = 0; i < opties.Length; i++)
            {
                if (i == geselecteerdeIndex)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine($"=>  {opties[i]}");
                }
                else
                {
                    Console.WriteLine($"    {opties[i]}");
                }
                Console.ResetColor();
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            Console.Clear();
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    geselecteerdeIndex = (geselecteerdeIndex - 1 + opties.Length) % opties.Length;
                    break;
                case ConsoleKey.DownArrow:
                    geselecteerdeIndex = (geselecteerdeIndex + 1) % opties.Length;
                    break;
                case ConsoleKey.Enter:
                    return opties[geselecteerdeIndex];
            }
        }
    }

    // lidmaatschapOpties():
    // als de lidmaatschap bestaat krijgt gebruiker de opties lidmaatschap opzeggen of wijzigen
    // als de lidmaatschap (nog) niet bestaat krijgt de gebruiker de optie om deze aan te vragen
    public static void lidmaatschapOpties(bool lidmaatschapExists, string email, int leeftijd)
    {
        string choice1 = "";
        string choice2 = "";
        while (choice1 != "Uitloggen" && choice2 != "Uitloggen")
        {
            if (lidmaatschapExists)
            {

                choice1 = Selecteren(new string[] { "Lidmaatschap opzeggen", "Lidmaatschap wijzigen", "Uitloggen" });
                switch (choice1)
                {
                    case "Lidmaatschap opzeggen":
                        Thread.Sleep(1000);
                        string bevestigingOpzeggen = Selecteren(new string[] { "Ja", "Nee" }, "Weet u zeker dat u uw lidmaatschap wilt opzeggen?:");
                        if (bevestigingOpzeggen == "Ja")
                        {
                            JsonDataManager.lidmaatschapBijwerkenJson(email, false, "", null, 0.0, []);
                            lidmaatschapExists = false;
                            Prompt("Lidmaatschap opgezegd");

                        }

                        break;

                    case "Lidmaatschap wijzigen":

                        LidmaatschapAanvragen(email, leeftijd);
                        break;

                    case "Uitloggen":
                        Console.WriteLine("Uitloggen bij ART BIOS...");
                        Thread.Sleep(1500);
                        Prompt("Uitgelogd");
                        break;
                }
            }
            if (!lidmaatschapExists)
            {
                choice2 = Selecteren(new string[] { "Lidmaatschap aanvragen", "Uitloggen" });

                switch (choice2)
                {
                    case "Lidmaatschap aanvragen":
                        LidmaatschapAanvragen(email, leeftijd);
                        if (betaald)
                        {
                            lidmaatschapExists = true;
                        }
                        break;
                    case "Uitloggen":
                        Console.WriteLine("Uitloggen bij ART BIOS...");

                        Thread.Sleep(1500);
                        Prompt("Uitgelogd");
                        break;
                }
            }
        }
    }

    // Prompt():
    // deze functie wordt aangeroepen om een bericht mee te geven nadat er is geselecteerd
    public static void Prompt(string prompt)
    {
        Thread.Sleep(2000);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(prompt);
        Console.ResetColor();
        Thread.Sleep(1000);


    }
}

public class Account
{
    string email = "";


    Random random = new Random();
    JObject klant = null;
    JArray jsonAccounts;

    string juisteWachtwoord = "";

    string achternaam;
    string wachtwoord;

    bool accountExists;
    bool lidmaatschapExists;
    bool terug = false;

    int klantnummer;
    int leeftijd;
    int x = 0;
    public static bool emailGevalideerd = false;
    public static string accountOptie;
    // emailInvoeren():
    // kijkt of de ingevoerde e-mailadress al bestaat of niet
    // bestaat het, dan worden alle gegevens in het object klant gestopt
    // kijkt of er al een lidmaatschap bij de bijbehorende account bestaat

    public void emailInvoeren()
    {
        StartScreen.DisplayAsciiArt();
        emailGevalideerd = false;


        do
        {
            Console.Write("Voer uw e-mailadres in: ");

            if (Console.ReadKey(true).Key == ConsoleKey.Backspace)
            {
                return;
            }


            email = Console.ReadLine();

            emailGevalideerd = true;
            bool emailbestaat = emailBestaat(email);

            if (accountOptie == "Account aanmaken")
            {
                if (!email.Contains("@") || !email.Contains(".") || email.Length < 7)
                {
                    ErrorMessage("Ongeldig e-mailadres. Probeer opnieuw.(Zorg er voor dat het e-mailaddress bestaat uit minimaal 7 tekens en een @ en . bevat)");
                    Console.Clear();
                    StartScreen.DisplayAsciiArt();
                    emailGevalideerd = false;


                }
                if (emailbestaat)
                {

                    ErrorMessage("Deze e-mailadress wordt al gebruikt, probeer een andere e-mailadress.");
                    emailGevalideerd = false;

                }
            }
            else if (accountOptie == "Inloggen")
            {
                if (!emailbestaat)
                {
                    ErrorMessage("Account bestaat niet");
                    emailGevalideerd = false;

                }
            }

        }
        while (emailGevalideerd == false);

    }
    public bool emailBestaat(string email)
    {
        accountExists = false;
        lidmaatschapExists = false;

        FileInfo fileInfo = new FileInfo("lidmaatschap.json");

        if (fileInfo.Length == 0)
        {
            List<string> legeLijst = new List<string>();

            string jsonList = JsonConvert.SerializeObject(legeLijst);
            File.WriteAllText("lidmaatschap.json", jsonList);

        }

        string jsonData = File.ReadAllText("lidmaatschap.json");
        jsonAccounts = JArray.Parse(jsonData);

        foreach (JObject account in jsonAccounts)
        {
            if (account["Email"].ToString() == email)
            {
                accountExists = true;
                klant = account;
                juisteWachtwoord = account["Wachtwoord"].ToString();

                if ((bool)account["Lidmaatschap"] == true)
                {
                    lidmaatschapExists = true;
                }
                return true;
            }
        }
        return false;

    }



    // Inloggen():
    // in deze functie wordt ingelogd
    // de waardes van het klant object worden meegegevn aan ieder variable 
    public bool Inloggen()
    {
        Console.Clear();
        StartScreen.DisplayAsciiArt();
        if (accountExists)
        {
            Console.Write("Voer uw wachtwoord in: ");
            wachtwoord = Wachtwoord();
            wachtwoord = Key.makeHash(wachtwoord);

            while (wachtwoord != juisteWachtwoord)
            {
                ErrorMessage("Wachtwoord is onjuist, probeer opnieuw.");
                Console.Write("Voer uw wachtwoord in: ");
                wachtwoord = Wachtwoord();
                wachtwoord = Key.makeHash(wachtwoord);

            }

            leeftijd = int.Parse(klant["Leeftijd"].ToString());
            klantnummer = int.Parse(klant["Klantnummer"].ToString());
            achternaam = klant["Achternaam"].ToString();
            return true;
        }
        return false;


    }

    // Registreren():
    // in deze functie worden de accounts aangemaakt
    // er wordt gecheckt op wachtwoord
    // automatisch klantnummer aangemaakt
    // er kan alleen een account aan worden gemaakt als de e-mailadress niet al bestaat
    public bool Registreren()
    {
        Console.Clear();
        StartScreen.DisplayAsciiArt();


        if (!accountExists)

        {
            bool klantnummerBestaat = false;

            Console.Write("Voer uw achternaam in: ");
            achternaam = Console.ReadLine();

            Console.Write("Voer uw leeftijd in: ");
            leeftijd = Convert.ToInt32(Console.ReadLine());


            if (leeftijd < 18)
            {
                ErrorMessage("Te jong om lidmaatschap aan te vragen");
                return false;
            }
            Console.Write("Maak wachtwoord aan: ");
            wachtwoord = Wachtwoord();


            while (wachtwoord.Length < 7 || !wachtwoord.Any(char.IsUpper) || !wachtwoord.Any(char.IsDigit))
            {
                ErrorMessage("Ongeldig wachtwoord. Het wachtwoord moet minimaal 7 tekens lang zijn en minstens één hoofdletter en één cijfer bevatten.");
                Console.Write("Maak wachtwoord aan: ");
                wachtwoord = Wachtwoord();
            }
            string hashedWachtwoord = Key.makeHash(wachtwoord);
            do
            {
                klantnummer = random.Next(2000, 20000);
                foreach (JObject account in jsonAccounts)
                {
                    if (int.Parse(account["Klantnummer"].ToString()) == klantnummer)
                    {
                        klantnummerBestaat = true;
                        break;
                    }
                }
            }
            while (klantnummerBestaat);

            JsonDataManager.lidmaatschapObject = new AccountGegevens(achternaam, email, hashedWachtwoord, leeftijd, klantnummer, false, "", null, 0.0, []);

            JsonDataManager.AppendAccountToJson();
            accountExists = true;
            return true;
        }
        return false;
    }

    public static string Wachtwoord()
    {
        string wachtwoord = "";
        int cursorPos = 0;
        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(true);
            if (key.Key != ConsoleKey.Escape && key.Key != ConsoleKey.Enter)
            {
                if (key.Key != ConsoleKey.Backspace)
                {
                    wachtwoord = wachtwoord.Insert(cursorPos, key.KeyChar.ToString());
                    cursorPos++;
                    Console.Write("*");
                }
                else if (cursorPos > 0)
                {
                    cursorPos--;
                    wachtwoord = wachtwoord.Remove(cursorPos, 1);
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    Console.Write(" ");
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                }
            }
        }
        while (key.Key != ConsoleKey.Enter);

        Console.WriteLine();
        return wachtwoord;
    }
    public void AccountOpties()
    {
        do
        {
            bool ingelogd = false;
            bool geregistreerd = false;
            emailGevalideerd = false;

            accountOptie = LidmaatschapAanvraag.Selecteren(new string[] { "Inloggen", "Account aanmaken", "Account verwijderen", "Quit" });
            switch (accountOptie)
            {
                case "Inloggen":

                    emailInvoeren();
                    if (emailGevalideerd)
                    {
                        Console.Clear();

                        ingelogd = Inloggen();
                    }



                    break;
                case "Account aanmaken":
                    emailInvoeren();

                    if (emailGevalideerd)
                    {
                        Console.Clear();

                        geregistreerd = Registreren();
                    }
                    break;


                case "Account verwijderen":
                    emailInvoeren();

                    if (emailGevalideerd)
                    {

                        Console.Clear();

                        bool inloggen = Inloggen();
                        if (inloggen == true)
                        {
                            string AccVerwijderen = LidmaatschapAanvraag.Selecteren(new string[] { "Account verwijderen bevestigen", "Quit" });

                            if (AccVerwijderen == "Account verwijderen bevestigen")
                            {
                                LidmaatschapAanvraag.Prompt("Account verwijderd");
                                JsonDataManager.DeleteAccountFromJson(email);
                                accountExists = false;
                            }
                        }
                    }
                    break;

                case "Quit":
                    return;
            }


            if (ingelogd != false || geregistreerd != false)
            {
                LidmaatschapAanvraag.lidmaatschapOpties(lidmaatschapExists, email, leeftijd);
            }

        }
        while (accountOptie != "Quit");
    }

    public static void ErrorMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
        Thread.Sleep(3000);
    }


    // kijkt of er een lidmaatschap bestaat zo ja returnt het de bijbehorende korting
    public List<string> HuidigeLidmaatschap()
    {

        string jsonData = File.ReadAllText("lidmaatschap.json");
        JArray jsonKlantLidmaatschappen = JArray.Parse(jsonData);

        foreach (JObject klant in jsonKlantLidmaatschappen)
        {
            if (klant["Email"].ToString() == email)
            {
                if (klant["Lidmaatschap"].ToObject<bool>() == true)
                {
                    List<string> kortingstypes = klant["KortingsType"].ToObject<List<string>>();
                    return kortingstypes;
                    break;
                }
            }
        }
        return [];
    }
    public void LidmaatschapAanmaken()
    {
        LidmaatschapAanvraag.LidmaatschapAanvragen(email, leeftijd);
    }


}

class Menu
{
    public void LidmaatschapSysteem()
    {
        Account account = new Account();
        account.AccountOpties();
    }
}