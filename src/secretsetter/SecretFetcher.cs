/* This file contains a simple wrapper for setting and fetching keyvault secrets
 * 
 * Author: Josh McIntyre
 */

using System;
using System.ComponentModel;
using System.Security;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

public class SecretFetcher
{

    private SecretClient client;

    // Define a custom exception for failed client setup
    public class NoKeyvaultConnectionException : Exception {}
    public class NoSecretFoundException : Exception {}
    public class NoSecretSetException : Exception {}

    public SecretFetcher()
    {
        setupClient();
    }

    // Configure the Azure client
    public SecretClient setupClient()
    {
        // Get keyvault URI from the environment configuration
        var keyVaultUri = Environment.GetEnvironmentVariable("KEYVAULT_URI");
        if (keyVaultUri == null)
        {
            throw new NoKeyvaultConnectionException();
        }

        // Create the secret client with the given config
        client = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());

        return client;
    }

    // Get a keyvault secret by name
    // Return the found secret string value
    // Or throw NoSecretFoundException
    public string getSecret(string secretName)
    {
        try
        {
            // Fetch the secret via the Azure client,
            // and extract the secret string value
            var secretRet = client.GetSecret(secretName);
            var secret = secretRet.Value.Value;

            return secret;
        }
        catch (Azure.RequestFailedException)
        {
            throw new NoSecretFoundException();
        }
    }

    // Set a keyvault secret by name
    // Throws NoSecretSetException if the secret cannot be set
    public void setSecret(string secretName, string secret)
    {
        try
        {
            // Set the secret via the Azure client
            var secretRet = client.SetSecret(new KeyVaultSecret(secretName, secret.ToString()));
        }
        catch (Azure.RequestFailedException)
        {
            throw new NoSecretSetException();
        }
    }
}
