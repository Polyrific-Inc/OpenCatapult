# Get
Display configurations

Usage: 
`dotnet pc.dll config get --name [name]`

**Options**
* all
    * Display all configuration items
    * Usage: `--all` (alias: `-a`)
* name
    * Name of the configuration item
    * Usage: `--name` (alias: `-n`)

# Import
Import configuration from a config file

Usage: 
`dotnet pc.dll config import --file [file]`

**Options**
* file (mandatory)
    * Full path of the file
    * Usage: `--file` (alias: `-f`)

# Remove
Remove configurations

Usage: 
`dotnet pc.dll config remove --name [name]`

**Options**
* all
    * Display all configuration items
    * Usage: `--all` (alias: `-a`)
* name
    * Name of the configuration item
    * Usage: `--name` (alias: `-n`)

# Set
Set configurations

Usage: 
`dotnet pc.dll config set --name [name] --value [value]` 

**Options**
* all
    * Set all configurations in wizard mode
    * Usage: `--all` (alias: `-a`)
* name
    * Name of the configuration item
    * Usage: `--name` (alias: `-n`)
* value
    * Value of the configuration item
    * Usage: `--value` (alias: `-v`)