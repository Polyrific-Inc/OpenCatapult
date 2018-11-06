# Add

Add job to queue

Usage: 
```sh
dotnet occli.dll queue add --project [project] --job [job]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |
| --job | -j | Name of the job definition ||| Yes |

# Get
Get a single job queue detailed record

Usage: 
```sh
dotnet occli.dll queue get --project [project] --number [number]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |
| --number | -n | Identifier of the queue (Id or Code) ||| Yes |

# List
List queued jobs

Usage: 
```sh
dotnet occli.dll queue list --project [project]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |
| --status | -s | Filter the queued jobs by their status | all, current, succeeded, failed | all | No |

# Log
Get complete log of a queued job

Usage: 
```sh
dotnet occli.dll queue log --project [project] --number [number]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |
| --number | -n | Identifier of the queue (Id or Code) ||| Yes |

# Restart
Restart the pending queue

Usage: 
```sh
dotnet occli.dll queue restart --project [project] --number [number]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |
| --number | -n | Identifier of the queue (Id or Code) ||| Yes |