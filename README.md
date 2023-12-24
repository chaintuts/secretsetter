## General
____________

### Author
* Josh McIntyre

### Website
* jmcintyre.net

### Overview
* SecretSetter is a simple command-line secret storage tool using Azure Keyvaults

## Development
________________

### Git Workflow
* master for releases (merge development)
* development for bugfixes and new features

### Building
* Build using Visual Studio for C# version
* For Python version, copy scripts to desired directory

### Features
* Get a secret value by name
* Set a secret value by name 

### Requirements
* Requires the .NET framework
* Requires Python for the Python version
* Requires Azure CLI

### Platforms
* Windows
* Mac OSX
* Linux

## Usage
____________

### CLI Usage
* Set environment variable `KEYVAULT_URI` with the URL of the keyvault
* Run `secretsetter.exe set <secretname>` to set a secret in the keyvault - enter and confirm the secret value via the command line
* Run `secretsetter.exe get <secretname>` to fetch a secret value from the keyvault by name

### Python CLI Usage
* Set environment variable `KEYVAULT_URI` with the URL of the keyvault
* Run `python3 secretsetter.py set <secretname>` to set a secret in the keyvault - enter and confirm the secret value via the command line
* Run `python3 secretsetter.py get <secretname>` to fetch a secret value from the keyvault by name

