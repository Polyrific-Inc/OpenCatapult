# Account

## Activate

Activate a suspended user

Usage: 
`dotnet pc.dll account activate --email [email] --password [password] --firstname [firstname] --lastname [lastname]`

**Options**
* email (mandatory)
    * Email of the user to be activated
    * Usage: `--email` (alias `-e`)

# Register
Register a catapult user

Usage: 
`dotnet pc.dll account register --email [email] --password [password] --firstname [firstname] --lastname [lastname]`

**Options**
* email (mandatory)
    * Email of the user
    * Usage: `--email` (alias `-e`)
* password (mandatory)
    * Password of the user
    * Usage: `--password` (alias: `-p`)
* firstname
    * First name of the user
    * Usage: `--firstname` (alias: `-fn`)
* lastname
    * Last name of the user
    * Usage: `--lastname` (alias: `-ln`)

# Remove
Remove a user

Usage: 
`dotnet pc.dll account remove --email [email]`

**Options**
* email (mandatory)
    * Email of the user to be removed
    * Usage: `--email` (alias: `-e`)

# Suspend
Suspend a user	

Usage: 
`dotnet pc.dll account suspend --email [email]`

**Options**
* email (mandatory)
    * Email of the user to be suspended
    * Usage: `--email` (alias: `-e`)
