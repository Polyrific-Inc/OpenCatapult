# Create a simple task provider

Here we will guide you into creating your own custom task provider. We will create a simple code generator provider that will generate a simple .Net Core Web. We will create the provider using dotnet core framework, though you can also create it using .NET Framework.

## Prerequisites
- A code editor. We will use [Visual Studio Code](https://code.visualstudio.com/download) in this example
- [dotnet core sdk version 2.1](https://dotnet.microsoft.com/download/dotnet-core/2.1)
- If you are on a non-windows OS, you'd need to install powershell [here](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-6#powershell-core)

## Create the provider project
Create a new dotnet core console project by using the dotnet cli:
```sh
dotnet new console --name Polyrific.Catapult.SimpleTaskProviders
``` 

Go into your project folder:
```sh
cd Polyrific.Catapult.SimpleTaskProviders
```

Next, you'd need to add the plugin core library that is available on [nuget](https://www.nuget.org/packages/Polyrific.Catapult.TaskProviders.Core/)
```sh
dotnet add package Polyrific.Catapult.TaskProviders.Core --version 1.0.0-beta2-*
```

## Let's code
Open the project using visual studio code:
```sh
code .
```

### Add plugin.yml
Create a new file inside the project folder named `plugin.yml`. This is the metadata of our task provider. It describe the name of the task provider, the additional configs that can be passed, and any [external services](../user-guides/external-services.md) that it requires.
```yml
name: 'Polyrific.Catapult.SimpleTaskProviders'
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

### Prepare the csproj
First, let's set the language version to c# 7.1. This will allow us to have async main method. Add the following line to the `<PropertyGroup>` section in `Polyrific.Catapult.SimpleTaskProviders.csproj` file:
```xml
<LangVersion>7.1</LangVersion>
```

The `Polyrific.Catapult.SimpleTaskProviders.csproj` file should like this:
```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp2.1</TargetFramework>    
    <LangVersion>7.1</LangVersion>
  </PropertyGroup>

  <ItemGroup>
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

namespace Polyrific.Catapult.SimpleTaskProviders
{
  class Program : CodeGeneratorProvider
  {
    public Program(string[] args) : base(args)
    {
    }

    public override string Name => "Polyrific.Catapult.SimpleTaskProviders";

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
  string projectTitle = string.Empty(); 
  if (AdditionalConfigs != null && AdditionalConfigs.ContainsKey("Title") && !string.IsNullOrEmpty(AdditionalConfigs["Title"]))
    projectTitle = AdditionalConfigs["Title"];
  
  // set the output location
  Config.OutputLocation = Config.OutputLocation ?? Config.WorkingLocation;

  // TODO: call the code generator logic


   // set content. This is optional, you don't have to implement this section
   if(File.Exists(Path.Combine(Config.OutputLocation,ProjectName,$"Startup.cs"))){
                
        var content = await LoadFile(Path.Combine(Config.OutputLocation,ProjectName,$"Startup.cs"));
        content = content.Replace("Hello World!", "Hello World From Catapult Task Provider!");
        await File.WriteAllTextAsync(Path.Combine(Config.OutputLocation,ProjectName,$"Startup.cs"), content);
    }

  return (Config.OutputLocation, null, "");     
}
```

Now let's create a private method that will generate a simple .Net Web App project. We'd utilize the dotnet CLI, and use the `dotnet new` command to create the project for us. We'd pass the command into powershell. Note that this is only one way to generate the code. 

```csharp
    private Task GenerateCode(string projectName, string outputLocation)
    {
      // if this code is run in linux/mac, change the "powershell" into "pwsh" and the arguments should be $"-c \"dotnet new web --name {projectName}\""
      var info = new ProcessStartInfo("powershell")
      {
          UseShellExecute = false,
          Arguments = $"dotnet new web --name {projectName}",
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

namespace Polyrific.Catapult.SimpleTaskProvider
{
    class Program : CodeGeneratorProvider
    {
        public Program(string[] args) :base(args){

        }

        public override string Name => "Polyrific.Catapult.SimpleTaskProvider";

        static async Task Main(string[] args)
        {
            var app = new Program(args);

            var result = await app.Execute();
            app.ReturnOutput(result);
        }

        public override async System.Threading.Tasks.Task<(string outputLocation, Dictionary<string, string> outputValues, string errorMessage)> Generate()
        {
                        // set the default title to project name
            string projectTitle = string.Empty; 
            if (AdditionalConfigs != null && AdditionalConfigs.ContainsKey("Title") && !string.IsNullOrEmpty(AdditionalConfigs["Title"]))
                projectTitle = AdditionalConfigs["Title"];

            // set the output location
            Config.OutputLocation = Config.OutputLocation ?? Config.WorkingLocation;

            // TODO: call the code generator logic
            await GenerateCode(ProjectName, Config.OutputLocation);


            if(File.Exists(Path.Combine(Config.OutputLocation,ProjectName,$"Startup.cs"))){
                
                var content = await LoadFile(Path.Combine(Config.OutputLocation,ProjectName,$"Startup.cs"));
                content = content.Replace("Hello World!", "Hello World From Catapult Task Provider!");
                await File.WriteAllTextAsync(Path.Combine(Config.OutputLocation,ProjectName,$"Startup.cs"), content);
            }

            return (Config.OutputLocation, null, "");     
        }

        private Task GenerateCode(string projectName, string outputLocation)
        {
            // if this code is run in linux/mac, change the "powershell" into "pwsh" and the arguments should be $"-c \"dotnet new mvc --name {projectName}\""
            var info = new ProcessStartInfo("powershell")
            {
                UseShellExecute = false,
                Arguments = $"dotnet new web --name {projectName}",
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

        private async Task<string> LoadFile(string filePath)
        {
            var content = await File.ReadAllTextAsync(filePath);

            content = content.Replace("// @ts-ignore", "");

            return content;
        }
    }
}

```

When catapult engine execute our task provider, it passes several arguments that is required by the base task provider class, along with the addditional configs as json string. During testing, we can pass the `--file` option instead, and specify the path to the json file containing the arguments. Since we're going to run a Code Generator Provider, you can use this template file:
- [CodeGeneratorProviderTest.json](../file/CodeGeneratorProviderTest.json)

After downloading the file, Please change the `config -> WorkingLocation` to an absolute path folder in your machine. This folder will be the location that is used by our code generator to create the code for us. Here you can modify the Additional config `title`, or add/modify the `models`.

Next, We'd utilize the visual studio code so we can run and/or debug our application. Create a folder `.vscode` in the project folder, and create these two files:

>launch.json
```json
{       
    "configurations": [
        {
        "name": "Launch",
        "type": "coreclr",
        "request": "launch",
        "program": "${workspaceFolder}/bin/Debug/netcoreapp2.1/Polyrific.Catapult.SimpleTaskProviders.dll",
        "preLaunchTask": "build",
        "args": "--file \"absolute\\path\\to\\CodeGeneratorProviderTest.json\""
    }]
}
```
Please don't forget the change the `args` section so it points to the correct `CodeGeneratorProviderTest.json` location.

>tasks.json
```json
{
    "version": "2.0.0",
    "command": "dotnet",
    "type": "shell",
    "args": [],
    "options":  {
        "cwd": "${workspaceRoot}/"
    },
    "tasks": [
        {
            "label": "build",
            "args": [ ],
            "group": "build",
            "problemMatcher": "$msCompile"
        }
    ]
}
```

Now we're ready to go. Hit F5, and wait for the code generation to complete. If All goes well, the DEBUG CONSOLE will have the following output:
```sh
[OUTPUT] {"outputLocation":"workingfolderpath","outputValues":null,"errorMessage":""}

```

Now open the folder in the `outputLocation` stated above, and your code is ready there. To run the application, you can open your shell, go to the `outputLocation` directory, and run the command
```sh
dotnet run
```