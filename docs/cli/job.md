# Job

## Add

Add a new job definition

Usage: 
`dotnet pc.dll job add --project [project] --name [name]`

**Options**
* project (mandatory)
    * Name of the project
    * Usage: `--project` (alias: `-p`)
* name (mandatory)
    * Name of the new job definition
    * Usage: `--name` (alias: `-n`)

# List
List project's job definition

Usage: 
`dotnet pc.dll job list --project [project]`

**Options**
* project (mandatory)
    * Name of the project
    * Usage: `--project` (alias: `-p`)

# Remove
Remove a job definition

Usage: 
`dotnet pc.dll job remove --project [project] --name [name]`

**Options**
* project (mandatory)
    * Name of the project
    * Usage: `--project` (alias: `-p`)
* name (mandatory)
    * Name of the job definition to be removed
    * Usage: `--name` (alias: `-n`)
