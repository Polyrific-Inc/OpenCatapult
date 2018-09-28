# Add

Add user as a project member

Usage: 
```sh
dotnet pc.dll member add --project [project] --user [user] --role [role]
```

**Options**
* project (mandatory)
    * Name of the project
    * Usage: `--project` (alias: `-p`)
* user (mandatory)
    * Username (email) of the user
    * Usage: `--user` (alias: `-u`)
* role
    * Role of the project member
    * Usage: `--role` (alias: `-r`)
    * Allowed values: `member` | `contributor` | `maintainer` | `owner`
    * Default value: `member`

# List
List members of the project

Usage: 
```sh
dotnet pc.dll member list --project [project]
```

**Options**
* project (mandatory)
    * Name of the project
    * Usage: `--project` (alias: `-p`)
* role
    * Role of the project member
    * Usage: `--role` (alias: `-r`)
    * Allowed values: `all` | `member` | `contributor` | `maintainer` | `owner`
    * Default value: `all`

# Remove
Remove a project member

Usage: 
```sh
dotnet pc.dll member remove --project [project] --user [user]
```

**Options**
* project (mandatory)
    * Name of the project
    * Usage: `--project` (alias: `-p`)
* user (mandatory)
    * Username (email) of the user
    * Usage: `--user` (alias: `-u`)

# Update
Update the role of a project member

Usage: 
```sh
dotnet pc.dll member update --project [project] --user [user] --role [role]
``` 

**Options**
* project (mandatory)
    * Name of the project
    * Usage: `--project` (alias: `-p`)
* user (mandatory)
    * Username (email) of the user
    * Usage: `--user` (alias: `-u`)
* role
    * Role of the project member
    * Usage: `--role` (alias: `-r`)
    * Allowed values: `member` | `contributor` | `maintainer` | `owner`
