# Add
Add new external service

Usage: 
`dotnet pc.dll service add --name [name] --property [propname]:[propvalue]`

**Options**
* name (mandatory)
    * Name of the service
    * Usage: `--name` (alias: `-n`)
* property
    * Project properties
    * Usage: `--property [propname]:[propvalue]` (alias: `-prop`)
    * Allows multiple values

# Get
Get a single service details

Usage: 
`dotnet pc.dll service get --name [name]`

**Options**
* name (mandatory)
    * Name of the external service
    * Usage: `--name` (alias: `-n`)

# List
List all registered services

Usage: 
`dotnet pc.dll service list`

# Remove
Remove an external service

Usage: 
`dotnet pc.dll service remove --name [name]` 

**Options**
* name (mandatory)
    * Name of the external service
    * Usage: `--name` (alias: `-n`)

# Update
Update an external service

Usage: 
`dotnet pc.dll service update --name [name] --property [propname]:[propvalue]` 

**Options**
* name (mandatory)
    * Name of the external service
    * Usage: `--name` (alias: `-n`)
* property
    * Project properties
    * Usage: `--property [propname]:[propvalue]` (alias: `-prop`)
    * Allows multiple values