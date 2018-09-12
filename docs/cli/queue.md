# Queue

## Add

Add job to queue

Usage: 
`dotnet pc.dll queue add --project [project] --job [job]`

**Options**
* project (mandatory)
    * Name of the project
    * Usage: `--project` (alias: `-p`)
* job (mandatory)
    * Name of the job definition
    * Usage: `--job` (alias: `-j`)

# List
List queued jobs

Usage: 
`dotnet pc.dll queue list --project [project]`

**Options**
* project (mandatory)
    * Name of the project
    * Usage: `--project` (alias: `-p`)

# Get
Get complete log of a queued job

Usage: 
`dotnet pc.dll queue get --project [project] --number [number]`

**Options**
* project (mandatory)
    * Name of the project
    * Usage: `--project` (alias: `-p`)
* number (mandatory)
    * Queue number
    * Usage: `--number` (alias: `-n`)

# Restart
Restart the pending queue

Usage: 
`dotnet pc.dll queue restart --project [project] --number [number]`

**Options**
* project (mandatory)
    * Name of the project
    * Usage: `--project` (alias: `-p`)
* number (mandatory)
    * Queue number
    * Usage: `--number` (alias: `-n`)
