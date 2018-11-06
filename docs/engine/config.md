# Get
Display configurations

Usage: 
```sh
dotnet ocengine.dll config get --name [name]
```

| Option | Alias | Description | Mandatory |
| --- | --- | --- | --- |
| --name | -n | Name of the configuration item | No |

Note: If the `--name` option is not included, it will display all configuration items.

# Import
Import configuration from a config file

Usage: 
```sh
dotnet ocengine.dll config import --file [file]
```

| Option | Alias | Description | Mandatory |
| --- | --- | --- | --- |
| --file | -f | Path to the config file | Yes |

# Remove
Remove configurations

Usage: 
```sh
dotnet ocengine.dll config remove --name [name]
```

| Option | Alias | Description | Mandatory |
| --- | --- | --- | --- |
| --all | -a | Remove all configuration items | No |
| --name | -n | Name of the configuration item | No |

Note: either `--all` or `--name` option needs to be included in the command

# Set
Set configurations

Usage: 
```sh
dotnet ocengine.dll config set --name [name] --value [value]
``` 

| Option | Alias | Description | Mandatory |
| --- | --- | --- | --- |
| --name | -n | Name of the configuration item | No |
| --value | -v | Value of the configuration item | No |

Note:
- If the `--name` option is not included, it will enter "wizard-mode" which will prompt user with all configuration items to configure
- If the `--name` option is included, the `--value` option needs to be included as well