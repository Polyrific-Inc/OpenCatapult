[Web UI](projects-web.md) | CLI

# Manage projects

In `OpenCatapult` terminology, Project is a unit of work which your team can collaborate to create an application. The structure of the application itself can be adjusted based on your needs. For example, a project can contain an API, an Admin application, and a front-facing customer application.

## Create project
Create a project by specifying the name and optionally the client of this project
```sh
dotnet occli.dll project create --name MyProject --client Polyrific
```

You can also create a project based on a template to pre-populate the models and jobs, by using the `template` option:
```sh
dotnet occli.dll project create --name MyProject --client Polyrific --template my-previous-project
```

Please find the details of the template content [here](../dev-guides/project-template.md).

Finally, [task providers](../home/intro.md#task-providers) may have some configuration key that you can override for each project. You can set this when creating the project by using the `property` option:
```sh
dotnet occli.dll project create --name MyProject --client Polyrific --property createAdmin:false  
```

For the configuration key that the task provider have, please refer to each task provider's [page](../task-providers/task-provider.md).

All of the created projects can be viewed using the `list` command:
```sh
dotnet occli.dll project list
```

## Update project

You can update the project's client or property by using the `update` command:
```sh
dotnet occli.dll project update --name MyProject --client Polyrific --property createAdmin:false
```

## Remove project

Remove a project by specifying the name of the project to be removed:
```sh
dotnet occli.dll project remove --name MyProject
```

## Archive project

When you want to remove external services of a project, but don't want to remove the project from the database, you can opt to archive it. The archived project can be [restored](#restore-project) later.
```sh
dotnet occli.dll project archive --name MyProject
```

## Restore project

Restore an archived project by specifying the name of the project:
```sh
dotnet occli.dll project restore --name MyProject
```

## Clone project

You can clone an existing project, and create a new one. The new project will have the [models](data-models.md) copied from the cloned project. You can optionally copy the [members](project-members.md) and [jobs](job-definitions.md) by using the option `includemember` and `includejob` respectively:
```sh
dotnet occli.dll project clone --project MyProject --name my-other-project --includemember --includejob
```

## Export project

You can export an existing project into a yaml file, when then can be used as a template when you create a new project. 
```sh
dotnet occli.dll project export --name MyProject
```

The yaml file will be created in the AppData folder. You can also specify a specific location for the template file to be saved:
```sh
dotnet occli.dll project export --name MyProject --output D:/MyProject.yaml
```