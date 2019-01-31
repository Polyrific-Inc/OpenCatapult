# Create a task provider

Here we will guide you into creating your own custom task provider. We will create a simple code generator provider that will generate an angular website. We will create the provider using dotnet core framework, though you can also create it using .NET Framework.

## Prerequisites
- A code editor. We will use [Visual Studio Code](https://code.visualstudio.com/download) in this example
- [dotnet core sdk version 2.1](https://dotnet.microsoft.com/download/dotnet-core/2.1)
- [angular cli](https://cli.angular.io/). Note that angular cli require [nodejs](https://nodejs.org) to be installed
- Opencatapult instalation to test your provider in action. If you have not already, please follow the [quick start](../home/start.md)

## Create the provider project
Create a new dotnet core console project by using the dotnet cli:
```sh
dotnet new console --name MyCodeGenerator
``` 

Go into your project folder:
```sh
cd MyCodeGenerator
```

Next, you'd need to add the plugin core library that is available on [nuget](https://www.nuget.org/packages/Polyrific.Catapult.Plugins.Core/)
```sh
dotnet add package Polyrific.Catapult.Plugins.Core --version 1.0.0-beta1-15556
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
name: 'MyCodeGenerator'
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

Note that we have an additional configs `Title`. This is an optional config that will be used by our task provider to set the title of our angular website

### Prepare the csproj
First, let's set the language version to c# 7.1. This will allow us to have async main method. Add the following line to the `<PropertyGroup>` section in `MyCodeGenerator.csproj` file:
```xml
<LangVersion>7.1</LangVersion>
```

The `MyCodeGenerator.csproj` file should like this:
```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp2.1</TargetFramework>    
    <LangVersion>7.1</LangVersion>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Humanizer.Core" Version="2.5.16" />
    <PackageReference Include="Polyrific.Catapult.Plugins.Core" Version="1.0.0-beta1-15556" />
  </ItemGroup>

</Project>
```

### Implement the task provider base class
Let's head up to `Program.cs`. The first thing to do is to inherit one of the task provider base class. Since we're going to create a code generator provider, we should inherit from `CodeGeneratorProvider`. Then we'd need to implement the base constructor and abstract method `Generate`. We'd also need to implement `Name` property, and return the name of our task provider.

```csharp
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Polyrific.Catapult.Plugins.Core;

namespace MyCodeGenerator
{
  class Program : CodeGeneratorProvider
  {
    public Program(string[] args) : base(args)
    {
    }

    public override string Name => "MyCodeGenerator";

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

The next thing is to write the logic of our code generator inside the `Generate` method. Here, we will only extract the configuration  and additional configurations that are passed into the task provider. 
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

Here's how the `Program.cs` should look now:
```csharp
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Humanizer;
using Polyrific.Catapult.Plugins.Core;

namespace MyCodeGenerator
{
  class Program : CodeGeneratorProvider
  {
    public Program(string[] args) : base(args)
    {
    }

    public override string Name => "MyCodeGenerator";

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

      // TODO: call the code generator logic

      return (Config.OutputLocation, null, "");        
    }
  }
}

```

### Create Helper classes
Our `Program` class would get too big if we put all of the code generation logic in there. So it'd be wise to separate some logic into different classes.

First we'd need a helper Class to  run CLI commands, since we'd use Angular CLI in this code generator. Create a folder `Helpers` since we'd have several helper classes, and create a class `CommandHelper.cs`. The CommandHelper static class will have a static method named `ExecuteNodeModule` that execute a node command with arguments.

```csharp
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MyCodeGenerator.Helpers
{
    public static class CommandHelper
    {
        public static async Task<string> ExecuteNodeModule(string command, string workingDirectory, ILogger logger = null)
        {
            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();
            var error = "";

            string fileName;

            // we cannot start the node module in ProcessStartInfo, so we will use powershell
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) 
            {
                fileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "powershell" : command;
            }
            else 
            {
                fileName = "pwsh";
                command = $"-c \"{command}\"";
            }

            var info = new ProcessStartInfo(fileName)
            {
                UseShellExecute = false,
                Arguments = command,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = workingDirectory
            };

            using (var process = Process.Start(info))
            {
                if (process != null)
                {
                    var reader = process.StandardOutput;
                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();

                        logger?.LogDebug(line);

                        outputBuilder.AppendLine(line);
                    }

                    var errorReader = process.StandardError;
                    while (!errorReader.EndOfStream)
                    {
                        var line = await errorReader.ReadLineAsync();

                        if (line.StartsWith("npm WARN"))
                        {
                            logger?.LogWarning(line);
                        }
                        else if (!string.IsNullOrEmpty(line))
                        {
                            errorBuilder.AppendLine(line);
                        }
                    }

                    error = errorBuilder.ToString();
                }
            }

            if (!string.IsNullOrEmpty(error))
                throw new Exception(error);

            return outputBuilder.ToString();
        }
    }
}

```
Aside from running the command and returning the result, it would also log any output of the command into the logger class.

Now we can create the class that will do most of the heavy lifting, we shall create a class called `CodeGenerator`, that exposes 1 public method, `Generate`. 
```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.Extensions.Logging;
using MyCodeGenerator.Helpers;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;

namespace MyCodeGenerator
{
    public class CodeGenerator
    {
        private readonly ILogger _logger;

        private static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public CodeGenerator(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<string> Generate(string projectName, string projectTitle, string outputLocation, List<ProjectDataModelDto> models)
        {
            try
            {
                // clean project name form space
                projectName = projectName.Replace(" ", "").Kebaberize();
                var projectFolder = Path.Combine(outputLocation, projectName);

                // 1. Generate the project
                await CreateAngularProject(projectName, outputLocation);
                await InitializeProject(projectFolder);

                // 2. Generate each model files
                await CreateHomeComponent(projectFolder, projectTitle, models);
                foreach (var model in models)
                {
                    await CreateModelRelatedFile(projectFolder, model);
                }

                return "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return ex.Message;
            }
        }

        private async Task CreateAngularProject(string projectName, string outputLocation)
        {
            await CommandHelper.ExecuteNodeModule($"ng new {projectName} --routing=true --skipGit=true", outputLocation, _logger);
        }

        private async Task InitializeProject(string projectFolder)
        {
            // install angular material to project
            await CommandHelper.ExecuteNodeModule($"ng add @angular/material", projectFolder, _logger);
        }

        private async Task CreateHomeComponent(string projectFolder, string projectTitle, List<ProjectDataModelDto> models)
        {
            var appFolder = Path.Combine(projectFolder, "src/app");
            
            if (File.Exists(Path.Combine(appFolder, "app.component.ts")))
            {
                var content = await LoadFile(Path.Combine(AssemblyDirectory, "Template/app", "app.component.ts"));
                content = content.Replace("$Title$", projectTitle);
                await File.WriteAllTextAsync(Path.Combine(appFolder, "app.component.ts"), content);
            }
            
            if (File.Exists(Path.Combine(appFolder, "app.component.css")))
            {
                var content = await LoadFile(Path.Combine(AssemblyDirectory, "Template/app", "app.component.css"));
                await File.WriteAllTextAsync(Path.Combine(appFolder, "app.component.css"), content);
            }

            if (File.Exists(Path.Combine(appFolder, "app.component.html")))
            {
                var content = await LoadFile(Path.Combine(AssemblyDirectory, "Template/app", "app.component.html"));

                var sb = new StringBuilder();
                foreach (var model in models)
                {
                    sb.AppendLine($"<a mat-list-item routerLink=\"/{model.Name.Kebaberize()}\">{model.Label}</a>");
                }
                content = content.Replace("$navlist$", sb.ToString());

                await File.WriteAllTextAsync(Path.Combine(appFolder, "app.component.html"), content);
            }
            
            
            if (File.Exists(Path.Combine(appFolder, "app.module.ts")))
            {
                var content = await LoadFile(Path.Combine(AssemblyDirectory, "Template/app", "app.module.ts"));
                await File.WriteAllTextAsync(Path.Combine(appFolder, "app.module.ts"), content);
            }

            if (File.Exists(Path.Combine(appFolder, "app-routing.module.ts")))
            {
                var content = await LoadFile(Path.Combine(AssemblyDirectory, "Template/app", "app-routing.module.ts"));

                var sb = new StringBuilder();
                foreach (var model in models)
                {
                    var modelName = model.Name.Kebaberize();
                    sb.AppendLine($"import {{ {model.Name}Component }} from './{modelName}/{modelName}.component';");
                }
                
                content = content.Replace("$ImportComponents$", sb.ToString());
                
                sb = new StringBuilder();
                foreach (var model in models)
                {
                    var modelName = model.Name.Kebaberize();
                    sb.AppendLine($"{{path: '{modelName}', component: {model.Name}Component }},");
                }
                
                content = content.Replace("$RouteComponents$", sb.ToString());

                await File.WriteAllTextAsync(Path.Combine(appFolder, "app-routing.module.ts"), content);
            }

            
            await CommandHelper.ExecuteNodeModule($"ng generate component home", projectFolder, _logger);
            
            if (File.Exists(Path.Combine(appFolder, "home", "home.component.html")))
            {
                var content = await LoadFile(Path.Combine(AssemblyDirectory, "Template/app/home", "home.component.html"));
                await File.WriteAllTextAsync(Path.Combine(appFolder, "home", "home.component.html"), content);
            }
        }

        private async Task CreateModelRelatedFile(string projectFolder, ProjectDataModelDto model)
        {
            await CreateModelComponent(projectFolder, model);

            await CreateModelDataSource(projectFolder, model);
        }

        private async Task CreateModelComponent(string projectFolder, ProjectDataModelDto model)
        {
            var modelName = model.Name.Kebaberize();
            await CommandHelper.ExecuteNodeModule($"ng generate component {modelName}", projectFolder, _logger);

            string componentFolder = Path.Combine(projectFolder, "src/app", modelName);
            if (File.Exists(Path.Combine(componentFolder, $"{modelName}.component.ts")))
            {
                var content = await LoadFile(Path.Combine(AssemblyDirectory, "Template/app/model", "model.component.ts"));

                content = content.Replace("$ModelName$", modelName);
                content = content.Replace("$ModelClassName$", model.Name);
                
                var sb = new StringBuilder();
                foreach (var property in model.Properties)
                {
                    var propertyName = property.Name.Camelize();
                    sb.Append($"'{propertyName}', ");
                }

                content = content.Replace("$PropertyList$", sb.ToString());

                await File.WriteAllTextAsync(Path.Combine(componentFolder, $"{modelName}.component.ts"), content);
            }
            
            if (File.Exists(Path.Combine(componentFolder, $"{modelName}.component.css")))
            {
                var content = await LoadFile(Path.Combine(AssemblyDirectory, "Template/app/model", "model.component.css"));
                await File.WriteAllTextAsync(Path.Combine(componentFolder, $"{modelName}.component.css"), content);
            }

            if (File.Exists(Path.Combine(componentFolder, $"{modelName}.component.html")))
            {
                var content = await LoadFile(Path.Combine(AssemblyDirectory, "Template/app/model", "model.component.html"));

                var sb = new StringBuilder();
                foreach (var property in model.Properties)
                {
                    var propertyName = property.Name.Camelize();
                    sb.AppendLine($"<!-- {property.Name} column -->");
                    sb.AppendLine($"<ng-container matColumnDef=\"{propertyName}\">");
                    sb.AppendLine($"<th mat-header-cell *matHeaderCellDef mat-sort-header>{property.Label}</th>");
                    sb.AppendLine($"<td mat-cell *matCellDef=\"let row\">{{{{row.{propertyName}}}}}</td>");
                    sb.AppendLine("</ng-container>");
                }
                content = content.Replace("$ColumnDefinition$", sb.ToString());

                await File.WriteAllTextAsync(Path.Combine(componentFolder, $"{modelName}.component.html"), content);
            }
        }

        private async Task CreateModelDataSource(string projectFolder, ProjectDataModelDto model)
        {
            var modelName = model.Name.Kebaberize();
            var componentFolder = Path.Combine(projectFolder, "src/app", modelName);
            var content = await LoadFile(Path.Combine(AssemblyDirectory, "Template/app/model", "model-datasource.ts"));

            var sb = new StringBuilder();
            foreach (var property in model.Properties)
            {
                var propertyName = property.Name.Camelize();

                string propertyType;
                switch (property.DataType)
                {
                    case PropertyDataType.String:
                        propertyType = "string";
                        break;
                    case PropertyDataType.Integer:
                    case PropertyDataType.Short:
                    case PropertyDataType.Float:
                    case PropertyDataType.Decimal:
                    case PropertyDataType.Double:
                        propertyType = "number";
                        break;
                    case PropertyDataType.Boolean:
                        propertyType = "boolean";
                        break;
                    default:
                        propertyType = "any";
                        break;
                }
                sb.AppendLine($"{propertyName}: {propertyType};");
            }
            
            content = content.Replace("$ModelDefinition$", sb.ToString());
            
            sb = new StringBuilder();
            for (var i = 0; i < 10; i++)
            {
                sb.Append("{");
                foreach (var property in model.Properties)
                {
                    var propertyName = property.Name.Camelize();
                    sb.Append($"{propertyName}: {GetRandomData(property.DataType)}, ");
                }

                sb.Append("},");
                sb.AppendLine();
            }
            
            content = content.Replace("$ModelDummyData$", sb.ToString());

            sb = new StringBuilder();
            sb.AppendLine("switch (this.sort.active) {");
            foreach (var property in model.Properties)
            {
                var propertyName = property.Name.Camelize();
                switch (property.DataType)
                {
                    case PropertyDataType.Boolean:
                    case PropertyDataType.String:
                        sb.AppendLine($"case '{propertyName}': return compare(a.{propertyName}, b.{propertyName}, isAsc);");
                        break;
                    case PropertyDataType.Integer:
                    case PropertyDataType.Short:
                    case PropertyDataType.Float:
                    case PropertyDataType.Decimal:
                    case PropertyDataType.Double:
                        sb.AppendLine($"case '{propertyName}': return compare(+a.{propertyName}, +b.{propertyName}, isAsc);");
                        break;
                    default:
                        break;
                }
            }
            sb.AppendLine("default: return 0;");
            sb.AppendLine("}");
            content = content.Replace("$ModelSort$", sb.ToString());
            
            content = content.Replace("$ModelName$", model.Name);

            await File.WriteAllTextAsync(Path.Combine(componentFolder, $"{modelName}-datasource.ts"), content);
        }

        private async Task<string> LoadFile(string filePath)
        {
            var content = await File.ReadAllTextAsync(filePath);

            content = content.Replace("// @ts-ignore", "");

            return content;
        }

        private string GetRandomData(string propertyType)
        {            
            var rand = new Random();
            switch (propertyType)
            {
                case PropertyDataType.Integer:
                case PropertyDataType.Short:
                case PropertyDataType.Float:
                case PropertyDataType.Decimal:
                case PropertyDataType.Double:
                    return rand.Next(10).ToString();
                case PropertyDataType.Boolean:
                    return (rand.NextDouble() >= 0.5) ? "true" : "false";
                default:
                    return $"\"dummy {rand.Next(10)}\"";
            }
        }
    }
}
```

The code above basically do four things:
- Create the angular project using `ng new`
- Add Material UI library using `ng add`
- Add angular components for each model using `ng generate component`
- Modify the generated components based on the model's property

For the last part, we'd need to add some template files in our project. Please download the following [zip file ](../file/Template.zip), and put it into your project. The folder structure should like this:
![Project structure](../img/provider-project.jpg)

The last thing is to call the `CodeGenerator` in the `Program.cs`:
Here's how the `Program.cs` should look now:
```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Humanizer;
using Polyrific.Catapult.Plugins.Core;

namespace MyCodeGenerator
{
    class Program : CodeGeneratorProvider
    {
        private readonly CodeGenerator _codeGenerator;

        public Program(string[] args) : base(args)
        {
            _codeGenerator = new CodeGenerator(Logger);
        }

        public override string Name => "MyCodeGenerator";

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

            var error = await _codeGenerator.Generate(ProjectName, projectTitle, Config.OutputLocation, Models);

            if (!string.IsNullOrEmpty(error))
                return ("", null, error);

            return (Config.OutputLocation, null, "");        
        }
    }
}

```

Now we're ready to test our task provider

## Testing the Task Provider

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
        "program": "${workspaceFolder}/bin/Debug/netcoreapp2.1/MyCodeGenerator.dll",
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
ng serve --open
```

## Installing the Task Provider

For now, there's two step to install the task provider

### Copy the published binary to the plugin folder

If you have run the build script for engine, the plugin folder should be available in
```
.\publish\engine\plugins
```

Let's create a folder where our task provider shall be published into. Since it's a generator provider, it should be under `GeneratorProvider` folder:
```
.\plublish\engine\plugins\GeneratorProvider\MyCodeGenerator
```

Now get the absolute path to this folder, then open a new shell, and go to our task provider source code project folder . Run the following command to publish our source code into the plugin folder:
```sh
dotnet publish --output "absolute path to .\plublish\engine\plugins\GeneratorProvider\MyCodeGenerator"
```

### Register the engine in the CLI

Remember earlier we created a `plugin.yml` file? Now is the time to use it. Open the opencatapult cli shell, login, then run this command:
```sh
dotnet occli.dll provider register --file "absolute path to MyCodeGenerator\plugin.yml"
```

And that's it, you can now create a generate task using the provider:
```sh
dotnet occli.dll task add --name Generate --type Generate --provider MyCodeGenerator
```

## Task Provider structure

> TODO: Describe required structure of a task provider

## Type of Task Provider

> TODO: Describe types of task providers

## Task Provider marketplace

> \[coming soon\]

