using System;
using System.Security.Cryptography;
using System.Text;


public class Program
{
    public static bool endLoop = false;
    public static void Main(string[] args)
    {

        while (!endLoop)
        {
            string? rawData;
            while (true)
            {
                Console.WriteLine("data to hash:");
                rawData = Console.ReadLine();
                if (rawData != null)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("give a correct input");
                }
            }
            string hashedKey = makeHash(rawData);// makeHash returnt de hash
            printInfo(rawData, hashedKey);
        }
    }


    public static void printInfo(string rawData, string result)
    {
        Console.WriteLine($"\u001b[38;2;58;156;58m{rawData}\u001b[0m will be converted into this:\n{result}");
        while (true)
        {
            Console.WriteLine("quit?  (Y/N)\n");
            string? ans = Console.ReadLine();
            if (ans == "y" || ans == "Y")
            {
                Console.WriteLine("byebye :)");
                endLoop = true;
                break;
            }
            else if (ans == "n" || ans == "N")
            {
                Console.WriteLine("time for another round...");
                break;
            }
            else
            {
                Console.WriteLine("ITS Y OR N !  (it's not that hard...)");
            }
        }
    }


    public static string makeHash(string? rawData)
    {
        if (rawData == null)
        {
            return "noData";// hier een check op maken zodat de functie niet "noData" in de json zet 
        }

        string result = string.Empty;

#pragma warning disable SYSLIB0021// een warning uitzetten, hier hoef je niet op te letten

        /*1. Create the hash algorithm instance. 
                You can choose from MD5, SHA1, SHA256, SHA384, 
                and SHA512.*/
        var myHash = SHA256Managed.Create();
#pragma warning restore SYSLIB0021// de warning weer aan zetten

        /*
            * 2. Invoke the ComputeHash method by passing 
                a byte array. 
            *    Just remember, you can pass any raw data, 
                and you need to convert that raw data 
                into a byte array.
            */
        var byteArrayResultOfRawData =
                Encoding.UTF8.GetBytes(rawData);

        /*
            * 3. The ComputeHash method, after a successful 
                execution it will return a byte array, 
            *    and you should store that in a variable. 
            */

        var byteArrayResult =
                myHash.ComputeHash(byteArrayResultOfRawData);

        /*
            * 4. After the successful execution of ComputeHash, 
                you can then convert 
                the byte array into a string. 
            */

        return result =
            string.Concat(Array.ConvertAll(byteArrayResult, h => h.ToString("X2")));
        /* dit is de uiteindelijke hashed string die naar de json moet 
        en die vergeleken moet worden met een nieuw gegeven gehashed wachtwoord
        */


    }
}

/* hash met 2 nummers
//=========================================================================================================
// C# Program to create a Hash
// Function for String data
using System;

class Geeks
{

    // Main Method
    public static void Main(String[] args)
    {

        // Declaring the an string array
        string[] values = new string[50];
        string str;

        // Values of the keys stored
        string[] keys = new string[] {"Alphabets",
            "Roman", "Numbers", "Alphanumeric",
                                "Tallypoints"};

        int hashCode;

        for (int k = 0; k < 5; k++)
        {

            str = keys[k];

            // calling HashFunction
            hashCode = HashFunction(str, values);

            // Storing keys at their hashcode's index
            values[hashCode] = str;
        }

        // Displaying Hashcodes along with key values
        for (int k = 0; k < (values.GetUpperBound(0)); k++)
        {

            if (values[k] != null)
                Console.WriteLine(k + " " + values[k]);
        }
    }

    // Defining the hash function
    static int HashFunction(string s, string[] array)
    {
        int total = 0;
        char[] c;
        c = s.ToCharArray();

        // Summing up all the ASCII values
        // of each alphabet in the string
        for (int k = 0; k <= c.GetUpperBound(0); k++)
            total += (int)c[k];

        return total % array.GetUpperBound(0);
    }
}
//=========================================================================================================
*/


/* encode dus niet hash

using System;
using System.Text;
using System.Security.Cryptography;

namespace HashConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            bool endLoop = false;
            while (!endLoop)
            {
                Console.WriteLine("data to hash:");
                string? plainData = Console.ReadLine();
                while (true)
                {
                    if (plainData != null)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("give an input");
                    }
                }

                Console.WriteLine($"Raw data: {plainData}");
                string hashedData = ComputeSha256Hash(plainData);
                Console.WriteLine($"Hash {hashedData}");
                Console.WriteLine(ComputeSha256Hash(plainData));
                Console.ReadLine();
            }
        }

        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
*/
