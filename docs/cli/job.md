# Add

Add a new job definition

Usage: 
```sh
dotnet occli.dll job add --project [project] --name [name]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |
| --name | -n | Name of the engine ||| Yes |

# List
List project's job definition

Usage: 
```sh
dotnet occli.dll job list --project [project]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |

# Get
Get a single job definition detailed record

Usage: 
```sh
dotnet occli.dll job get --project [project] --name [name]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |
| --name | -n | Name of the engine ||| Yes |

# Remove
Remove a job definition

Usage: 
```sh
dotnet occli.dll job remove --project [project] --name [name]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |
| --name | -n | Name of the engine ||| Yes |
| --autoconfirm | -ac | Perform the removal without asking for confirmation || false | No |

# Update
Update a job definition

Usage: 
```sh
dotnet occli.dll job update --project [project] --name [name] --rename [new name]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |
| --name | -n | Name of the engine ||| Yes |
| --rename | -rn | New name of the job definition ||| No |