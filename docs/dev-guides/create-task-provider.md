# Create a task provider

Here we will guide you into creating your own custom task provider. We will create a simple code generator provider that will generate an asp net core mvc from the `dotnet new` mvc template. We will create the provider using dotnet core framework, though you can also create it using .NET Framework.

## Prerequisites
- A code editor. We will use [Visual Studio Code](https://code.visualstudio.com/download) in this example
- dotnet core sdk version 2.1
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
dotnet add package Polyrific.Catapult.Plugins.Core --version 1.0.0-beta1-15222
```

## Let's code
Open the project using visual studio code:
```sh
code .
```

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
    <PackageReference Include="Polyrific.Catapult.Plugins.Core" Version="1.0.0-beta1-15222" />
  </ItemGroup>

</Project>
```

Let's head up to `Program.cs`. The first thing to do is to inherit one of the task provider base class. Since we're going to create a code generator provider, we should inherit from `CodeGeneratorProvider`

```csharp
using System;
using Polyrific.Catapult.Plugins.Core;

namespace MyCodeGenerator
{
    class Program : CodeGeneratorProvider
    {
        public Program(string[] args) : base(args)
        {
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}

```

## Plugin structure

> TODO: Describe required structure of a plugin

## Type of plugin

> TODO: Describe types of plugins

## Testing the Plugin

> TODO: 

## Plugin marketplace

> \[coming soon\]

