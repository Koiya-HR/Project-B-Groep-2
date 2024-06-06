using System;
using System.Security.Cryptography;
using System.Text;


public class Key
{
    public static bool endLoop = false;
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