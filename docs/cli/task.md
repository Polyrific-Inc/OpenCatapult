# Add

Add a new job task definition

Usage: 
```sh
dotnet occli.dll task add --project [project] --job [job] --name [name] --type [type] --provider [provider] --property [propname]:[propvalue]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |
| --job | -j | Name of the job definition ||| Yes |
| --name | -n | Name of the new job task ||| Yes |
| --provider | -prov | Name of the provider for the task ||| Yes |
| --type | -t | Type of the job task | Clone, Generate, Push, Merge, Build, PublishArtifact, Deploy, DeployDb, Test | Generate | No |
| --property [key]:[value] | -prop | Property of the job task || Allowed multiple | No |
| --sequence | -s | The sequence of the task when run by engine ||| No |

# Get
Get a single job task definition

Usage: 
```sh
dotnet occli.dll task get --project [project] --job [job] --name [name]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |
| --job | -j | Name of the job definition ||| Yes |
| --name | -n | Name of the job task ||| Yes |

# List
List job task definitions

Usage: 
```sh
dotnet occli.dll task list --project [project] --job [job]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |
| --job | -j | Name of the job definition ||| Yes |

# Remove
Remove a job task definition

Usage: 
```sh
dotnet occli.dll task remove --project [project] --job [job] --name [name]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |
| --job | -j | Name of the job definition ||| Yes |
| --name | -n | Name of the job task ||| Yes |
| --autoconfirm | -ac | Perform the removal without asking for confirmation || false | No |

# Update
Update a job task definition

Usage: 
```sh
dotnet occli.dll task update --project [project] --job [job] --name [name] --rename [newname] --type [type] --provider [provider] --property [propname]:[propvalue]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |
| --job | -j | Name of the job definition ||| Yes |
| --name | -n | Name of the job task ||| Yes |
| --rename | -rn | New name of the job task ||| No |
| --provider | -prov | Name of the provider for the task ||| No |
| --type | -t | Type of the job task | Clone, Generate, Push, Merge, Build, PublishArtifact, Deploy, DeployDb, Test || No |
| --property [key]:[value] | -prop | Property of the job task || Allowed multiple | No |
| --sequence | -s | The sequence of the task when run by engine ||| No |
