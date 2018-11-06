# Activate

Activate a suspended engine

Usage: 
```sh
dotnet occli.dll engine activate --name [name]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --name | -n | Name of the suspended engine ||| Yes |

# Get
Get a single engine record

Usage: 
```sh
dotnet occli.dll engine get --name [name]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --name | -n | Name of the engine ||| Yes |
    
# List
List registered engines

Usage: 
```sh
dotnet occli.dll engine list --status [status]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --status | -s | Filter the engines by their status | all, active, suspended, running | all | No |

# Register
Register a new engine

Usage: 
```sh
dotnet occli.dll engine register --name [name]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --name | -n | Name of the engine ||| Yes |

# Remove
Remove an engine

Usage: 
```sh
dotnet occli.dll engine remove --name [name]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --name | -n | Name of the engine ||| Yes |
| --autoconfirm | -ac | Perform the removal without asking for confirmation || false | No |

# Suspend
Suspend an engine

Usage: 
```sh
dotnet occli.dll engine suspend --name [name]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --name | -n | Name of the active engine ||| Yes |

# Token
Generate auth token for an engine

Usage: 
```sh
dotnet occli.dll engine token --name [name]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --name | -n | Name of the engine ||| Yes |