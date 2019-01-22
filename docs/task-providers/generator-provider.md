# Generator Provider

Generator providers can be used in [Generate task](../user-guides/job-definitions.md#generate). The main role of this provider is to provide a specific implementation of source code generation based on the project configuration and the defined data models.

## Polyrific.Catapult.Plugins.AspNetCoreMvc

`OpenCatapult` provides `Polyrific.Catapult.Plugins.AspNetCoreMvc` as the built-in provider for Generator Provider. This provider will create an AspNet Core Mvc application. The generated code further explained in the last section.

### Usage

This provider can only be used in Generate task. You can use the name `Polyrific.Catapult.Plugins.AspNetCoreMvc` when adding or updating a Generate task:

```sh
dotnet occli.dll task add -p SampleProject -j Default -n Generate -t Generate -prov Polyrific.Catapult.Plugins.AspNetCoreMvc
```

```sh
dotnet occli.dll task update -p SampleProject -j Default -n Generate -prov Polyrific.Catapult.Plugins.AspNetCoreMvc
```

### Additional configs

This provider has several additional configurations that you can use to fit your use case:

| Name | Description | Default Value | Mandatory |
| --- | --- | --- | --- | --- |
| AdminEmail | The email of the admin user used to login to the application | - | Yes |

### Structures of generated code

The AspNetCoreMvc generator will generate the code with the following structure:

- Main Web Project: The Web UI project. It uses Asp Net Core Mvc as the web framework.
- Business layer project: The core business of the applications reside here.
- Data project: The project that define the structure of physical database. It uses entity framework core fluent API to define the data structure, and generate migration script.
- Infrastructure project: Mainly used to define the dependency injection between Data project and Main Web project, to keep the dependency clean.

<!--TODO: explain more on the template>
