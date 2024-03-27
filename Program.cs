public class Program
{
    public static void Main()
    {
        Zaal zaal = new(5, 10);
        bool run = true;
        while (run)
        {
            Console.WriteLine("1: show seats \n2: quit");
            string? input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    zaal.PrintStoelArray();
                    break;
                case "2":
                    run = false;
                    break;
                default:
                    Console.WriteLine("input not correct");
                    break;
            }
        }

    }
}
