/* This file contains a simple command-line UI for setting and fetching secrets in a keyvault
 * 
 * Author: Josh McIntyre
 */

using System;
using System.Net;


namespace secretsetter
{
    internal class Program
    {

        static void printUsage()
        {
            Console.WriteLine("Usage: secretsetter.exe set <secretname>");
            Console.WriteLine("Usage: secretsetter.exe get <secretname>");
        }

        static void Main(string[] args)
        {
            SecretFetcher secretFetcher;
            try
            {
                secretFetcher = new SecretFetcher();

                if (args.Length < 2)
                {
                    printUsage();
                    Environment.Exit(1);
                }
                if (args.Length == 2)
                {
                    // Get case - fetch the secret from the keyvault and print to the console
                    if (args[0] == "get")
                    {
                        var secretName = args[1];
                        try
                        {
                            var secret = secretFetcher.getSecret(secretName);

                            Console.WriteLine(secretName + ": " + secret);
                            Environment.Exit(0);
                        }
                        catch (SecretFetcher.NoSecretFoundException)
                        {
                            Console.WriteLine("No secret found for: " + secretName);
                            Environment.Exit(1);
                        }
                    }
                    // Set case - retrieve the secret from the command line and put in the keyvault
                    else if (args[0] == "set")
                    {
                        var secretName = args[1];

                        // Get the secret value from the command line and confirm a match
                        Console.Write("Enter secret value: ");
                        var secret = Console.ReadLine();
                        Console.Write("Confirm secret value: ");
                        var secretConfirm = Console.ReadLine();

                        if (secret != secretConfirm)
                        {
                            Console.WriteLine("Secrets do not match");
                            Environment.Exit(1);
                        }

                        // Set the secret in the keyvault
                        try
                        {
                            secretFetcher.setSecret(secretName, secret);

                            Console.WriteLine(secretName + ": " + secret);
                            Environment.Exit(0);
                        }
                        catch (SecretFetcher.NoSecretSetException)
                        {
                            Console.WriteLine("Unable to set secret: " + secretName);
                            Environment.Exit(1);
                        }
                    }
                    else
                    {
                        printUsage();
                    }
                }

            }
            catch (SecretFetcher.NoKeyvaultConnectionException)
            {
                Console.WriteLine("Error: Must provide KEYVAULT_URI in environment variables");
                Environment.Exit(1);
            }

        }
    }
}
