# Add

Add a project data model property

Usage: 
```sh
dotnet pc.dll property add --project [project] --model [model] --name [name] --label [label] --type [type] --controltype [controltype] --relational [relational] --relationaltype [relationaltype]
```

**Options**
* project (mandatory)
    * Name of the project
    * Usage: `--project` (alias: `-p`)
* model (mandatory)
    * Name of the data model
    * Usage: `--model` (alias: `-m`)
* name (mandatory)
    * Name of the data model property
    * Usage: `--name` (alias: `-n`)
* label
    * Label of the data model property
    * Usage: `--label` (alias: `-l`)
* type
    * Data type of the property
    * Usage: `--type` (alias: `-t`)
    * Allowed values: `string` | `int` | `short` | `float` | `double` | `decimal` | `bool` | `datetime` |`byte` | `guid` | `dbgeography`
    * Default value: `string`
* controltype
    * Control type of the property
    * Usage: `--controltype` (alias: `-ct`)
    * Allowed values: `input-text` | `input-number` | `input-file` | `textarea` | `checkbox` | `select` | `radio` | `calendar` |`image`
    * Default value: `input-text`
* relational
    * Name of the related data model
    * Usage: `--relational` (alias: `-r`)
* relationaltype
    * Relationship type
    * Usage: `--relationaltype` (alias: `-rt`)
    * Allowed values: `one-to-one` | `one-to-many` | `many-to-many`
    * Default value: `one-to-one`

# List
List properties in a data model

Usage: 
```sh
dotnet pc.dll property list --project [project] --model [model]
```

**Options**
* project (mandatory)
    * Name of the project
    * Usage: `--project` (alias: `-p`)
* model (mandatory)
    * Name of the data model
    * Usage: `--model` (alias: `-m`)

# Remove
Remove a project data model property

Usage: 
```sh
dotnet pc.dll property remove --project [project] --model [model] --name [name]
```

**Options**
* project (mandatory)
    * Name of the project
    * Usage: `--project` (alias: `-p`)
* model (mandatory)
    * Name of the data model
    * Usage: `--model` (alias: `-m`)
* name (mandatory)
    * Name of the data model property
    * Usage: `--name` (alias: `-n`)

# Update
Update a project data model property

Usage: 
```sh
dotnet pc.dll property update --project [project] --name [name] --rename [newname] --label [label] --type [type] --controltype [controltype] --relational [relational] --relationaltype [relationaltype]
```

**Options**
* project (mandatory)
    * Name of the project
    * Usage: `--project` (alias: `-p`)
* model (mandatory)
    * Name of the data model
    * Usage: `--model` (alias: `-m`)
* name (mandatory)
    * Name of the data model property
    * Usage: `--name` (alias: `-n`)
* rename
    * New name of the data model property
    * Usage: `--rename` (alias: `-r`)
* label
    * Label of the data model property
    * Usage: `--label` (alias: `-l`)
* type
    * Data type of the property
    * Usage: `--type` (alias: `-t`)
    * Allowed values: `string` | `int` | `short` | `float` | `double` | `decimal` | `bool` | `datetime` |`byte` | `guid` | `dbgeography`
* controltype
    * Control type of the property
    * Usage: `--controltype` (alias: `-ct`)
    * Allowed values: `input-text` | `input-number` | `input-file` | `textarea` | `checkbox` | `select` | `radio` | `calendar` |`image`
* relational
    * Name of the related data model
    * Usage: `--relational` (alias: `-r`)
* relationaltype
    * Relationship type
    * Usage: `--relationaltype` (alias: `-rt`)
    * Allowed values: `one-to-one` | `one-to-many` | `many-to-many`
