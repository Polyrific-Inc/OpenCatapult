# Archive

Archive a project

Usage: 
```sh
dotnet occli.dll project archive --name [name]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --name | -n | Name of the active project ||| Yes |

# Clone
Clone a project

Usage: 
```sh
dotnet occli.dll project clone --project [project] --name [name] --includemember --includejob
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project to be cloned ||| Yes |
| --name | -n | Name of the new project ||| Yes |
| --includemember | -m | Include project members || false | No |
| --includejob | -j | Include job definitions || false | No |

# Create
Create a project

Usage: 
```sh
dotnet occli.dll project create --name [name] --client [client] --template [template] --property [propname]:[propvalue]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --name | -n | Name of the new project ||| Yes |
| --client | -c | Client of the project ||| No |
| --template | -t | Project template name or path to template file (.yaml) ||| No |
| --property [name]:[value] | -prop | Project properties | allowed multiple || No |

# Get
Get a single project detailed record

Usage: 
```sh
dotnet occli.dll project get --name [name]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --name | -n | Name of the project ||| Yes |

# Update
Update a project

Usage: 
```sh
dotnet occli.dll project update --name [name] --client [client] --property [propname]:[propvalue]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --name | -n | Name of the project ||| Yes |
| --client | -c | Client of the project ||| No |
| --property [name]:[value] | -prop | Project properties | allowed multiple || No |

# Export
Export project into a yaml file

Usage: 
```sh
dotnet occli.dll project export --name [name]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --name | -n | Name of the project ||| Yes |
| --output | -o | Location of the output file || /Templates/{project}.yaml | No |

# List
List projects which the user has access to

Usage: 
```sh
dotnet occli.dll project list --status [status] --all
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --status | -s | Filter the projects by their status | all, active, archived | all | No |
| --all | -a | Retrieve all projects including the ones the current user is not the member (for administrator only) || false | No |

# Remove
Remove a project

Usage: 
```sh
dotnet occli.dll project remove --name [name]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --name | -n | Name of the project ||| Yes |
| --autoconfirm | -ac | Perform the removal without asking for confirmation || false | No |

# Restore
Restore an archived project

Usage: 
```sh
dotnet occli.dll project restore --name [name]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --name | -n | Name of the archived project ||| Yes |
