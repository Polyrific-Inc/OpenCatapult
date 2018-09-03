# Activate
Activate a suspended user

Usage: 
<code>dotnet pc.dll account activate --email [email] --password [password] --firstname [firstname] --lastname [lastname]</code>

**Options**
<details>
    <summary>email (mandatory)</summary>
    <p>
        <code>--email</code> (alias: <code>-e</code>)
    </p>
    <p>
        Email of the user to be activated
    </p>
</details>

# Register
Register a catapult user

Usage: 
<code>dotnet pc.dll account register --email [email] --password [password] --firstname [firstname] --lastname [lastname]</code>

**Options**
<details>
    <summary>email (mandatory)</summary>
    <p>
        <code>--email</code> (alias: <code>-e</code>)
    </p>
    <p>
        Email of the user
    </p>
</details>
<details>
    <summary>password (mandatory)</summary>
    <p>
        <code>--password</code> (alias: <code>-p</code>)
    </p>
    <p>
        Password of the user
    </p>
</details>
<details>
    <summary>firstname</summary>
    <p>
        <code>--firstname</code> (alias: <code>-fn</code>)
    </p>
    <p>
        First name of the user
    </p>
</details>
<details>
    <summary>lastname</summary>
    <p>
        <code>--lastname</code> (alias: <code>-ln</code>)
    </p>
    <p>
        Last name of the user
    </p>
</details>

# Remove
Remove a user

Usage: 
<code>dotnet pc.dll account remove --email [email]</code>

**Options**
<details>
    <summary>email (mandatory)</summary>
    <p>
        <code>--email</code> (alias: <code>-e</code>)
    </p>
    <p>
        Email of the user to be removed
    </p>
</details>

# Suspend
Suspend a user	

Usage: 
<code>dotnet pc.dll account suspend --email [email]</code>

**Options**
<details>
    <summary>email (mandatory)</summary>
    <p>
        <code>--email</code> (alias: <code>-e</code>)
    </p>
    <p>
        Email of the user to be suspended
    </p>
</details>