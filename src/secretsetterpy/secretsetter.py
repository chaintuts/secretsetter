# This file contains code for fetching secrets from a secure keyvault
#
# Author: Josh McIntyre
#
import argparse
import sys

import secretfetcher

# The main entry point for the program
def main():

    parser = argparse.ArgumentParser(description="Set and fetch secret values in a keyvault")
    parser.add_argument("command", type=str, choices=["set", "get"], help="Action to perform with secret name")
    parser.add_argument("secret_name", type=str, help="The name of the secret")
    args = parser.parse_args()

    try:
        sf = secretfetcher.SecretFetcher()
    except secretfetcher.NoKeyvaultConnectionError:
        print("Error: Must provide KEYVAULT_URI in environment variables")
        sys.exit(1)

    if args.command == "get":
    
        # Fetch the secret from the keyvault
        secret_name = args.secret_name
        try:
            secret = sf.get_secret(secret_name)
            print(f"{secret_name}: {secret}")
            sys.exit(0)
        except secretfetcher.NoSecretFoundException:
            print(f"No secret found for {secret_name}")

    elif args.command == "set":
        
        # Get the secret value from the command line
        secret_name = args.secret_name
        secret = input("Enter secret value: ")
        confirm_secret = input("Confirm secret value: ")
        if secret != confirm_secret:
            print("Secrets do not match")
            sys.exit(1)
        
        # Set the secret in the keyvault
        try:
            sf.set_secret(secret_name, secret)
            print(f"{secret_name}: {secret}")
            sys.exit(0)
        except secretfetcher.NoSecretSetException:
            print(f"Unable to set secret {secret_name}")
            sys.exit(1)

if __name__ == "__main__":

	main()