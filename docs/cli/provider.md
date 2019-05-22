# Get
Get a single task provider details

Usage: 
```sh
dotnet occli.dll provider get --name [name]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --name | -n | Name of the task provider ||| Yes |

# List
List all registered task providers

Usage: 
```sh
dotnet occli.dll provider list --type [type]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --type | -t | Type of the task providers | all, BuildProvider, DatabaseProvider, GeneratorProvider, HostingProvider, RepositoryProvider, StorageProvider, TestProvider | all | No |

# Register
Register a task provider

Usage: 
```sh
dotnet occli.dll provider register --file [path to taskprovider.yml file]
``` 

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --file | -f | Path to the `taskprovider.yml` file ||| Yes |

# Remove
Remove a task provider

Usage: 
```sh
dotnet occli.dll provider remove --name [name]
``` 

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --name | -n | Name of the task provider ||| Yes |
| --autoconfirm | -ac | Perform the removal without asking for confirmation || false | No |