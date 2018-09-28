# Manage plugins

Aside from the built-in catapult provider in each job task definition, you can install another plugin which you can [write yourself](../dev-guides/create-plugin.md), or download from [marketplace](../dev-guides/create-plugin.md#plugin-marketplace).

## Install plugin

To install a plugin, use the `plugin register` command, and specify the path to `plugin.yml` file:
```sh
dotnet PC.dll plugin register --file d:\MyPlugin\plugin.yml
```

## Remove plugin

To uninstall the plugin, use the name of the plugin as the parameter:
```sh
dotnet PC.dll plugin remove --name MyPlugin 

