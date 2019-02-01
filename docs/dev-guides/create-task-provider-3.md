# Create a task provider - Part 3 (Testing the Task Provider)

> You can find the code in this tutorial in our GitHub repository: https://github.com/Polyrific-Inc/Polyrific.Catapult.Plugins.Angular/tree/tutorial-part-3

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
        "program": "${workspaceFolder}/bin/Debug/netcoreapp2.1/Polyrific.Catapult.Plugins.Angular.dll",
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

## Summary
As a task provider developer, you have now learned how to test your application outside opencatapult. Now the next step is to integrate your task provider into the opencatapult engine, so you can create a task that use your task provider. We will cover this in [Part 4](./create-task-provider-4.md).