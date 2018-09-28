# Get
Get a single plugin details

Usage: 
`dotnet pc.dll plugin get --name [name]`

**Options**
* name (mandatory)
    * Name of the plugin
    * Usage: `--name` (alias: `-n`)

# List
List all registered plugins

Usage: 
`dotnet pc.dll plugin list --type [type]`

**Options**
* type
    * Type of the plugins
    * Usage: `--role` (alias: `-r`)
    * Allowed values: `all` | `generator` | `repository` | `build` | `deploy`
    * Default value: `all`

# Register
Remove a plugin

Usage: 
`dotnet pc.dll plugin register --name [name]` 

**Options**
* name (mandatory)
    * Name of the plugin
    * Usage: `--name` (alias: `-n`)

# Remove
Remove a plugin

Usage: 
`dotnet pc.dll plugin remove --name [name]` 

**Options**
* name (mandatory)
    * Name of the plugin
    * Usage: `--name` (alias: `-n`)