# Get
Get a single plugin details

Usage: 
```sh
dotnet occli.dll plugin get --name [name]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --name | -n | Name of the plugin ||| Yes |

# List
List all registered plugins

Usage: 
```sh
dotnet occli.dll plugin list --type [type]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --type | -t | Type of the plugins | all, BuildProvider, DatabaseProvider, GeneratorProvider, HostingProvider, RepositoryProvider, StorageProvider, TestProvider | all | No |

# Register
Register a plugin

Usage: 
```sh
dotnet occli.dll plugin register --file [path to plugin.yml file]
``` 

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --file | -f | Path to the `plugin.yml` file ||| Yes |

# Remove
Remove a plugin

Usage: 
```sh
dotnet occli.dll plugin remove --name [name]
``` 

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --name | -n | Name of the plugin ||| Yes |
| --autoconfirm | -ac | Perform the removal without asking for confirmation || false | No |