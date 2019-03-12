# Generator Provider

Generator providers can be used in [Generate task](../user-guides/job-definitions.md#generate). The main role of this provider is to provide a specific implementation of source code generation based on the project configuration and the defined data models.

## Polyrific.Catapult.TaskProviders.AspNetCoreMvc

`OpenCatapult` provides `Polyrific.Catapult.TaskProviders.AspNetCoreMvc` as the built-in provider for Generator Provider. This provider will create an AspNet Core Mvc application. The generated code further explained in the last section.

### Usage

This provider can only be used in Generate task. You can use the name `Polyrific.Catapult.TaskProviders.AspNetCoreMvc` when adding or updating a Generate task:

```sh
dotnet occli.dll task add -p SampleProject -j Default -n Generate -t Generate -prov Polyrific.Catapult.TaskProviders.AspNetCoreMvc
```

```sh
dotnet occli.dll task update -p SampleProject -j Default -n Generate -prov Polyrific.Catapult.TaskProviders.AspNetCoreMvc
```

### Additional configs

This provider has several additional configurations that you can use to fit your use case:

| Name | Description | Default Value | Mandatory |
| --- | --- | --- | --- | --- |
| AdminEmail | The email of the admin user used to login to the application | - | Yes |

### Running the generated code
You would require [dotnet core sdk](https://dotnet.microsoft.com/download/dotnet-core/2.1) to run this in your local. Please go to the directory by using the cd command
```sh
cd path/to/solution_folder
```

You would need to modify the connection string in the `appsettings.json` file. If you've installed visual studio code you can run this:
```sh
code .\SampleProject\appsettings.json
```

Add the connection string to the json file, based on your database configuration:
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SampleProject.db;User ID=sa;Password=samprod;"
}
```

Note that to use a localhost database server you would need SQL Server instance installed in your local machine.

After the connection string is set, run the following command to initialize the database:
```sh
dotnet ef database update --startup-project SampleProject --project SampleProject.Data
```

Then, to run the web, use the following command:
```sh
dotnet run --project SampleProject --urls "http://localhost:8006;https://localhost:44306"
```

Finally, you can then open the website in your browser using one of these urls:
- http://localhost:8006
- https://localhost:44306

You can also login into the admin pages using the following default admin:
user: {{AdminEmail}}
password: opencatapult

We highly recommend you to update the password. You can do that in https://localhost:44306/Identity/Account/Manage/ChangePassword

### Structures of generated code

The AspNetCoreMvc generator will generate the code with the following structure:

#### Web Project
This is the web mvc project. It contains the view models, controllers, and views for you to modify the web UI looks. 

The front facing web pages are located in folders:
- Controllers
- Models
- Views

There's also admin web pages which you can find in the `Areas\Admin` folder.

The `AutoMapperProfiles` contains the mapper profile files between your Entities in Core project and your ViewModels.

#### Core Project
This is where the core business logic resides. It contains the entities and logics of your application. It also contains the abstraction for the repository.

#### Data Project
This is Database specific project that implements the repository abstraction from the Core project. It uses Entity Framework Core. Here you can find:
- Repository classes: The class used to retrieve data from the database
- Entity Configs: Defines the physical database configuration you want to override. It defaults to the structure defined in the `Core.Entities`

#### Infrastructure Project
This is a bootstrapper project, to link the Dependency Injection from Data project to Web project. This way, we can have a cleaner dependency map where the Web only depend on the Core project.


### Advanced

#### Setting up email
You can setup an smtp server to have the email functionality runnning. This would be required if you want to enable forgot password feature. To setup the smtp, modify the following section in `appsettings.json`
```json
"SmtpSetting": {
    "Server": "localhost",
    "Port": 0,
    "Username": "username",
    "Password": "password",
    "SenderEmail": "localhost"
}
```
