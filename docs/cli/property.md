# Add

Add a project data model property

Usage: 
```sh
dotnet occli.dll property add --project [project] --model [model] --name [name] --label [label] --type [type] --controltype [controltype] --relational [relational] --relationaltype [relationaltype]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |
| --model | -m | Name of the data model ||| Yes |
| --name | -n | Name of the data model property ||| Yes |
| --label | -l | Label of the data model property || same as the name| No |
| --type | -t | Data type of the property | string, int, short, float, double, decimal, bool, datetime, byte, guid, dbgeography | string | No |
| --controltype | -ct | Control type of the property | input-text, input-number, input-file, textarea, checkbox, select, radio, calendar, image | input-text | No |
| --relational | -r | Name of the related data model ||| No |
| --relationaltype | -rt | Type of the relationship | one-to-one, one-to-many | one-to-one | No |

# List
List properties in a data model

Usage: 
```sh
dotnet occli.dll property list --project [project] --model [model]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |
| --model | -m | Name of the data model ||| Yes |

# Remove
Remove a project data model property

Usage: 
```sh
dotnet occli.dll property remove --project [project] --model [model] --name [name]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |
| --model | -m | Name of the data model ||| Yes |
| --name | -n | Name of the data model property ||| Yes |
| --autoconfirm | -ac | Perform the removal without asking for confirmation || false | No |

# Update
Update a project data model property

Usage: 
```sh
dotnet occli.dll property update --project [project] --name [name] --rename [newname] --label [label] --type [type] --controltype [controltype] --relational [relational] --relationaltype [relationaltype]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --project | -p | Name of the project ||| Yes |
| --model | -m | Name of the data model ||| Yes |
| --name | -n | Name of the data model property ||| Yes |
| --rename | -rn | New name of the data model property ||| No |
| --label | -l | Label of the data model property ||| No |
| --type | -t | Data type of the property | string, int, short, float, double, decimal, bool, datetime, byte, guid, dbgeography | | No |
| --controltype | -ct | Control type of the property | input-text, input-number, input-file, textarea, checkbox, select, radio, calendar, image || No |
| --relational | -r | Name of the related data model ||| No |
| --relationaltype | -rt | Type of the relationship | one-to-one, one-to-many || No |