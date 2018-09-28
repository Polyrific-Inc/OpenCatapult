# Get
Get a single plugin details

Usage: 
```sh
dotnet pc.dll plugin get --name [name]
```

**Options**
* name (mandatory)
    * Name of the plugin
    * Usage: `--name` (alias: `-n`)

# List
List all registered plugins

Usage: 
```sh
dotnet pc.dll plugin list --type [type]
```

**Options**
* type
    * Type of the plugins
    * Usage: `--role` (alias: `-r`)
    * Allowed values: `all` | `BuildProvider` | `DatabaseProvider` | `GeneratorProvider` | `HostingProvider` | `RepositoryProvider` | `StorageProvider` | `TestProvider`
    * Default value: `all`

# Register
Register a plugin

Usage: 
```sh
dotnet pc.dll plugin register --file [path to plugin.yml file]
``` 

**Options**
* file (mandatory)
    * Name of the plugin
    * Usage: `--file` (alias: `-f`)

# Remove
Remove a plugin

Usage: 
```sh
dotnet pc.dll plugin remove --name [name]
``` 

**Options**
* name (mandatory)
    * Name of the plugin
    * Usage: `--name` (alias: `-n`)