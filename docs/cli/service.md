# Add
Add new external service

Usage: 
```sh
dotnet pc.dll service add --name [name] --type [service type] --description [description]
```

**Options**
* name (mandatory)
    * Name of the service
    * Usage: `--name` (alias: `-n`)
* type (mandatory)
    * Type of the external service
    * Usage: `--type` (alias: `-t`)
* description
    * Description of the external service
    * Usage: `--description` (alias: `-d`)

# Get
Get a single service details

Usage: 
```sh
dotnet pc.dll service get --name [name]
```

**Options**
* name (mandatory)
    * Name of the external service
    * Usage: `--name` (alias: `-n`)

# List
List all registered services

Usage: 
```sh
dotnet pc.dll service list
```

# Remove
Remove an external service

Usage: 
```sh
dotnet pc.dll service remove --name [name]
``` 

**Options**
* name (mandatory)
    * Name of the external service
    * Usage: `--name` (alias: `-n`)

# Update
Update an external service

Usage: 
```sh
dotnet pc.dll service update --name [name] --rename [new name]
``` 

**Options**
* name (mandatory)
    * Name of the external service
    * Usage: `--name` (alias: `-n`)
* rename
    * New name of the external service