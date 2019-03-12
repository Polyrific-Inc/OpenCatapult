# Create a task provider - Part 1

Here we will guide you into creating your own custom task provider. We will create a simple code generator provider that will generate an angular website. We will create the provider using dotnet core framework, though you can also create it using .NET Framework.

> You can find the code in this tutorial in our GitHub repository: https://github.com/Polyrific-Inc/Polyrific.Catapult.TaskProviders.Angular/tree/tutorial-part-1

## Prerequisites
- A code editor. We will use [Visual Studio Code](https://code.visualstudio.com/download) in this example
- [dotnet core sdk version 2.1](https://dotnet.microsoft.com/download/dotnet-core/2.1)
- [angular cli](https://cli.angular.io/). Note that angular cli require [nodejs](https://nodejs.org) to be installed
- Opencatapult instalation to test your provider in action. If you have not already, please follow the [quick start](../home/start.md)
- If you are on a non-windows OS, you'd need to install powershell [here](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-6#powershell-core)

## Create the provider project
Create a new dotnet core console project by using the dotnet cli:
```sh
dotnet new console --name Polyrific.Catapult.TaskProviders.Angular
``` 

Go into your project folder:
```sh
cd Polyrific.Catapult.TaskProviders.Angular
```

Next, you'd need to add the plugin core library that is available on [nuget](https://www.nuget.org/packages/Polyrific.Catapult.TaskProviders.Core/)
```sh
dotnet add package Polyrific.Catapult.TaskProviders.Core --version 1.0.0-beta2-*
```

Angular follows the kebab naming convention for its components. Since the model names that are provided might not follow this convention, we need to convert those name into kebab case. To do this, we can use the [humanizer](https://github.com/Humanizr/Humanizer) library:
```sh
dotnet add package Humanizer.Core
```

## Let's code
Open the project using visual studio code:
```sh
code .
```

### Add plugin.yml
Create a new file inside the project folder named `plugin.yml`. This is the metadata of our task provider. It describe the name of the task provider, the additional configs that can be passed, and any [external services](../user-guides/external-services.md) that it requires.
```yml
name: 'Polyrific.Catapult.TaskProviders.Angular'
type: 'GeneratorProvider'
author: 'Polyrific'
version: '1.0.0'
additional-configs:
  - name: Title
    label: Title
    hint: The website title
    type: string
    is-required: false
    is-secret: false
```

Note that we have an additional configs `Title`. This is an optional config that will be used by our task provider to set the title of our angular website. Though, we'd not use this config until [Part 2](./create-task-provider-2.md).

### Prepare the csproj
First, let's set the language version to c# 7.1. This will allow us to have async main method. Add the following line to the `<PropertyGroup>` section in `Polyrific.Catapult.TaskProviders.Angular.csproj` file:
```xml
<LangVersion>7.1</LangVersion>
```

The `Polyrific.Catapult.TaskProviders.Angular.csproj` file should like this:
```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp2.1</TargetFramework>    
    <LangVersion>7.1</LangVersion>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Humanizer.Core" Version="2.5.16" />
    <PackageReference Include="Polyrific.Catapult.TaskProviders.Core" Version="1.0.0-beta2-*" />
  </ItemGroup>

</Project>
```

### Implement the task provider base class
Let's head up to `Program.cs`. The first thing to do is to inherit one of the task provider base class. Since we're going to create a code generator provider, we should inherit from `CodeGeneratorProvider`. Then we'd need to implement the base constructor and abstract method `Generate`. We'd also need to override the `Name` property, and return the name of our task provider as stated in [plugin.yml](./create-task-provider.md#add-plugin.yml).

```csharp
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Polyrific.Catapult.TaskProviders.Core;

namespace Polyrific.Catapult.TaskProviders.Angular
{
  class Program : CodeGeneratorProvider
  {
    public Program(string[] args) : base(args)
    {
    }

    public override string Name => "Polyrific.Catapult.TaskProviders.Angular";

    static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");
    }

    public override Task<(string outputLocation, Dictionary<string, string> outputValues, string errorMessage)> Generate()
    {
      throw new NotImplementedException();
    }
  }
}


```

Now let's write the main function. This should pretty much the same for all task providers, which is to pass the args to the `Program` constructor then call the `Execute` and `ReturnOutput` methods in the base class.
```csharp
static async Task Main(string[] args)
{
  var app = new Program(args);
  
  var result = await app.Execute();
  app.ReturnOutput(result);
}
```

The next thing is to write the logic of our code generator inside the `Generate` method. Here, we will only extract the configuration  and additional configurations that are passed into the task provider. We'd also need to get the `Config` property in the base class, to determine where our source code will be saved. Our user can enter their prefered location in `Config.OutputLocation` but if they have no preferences, we'd take the `Config.WorkingLocation` instead. 
```csharp
public override async Task<(string outputLocation, Dictionary<string, string> outputValues, string errorMessage)> Generate()
{
  // set the default title to project name
  string projectTitle = ProjectName.Humanize(); 
  if (AdditionalConfigs != null && AdditionalConfigs.ContainsKey("Title") && !string.IsNullOrEmpty(AdditionalConfigs["Title"]))
    projectTitle = AdditionalConfigs["Title"];
  
  // set the output location
  Config.OutputLocation = Config.OutputLocation ?? Config.WorkingLocation;

  // TODO: call the code generator logic

  return (Config.OutputLocation, null, "");     
}
```

Now let's create a private method that will generate a simple angular project. We'd utilize the Angular CLI, and use the `ng new` command to create the project for us. We'd pass the command into powershell. Note that this is only one way to generate the code. If you do not want to use the Angular CLI, you can probably provide some template files, then copy the template files into the `OutputLocation`

```csharp
    private Task GenerateCode(string projectName, string outputLocation)
    {
      // if this code is run in linux/mac, change the "powershell" into "pwsh" and the arguments should be $"-c \"ng new {projectName.Kebaberize()} --skipGit=true --skipInstall=true\""
      var info = new ProcessStartInfo("powershell")
      {
          UseShellExecute = false,
          Arguments = $"ng new {projectName.Kebaberize()} --skipGit=true --skipInstall=true",
          RedirectStandardInput = true,
          RedirectStandardOutput = true,
          RedirectStandardError = true,
          CreateNoWindow = true,
          WorkingDirectory = outputLocation
      };

      using (var process = Process.Start(info))
      {
        process.WaitForExit();
      }

      return Task.CompletedTask;
    }
```

We can then call the method inside the `Generate` method:
```csharp
await GenerateCode(ProjectName, Config.OutputLocation);
``` 

Here's how the `Program.cs` should look now:
```csharp
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Humanizer;
using Polyrific.Catapult.TaskProviders.Core;

namespace Polyrific.Catapult.TaskProviders.Angular
{
  class Program : CodeGeneratorProvider
  {
    public Program(string[] args) : base(args)
    {
    }

    public override string Name => "Polyrific.Catapult.TaskProviders.Angular";

    static async Task Main(string[] args)
    {
      var app = new Program(args);
      
      var result = await app.Execute();
      app.ReturnOutput(result);
    }

    public override async Task<(string outputLocation, Dictionary<string, string> outputValues, string errorMessage)> Generate()
    {
      string projectTitle = ProjectName.Humanize(); // set the default title to project name
      if (AdditionalConfigs != null && AdditionalConfigs.ContainsKey("Title") && !string.IsNullOrEmpty(AdditionalConfigs["Title"]))
          projectTitle = AdditionalConfigs["Title"];
      
      Config.OutputLocation = Config.OutputLocation ?? Config.WorkingLocation;

      await GenerateCode(ProjectName, Config.OutputLocation);

      return (Config.OutputLocation, null, "");        
    }

    private Task GenerateCode(string projectName, string outputLocation)
    {
      // if this code is run in linux/mac, change the "powershell" into "pwsh" and the arguments should be $"-c \"ng new {projectName.Kebaberize()} --skipGit=true --skipInstall=true\""
      var info = new ProcessStartInfo("powershell")
      {
          UseShellExecute = false,
          Arguments = $"ng new {projectName.Kebaberize()} --skipGit=true --skipInstall=true",
          RedirectStandardInput = true,
          RedirectStandardOutput = true,
          RedirectStandardError = true,
          CreateNoWindow = true,
          WorkingDirectory = outputLocation
      };

      using (var process = Process.Start(info))
      {
        process.WaitForExit();
      }

      return Task.CompletedTask;
    }
  }
}

```

## Summary

Now our code generator provider technically can be used by opencatapult engine. You can move on to [Part 2](./create-task-provider-2.md), or if you want to immediately see how the code generated, you can jump into [Part 3](./create-task-provider-3.md) to see how to test the task provider
