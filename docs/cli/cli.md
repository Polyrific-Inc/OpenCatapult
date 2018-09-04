# Command Line Intefrace Commands

Opencatapult provide a command line interface to interact with the API

## Login
Login to catapult

Usage:
<code>dotnet pc.dll login --user [user] --password [password]</code>

**Options:**
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

## Engine		
Catapult engine registration commands

Usage: 
<code>dotnet pc.dll engine [subcommands]</code>

### Subcommands

- [activate](../engine/#activate)
- [get](../engine/#get)
- [list](../engine/#list)
- [register](../engine/#register)
- [remove](../engine/#remove)
- [suspend](../engine/#suspend)
- [token](../engine/#token)

## Job		
Job Definition related commands

Usage: 
<code>dotnet pc.dll job [subcommands]</code>

### Subcommands

- [add](../job/#add)
- [list](../job/#list)
- [remove](../job/#remove)

## Member		
Project member related commands

Usage: 
<code>dotnet pc.dll member [subcommands]</code>

### Subcommands

- [add](../member/#add)
- [list](../member/#list)
- [remove](../member/#remove)
- [update](../member/#update)

## Model		
Project Data Model related commands

Usage: 
<code>dotnet pc.dll model [subcommands]</code>

### Subcommands

- [add](../model/#add)
- [list](../model/#list)
- [remove](../model/#remove)
- [update](../model/#update)

## Project		
Project related commands

Usage: 
<code>dotnet pc.dll project [subcommands]</code>

### Subcommands

- [archive](../project/#archive)
- [clone](../project/#clone)
- [create](../project/#create)
- [export](../project/#export)
- [List](../project/#list)
- [remove](../project/#remove)
- [restore](../project/#restore)

## Property		
Project Data Model Property related commands

Usage: 
<code>dotnet pc.dll property [subcommands]</code>

### Subcommands

- [add](../property/#add)
- [list](../property/#list)
- [remove](../property/#remove)
- [update](../property/#update)

## Property		
Job Queue related command.

Usage: 
<code>dotnet pc.dll queue [subcommands]</code>

### Subcommands

- [add](../queue/#add)
- [get](../queue/#get)
- [list](../queue/#list)
- [restart](../queue/#restart)

## Task		
Job Task Definitions related command.

Usage: 
<code>dotnet pc.dll task [subcommands]</code>

### Subcommands

- [add](../task/#add)
- [list](../task/#list)
- [remove](../task/#remove)
- [update](../task/#update)