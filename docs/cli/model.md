# Add

Add a project data model

Usage: 
```sh
dotnet occli.dll model add --project [project] --name [name]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |
| --name | -n | Name of the data model ||| Yes |
| --description | -d | Description of the data model ||| No |
| --label | -l | Label of the data model ||| No |

# Get
Get a single project data model detailed record

Usage: 
```sh
dotnet occli.dll model get --project [project] --name [name]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |
| --name | -n | Name of the data model ||| Yes |

# List
List data models in a project

Usage: 
```sh
dotnet occli.dll model list --project [project]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |

# Remove
Remove a project data model

Usage: 
```sh
dotnet occli.dll model remove --project [project] --name [name]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |
| --name | -n | Name of the data model ||| Yes |
| --autoconfirm | -ac | Perform the removal without asking for confirmation || false | No |

# Update
Update a project data model

Usage: 
```sh
dotnet occli.dll model update --project [project] --name [name] --rename [newname]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |
| --name | -n | Name of the data model ||| Yes |
| --rename | -rn | New name of the data model ||| No |
| --description | -d | Description of the data model ||| No |
| --label | -l | Label of the data model ||| No |