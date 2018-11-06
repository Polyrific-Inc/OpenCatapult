# Get
Display configuration items

Usage: 
```sh
dotnet occli.dll config get --name [name]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --name | -n | Name of the configuration item ||| No |

Note: If `--name` option is not included, it will display all configuration items.

# Import
Import configuration from a config file

Usage: 
```sh
dotnet occli.dll config import --file [file]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --file | -f | Path of the file containing configuration ||| Yes |

Note: The content of the file should be in json format, with the "key:value" of the config items which will be configured. For example:
```json
{
    "ApiUrl": "https://localhost"
    "ApiRequestTimeout": "00:00:30"
}
```

# Remove
Remove configurations

Usage: 
```sh
dotnet occli.dll config remove --name [name]
```

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --all | -a | Remove all configuration items ||| No |
| --name | -n | Name of the configuration item ||| No |
| --autoconfirm | -ac | Perform the removal without asking for confirmation || false | No |

# Set
Set the value of configuration items

Usage: 
```sh
dotnet occli.dll config set --name [name] --value [value]
``` 

| Option | Alias | Description | Allowed Values | DefaultValue | Mandatory |
| --- | --- | --- | --- | --- | --- |
| --name | -n | Name of the configuration item ||| No |
| --value | -v | Value of the configuration item ||| No |

Note: if the `--name` option is not included, it will enter "wizard-mode" which will prompt all configuration items to the user.