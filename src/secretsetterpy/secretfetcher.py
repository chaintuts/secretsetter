# This file contains a simple wrapper for fetching a keyvault secret
#
# Author: Josh McIntyre
#
import os

from azure.core.exceptions import HttpResponseError, ResourceNotFoundError
from azure.keyvault.secrets import SecretClient
from azure.identity import DefaultAzureCredential

class NoKeyvaultConnectionException(Exception): pass
class NoSecretFoundException(Exception): pass
class NoSecretSetException(Exception): pass

class SecretFetcher:

    # Setup the Azure client
    def __init__(self):
    
        self.client = None

        self._setup_client()
        
    def _setup_client(self):

        kv_uri = os.environ.get("KEYVAULT_URI", None)
        if not kv_uri:
            raise NoKeyvaultConnectionException

        self.client = SecretClient(vault_url=kv_uri, credential=DefaultAzureCredential())
    
    # Fetch the secret value
    # Return the secret value string, or
    # raise NoSecretFoundException
    def get_secret(self, secret_name):
    
        try:
            secret_ret = self.client.get_secret(secret_name)
            secret = secret_ret.value
            
            return secret
        except ResourceNotFoundError:
            raise NoSecretFoundException
            
    # Set the secret by name
    # raise NoSecretSetException
    def set_secret(self, secret_name, secret):
    
        try:
            secret_ret = self.client.set_secret(secret_name, secret)
        except HttpResponseError:
            raise NoSecretSetException