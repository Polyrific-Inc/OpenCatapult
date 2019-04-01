# Create a simple task provider under 10 minutes

Here we will guide you into creating your own custom task provider. We will create a simple code generator provider that will generate a simple .Net Core Web. We will create the provider using dotnet core framework, though you can also create it using .NET Framework.

> You can find the code in this tutorial in our GitHub repository: https://github.com/Polyrific-Inc/Polyrific.Catapult.TaskProviders.SimpleGenerator


## Prerequisites
- A code editor. We will use [Visual Studio Code](https://code.visualstudio.com/download) in this example. You can use any code editor that you want even notepad :)
- [dotnet core sdk version 2.1](https://dotnet.microsoft.com/download/dotnet-core/2.1)

## 1. Create the provider project
Create a new dotnet core console project by using the dotnet cli. Open new command line window, set the path to your working folder, and execute the command below:
```sh
dotnet new console --name Polyrific.Catapult.TaskProviders.SimpleGenerator
``` 

Go into your project folder:
```sh
cd Polyrific.Catapult.TaskProviders.SimpleGenerator
```

Next, you'd need to add the plugin core library that is available on [nuget](https://www.nuget.org/packages/Polyrific.Catapult.TaskProviders.Core/)
```sh
dotnet add package Polyrific.Catapult.TaskProviders.Core --version 1.0.0-beta2-*
```

## 2. Let's code
Open new command line window, set the path to your working folder, and execute the command below to open Visual Studio Code:
```sh
code .
```

### Add plugin.yml
Create a new file inside the project folder named `plugin.yml`. This is the metadata of our task provider. It describe the name of the task provider, the additional configs that can be passed, and any [external services](../user-guides/external-services.md) that it requires.

```yml
name: 'Polyrific.Catapult.TaskProviders.SimpleGenerator'
type: 'GeneratorProvider'
author: 'Polyrific'
version: '1.0.0'
```

### Implement the task provider base class
Let's head up to `Program.cs`. The first thing to do is to inherit one of the task provider base class. Since we're going to create a code generator provider, we should inherit from `CodeGeneratorProvider`. Then we'd need to implement the base constructor and abstract method `Generate`.

```csharp
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Polyrific.Catapult.TaskProviders.Core;

namespace Polyrific.Catapult.TaskProviders.SimpleGenerator
{
  class Program : CodeGeneratorProvider
  {
    public Program(string[] args) : base(args)
    {
    }

    public override string Name => "Polyrific.Catapult.TaskProviders.SimpleGenerator";

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
static void Main(string[] args)
{
    var app = new Program(args);
    var result = app.Execute().Result;
    Console.Write(result);
}
```

The next thing is to write the logic of our code generator inside the `Generate` method.
```csharp
public override async Task<(string outputLocation, Dictionary<string, string> outputValues, string errorMessage)> Generate()
{ 
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
      var info = new ProcessStartInfo("dotnet")
      {
          UseShellExecute = false,
          Arguments = $"new web --name {projectName}",
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
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Polyrific.Catapult.TaskProviders.Core;

namespace Polyrific.Catapult.TaskProviders.SimpleGenerator
{
    class Program : CodeGeneratorProvider
    {
      
        public Program(string[] args) :base(args){

        }

        public override string Name => "Polyrific.Catapult.TaskProviders.SimpleGenerator";

        static void Main(string[] args)
        {
            var app = new Program(args);
            var result = app.Execute().Result;
            Console.Write(result);
        }

        public override async System.Threading.Tasks.Task<(string outputLocation, Dictionary<string, string> outputValues, string errorMessage)> Generate()
        {                       
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
            var info = new ProcessStartInfo("dotnet")
            {
                UseShellExecute = false,
                Arguments = $"new web --name {projectName}",
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


## 3. Let's Run
Now we're ready to go. Open new command line window and set the path to your project folder. Next, run the command below
```sh
dotnet run -- --file CodeGeneratorProviderTest.json
```

If All goes well, the DEBUG CONSOLE will have the following output:
```sh
[OUTPUT] {"outputLocation":"workingfolderpath","outputValues":null,"errorMessage":""}

```

Now open the folder in the `outputLocation` stated above, and your code is ready there. To run the application, you can open your command line window, go to the `outputLocation` directory, and run the command
```sh
dotnet run -o
```

Your custom task provider now is ready to be installed, please follow the link [here](https://docs.opencatapult.net/guides/dev-guides/create-task-provider-4) for detail.
