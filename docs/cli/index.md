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

- [Register](account.md#register)
- [Remove](account.md#remove)
- [Suspend](account.md#suspend)
- [Activate](account.md#activate)

## Engine		
Catapult engine registration commands

Usage: 
<code>dotnet pc.dll engine [subcommands]</code>

### Subcommands

- [activate](engine.md#activate)
- [get](engine.md#get)
- [list](engine.md#list)
- [register](engine.md#register)
- [remove](engine.md#remove)
- [suspend](engine.md#suspend)
- [token](engine.md#token)

## Job		
Job Definition related commands

Usage: 
<code>dotnet pc.dll job [subcommands]</code>

### Subcommands

- [add](job.md#add)
- [list](job.md#list)
- [remove](job.md#remove)

## Member		
Project member related commands

Usage: 
<code>dotnet pc.dll member [subcommands]</code>

### Subcommands

- [add](member.md#add)
- [list](member.md#list)
- [remove](member.md#remove)
- [update](member.md#update)

## Model		
Project Data Model related commands

Usage: 
<code>dotnet pc.dll model [subcommands]</code>

### Subcommands

- [add](model.md#add)
- [list](model.md#list)
- [remove](model.md#remove)
- [update](model.md#update)

## Project		
Project related commands

Usage: 
<code>dotnet pc.dll project [subcommands]</code>

### Subcommands

- [archive](project.md#archive)
- [clone](project.md#clone)
- [create](project.md#create)
- [export](project.md#export)
- [List](project.md#list)
- [remove](project.md#remove)
- [restore](project.md#restore)

## Property		
Project Data Model Property related commands

Usage: 
<code>dotnet pc.dll property [subcommands]</code>

### Subcommands

- [add](property.md#add)
- [list](property.md#list)
- [remove](property.md#remove)
- [update](property.md#update)

## Property		
Job Queue related command.

Usage: 
<code>dotnet pc.dll queue [subcommands]</code>

### Subcommands

- [add](queue.md#add)
- [get](queue.md#get)
- [list](queue.md#list)
- [restart](queue.md#restart)

## Task		
Job Task Definitions related command.

Usage: 
<code>dotnet pc.dll task [subcommands]</code>

### Subcommands

- [add](task.md#add)
- [list](task.md#list)
- [remove](task.md#remove)
- [update](task.md#update)