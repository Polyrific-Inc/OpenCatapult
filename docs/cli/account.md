# Activate

Activate a suspended user

Usage: 
```sh
dotnet pc.dll account activate --email [email]
```

**Options**
* email (mandatory)
    * Email of the user to be activated
    * Usage: `--email` (alias `-e`)

# Register
Register a catapult user

Usage: 
```sh
dotnet pc.dll account register --email [email] --firstname [firstname] --lastname [lastname]
```

**Options**
* email (mandatory)
    * Email of the user
    * Usage: `--email` (alias `-e`)
* firstname
    * First name of the user
    * Usage: `--firstname` (alias: `-fn`)
* lastname
    * Last name of the user
    * Usage: `--lastname` (alias: `-ln`)

# Remove
Remove a user

Usage: 
```sh
dotnet pc.dll account remove --email [email]
```

**Options**
* email (mandatory)
    * Email of the user to be removed
    * Usage: `--email` (alias: `-e`)

# Suspend
Suspend a user	

Usage: 
```sh
dotnet pc.dll account suspend --email [email]
```

**Options**
* email (mandatory)
    * Email of the user to be suspended
    * Usage: `--email` (alias: `-e`)

# Update
Update user profile

Usage: 
```sh
dotnet pc.dll account update --email [email] --firstname [firstname] --lastname [lastname]
```

**Options**
* email (mandatory)
    * Email of the user
    * Usage: `--email` (alias `-e`)
* firstname
    * First name of the user
    * Usage: `--firstname` (alias: `-fn`)
* lastname
    * Last name of the user
    * Usage: `--lastname` (alias: `-ln`)
    
# Updatepassword
Update user's password. You will be prompted to input the old and new password.

Usage: 
```sh
dotnet pc.dll account updatepassword --email [email]
```

**Options**
* email (mandatory)
    * Email of the user
    * Usage: `--email` (alias `-e`)