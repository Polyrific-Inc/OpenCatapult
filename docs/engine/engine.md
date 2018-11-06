# Engine

Engine runs as a stand-alone console application. Its main job is to watch for any queued jobs, and execute them. It should always connect to API to check for the queued jobs, and report back the execution status. So it is important to make sure that the correct API Url has been correctly configured before starting the Engine.

## Architecture

![Architecture](../img/engine-arch.jpeg)

## Commands

Engine is equipped with some commands that serve different purpose:

### Config		
Set/modify engine Configurations

Usage: 
```sh
dotnet ocengine.dll config [subcommands]
```

#### Subcommands

* [get](config.md#get)
* [import](config.md#import)
* [remove](config.md#remove)
* [set](config.md#set)

### Check
Check for a queud job. 

Usage:
```sh
dotnet ocengine.dll check
```

| Option | Description | Default Value | Mandatory |
| --- | --- | --- | --- |
| --auto | Automatically run the job without asking for confirmation | false | No |

Note: If the `--auto` option is not included, it will prompt user whether she wants to execute the queued job or not.

### Start
Start the engine to periodically check for job queues and execute them.

Usage: 
```sh
dotnet ocengine.dll start
```