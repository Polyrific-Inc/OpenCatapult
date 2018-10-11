# Engine

## Architecture

![Architecture](../img/engine-arch.jpeg)

## Commands
### Check
Check for a queud job. 

Usage:
```sh
dotnet pcengine.dll check
```

**Options:**
* auto
    * Use this option if you want to automatically run the job without asking for confirmation
    * Usage: `--auto`

### Config		
Set/modify engine Configurations

Usage: 
```sh
dotnet pcengine.dll config [subcommands]
```

#### Subcommands

* [get](config.md#get)
* [import](config.md#import)
* [remove](config.md#remove)
* [set](config.md#set)

### Start
Start the engine to periodically check for job queues and execute them.

Usage: 
```sh
dotnet pcengine.dll start
```