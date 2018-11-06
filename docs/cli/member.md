# Add

Add user as a project member

Usage: 
```sh
dotnet occli.dll member add --project [project] --user [user] --role [role]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |
| --user | -u | Username of the user ||| Yes |
| --role | -r | Role of the project member | member, contributor, maintainer, owner | member | No |

# List
List members of the project

Usage: 
```sh
dotnet occli.dll member list --project [project]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |
| --role | -r | Role of the project member | all, member, contributor, maintainer, owner | all | No |

# Remove
Remove a project member

Usage: 
```sh
dotnet occli.dll member remove --project [project] --user [user]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |
| --user | -u | Username of the user ||| Yes |
| --autoconfirm | -ac | Perform the removal without asking for confirmation || false | No |

# Update
Update the role of a project member

Usage: 
```sh
dotnet occli.dll member update --project [project] --user [user] --role [role]
``` 

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |
| --user | -u | Username of the user ||| Yes |
| --role | -r | Role of the project member | member, contributor, maintainer, owner || Yes |