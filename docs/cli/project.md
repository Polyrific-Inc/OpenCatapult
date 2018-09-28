# Archive

Archive a project

Usage: 
```sh
dotnet pc.dll project archive --name [name]
```

**Options**
* name (mandatory)
    * Name of the project to be archived
    * Usage: `--name` (alias: `-n`)

# Clone
Clone a project

Usage: 
```sh
dotnet pc.dll project clone --project [project] --name [name] --includemember --includejob
```

**Options**
* project (mandatory)
    * Name of the project to be cloned
    * Usage: `--project` (alias: `-p`)
* name (mandatory)
    * Name of the new project
    * Usage: `--name` (alias: `-n`)
* includemember
    * Include project members
    * Usage: `--includemember` (alias: `-m`)
* includejob
    * Include job definitions
    * Usage: `--includejob` (alias: `-j`)

# Create
Create a project

Usage: 
```sh
dotnet pc.dll project create --name [name] --client [client] --template [template] --property [propname]:[propvalue]
```

**Options**
* name (mandatory)
    * Name of the new project
    * Usage: `--name` (alias: `-n`)
* client
    * Client of the new project
    * Usage: `--client` (alias: `-c`)
* template
    * Project template name or path to template file (.yaml)
    * Usage: `--template` (alias: `-t`)
* property
    * Project properties
    * Usage: `--property [propname]:[propvalue]` (alias: `-prop`)
    * Allows multiple values

# Update
Update a project

Usage: 
```sh
dotnet pc.dll project update --name [name] --client [client] --property [propname]:[propvalue]
```

**Options**
* name (mandatory)
    * Name of the new project
    * Usage: `--name` (alias: `-n`)
* client
    * Client of the new project
    * Usage: `--client` (alias: `-c`)
* property
    * Project properties
    * Usage: `--property [propname]:[propvalue]` (alias: `-prop`)
    * Allows multiple values

# Export
Export project into a yaml file

Usage: 
```sh
dotnet pc.dll project export --name [name]
```

**Options**
* name (mandatory)
    * Name of the project to be exported
    * Usage: `--name` (alias: `-n`)
* output
    * Output file location
    * Usage: `--output` (alias: `-o`)

# List
List projects which the user has access to

Usage: 
```sh
dotnet pc.dll project list --status [status]
```

**Options**
* status
    * Filter the projects by their status
    * Usage: `--status` (alias: `-s`)
    * Allowed values: `all` | `active` | `archived`
    * Default value: `all`

# Remove
Remove a project

Usage: 
```sh
dotnet pc.dll project remove --name [name]
```

**Options**
* name (mandatory)
    * Name of the project to be removed
    * Usage: `--name` (alias: `-n`)

# restore
Restore an archived project

Usage: 
```sh
dotnet pc.dll project restore --name [name]
```

**Options**
* name (mandatory)
    * Name of the project to be restored
    * Usage: `--name` (alias: `-n`)
