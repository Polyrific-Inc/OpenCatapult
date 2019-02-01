# Create a task provider - Part 4 (Installing the task Pprovider)

For now, there's two step to install the task provider

## Copy the published binary to the plugin folder

If you have run the build script for engine, the plugin folder should be available in
```
.\publish\engine\plugins
```

Let's create a folder where our task provider shall be published into. Since it's a generator provider, it should be under `GeneratorProvider` folder:
```
.\plublish\engine\plugins\GeneratorProvider\Polyrific.Catapult.Plugins.Angular
```

Now get the absolute path to this folder, then open a new shell, and go to our task provider source code project folder . Run the following command to publish our source code into the plugin folder:
```sh
dotnet publish --output "absolute path to .\plublish\engine\plugins\GeneratorProvider\Polyrific.Catapult.Plugins.Angular"
```

## Register the engine in the CLI

Remember earlier we created a `plugin.yml` file? Now is the time to use it. Open the opencatapult cli shell, login, then run this command:
```sh
dotnet occli.dll provider register --file "absolute path to Polyrific.Catapult.Plugins.Angular\plugin.yml"
```

And that's it, you can now create a generate task using the provider:
```sh
dotnet occli.dll task add --name Generate --type Generate --provider Polyrific.Catapult.Plugins.Angular
```

### Summary
That is it for the task provider tutorial. We hope it gives you a general idea on how opencatapult task provider work. If you want to learn more about other task providers, please go to the [task provider reference](../task-providers/task-provider.md)