# Command Line Intefrace Commands

Opencatapult provide a command line interface to interact with the API

## Login
Login to catapult

Usage:
<code>dotnet pc.dll login --user [user] --password [password]</code>

Options:
<details>
    <summary>user (mandatory)</summary>
    <p>
        <code>--user</code> (alias: <code>-u</code>)
    </p>
    <p>
        The user email used for login
    </p>
</details>
<details>
    <summary>password (mandatory)</summary>
    <p>
        <code>--password</code> (alias: <code>-p</code>)
    </p>
    <p>
        The password of the user
    </p>
</details>

## Logout
Logout from catapult

Usage:
<code>dotnet pc.dll logout</code>

## Account		
User account related commands			

### Subcommands

- [Register](../account/#register)
- [Remove](../account/#remove)
- [Suspend](../account/#suspend)
- [Activate](../account/#activate)