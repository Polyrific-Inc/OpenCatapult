# Add
Add new external service connection

Usage: 
```sh
dotnet occli.dll service add --name [name] --type [service type] --description [description]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --name | -n | Name of the service connection ||| Yes |
| --type | -t | Type of the service | generic, AzureAppService, GitHub || Yes |
| --description | -d | Description of the service connection ||| No |
| --property [key]:[value] | -prop | Property of the service | allowed multiple || No |

Note:
- you could always use `Generic` type if your service is not supported yet
- the benefit of using supported type (other than `Generic`) is that it will have defined properties and input validation

# Get
Get a single service details

Usage: 
```sh
dotnet occli.dll service get --name [name]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --name | -n | Name of the service connection ||| Yes |

# List
List all registered service connections

Usage: 
```sh
dotnet occli.dll service list
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --type | -t | Type of the service | all, Generic, AzureAppService, GitHub | all | No |

# Remove
Remove an external service

Usage: 
```sh
dotnet occli.dll service remove --name [name]
``` 

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --name | -n | Name of the service connection ||| Yes |
| --autoconfirm | -ac | Perform the removal without asking for confirmation || false | No |

# Update
Update an external service

Usage: 
```sh
dotnet occli.dll service update --name [name] --rename [new name]
``` 

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --name | -n | Name of the service connection ||| Yes |
| --rename | -rn | New name of the service connection ||| No |
| --description | -d | Description of the service connection ||| No |
| --property [key]:[value] | -prop | Property of the service | allowed multiple || No |