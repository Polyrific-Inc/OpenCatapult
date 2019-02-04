# Hosting Provider

Hosting providers can be used in [Deploy task](../user-guides/job-definitions.md#deploy). The main role of this provider is to provide a specific way to deploy build artifact into a hosting environment.

## Polyrific.Catapult.Plugins.AzureAppService

`OpenCatapult` provides `Polyrific.Catapult.Plugins.AzureAppService` as the built-in provider for Hosting Provider. This provider can be used to deploy a web application into Azure App Service instance. It uses kudu deploy `zipdeploy` api to upload the package and update the application.

### Usage

This provider can only be used in Deploy task. You can use the name `Polyrific.Catapult.Plugins.AzureAppService` when adding or updating a Deploy task:

```sh
dotnet occli.dll task add -p SampleProject -j Default -n Deploy -t Deploy -prov Polyrific.Catapult.Plugins.AzureAppService
```

```sh
dotnet occli.dll task update -p SampleProject -j Default -n Deploy -prov Polyrific.Catapult.Plugins.AzureAppService
```

### Required services

This provider requires the `Azure` external service to get the connection details. Please refer to the [External Service guideline](../user-guides/external-services.md#AzureAppService) for more info.

### Additional configs

This provider has several additional configurations that you can use to fit your use case:

| Name | Description | Default Value | Mandatory |
| --- | --- | --- | --- | --- |
| SubscriptionId | The subscription id for the app service | -| Yes |
| ResourceGroupName | The name of the resource group in which the app service reside | -| Yes |
| AppServiceName | The name of the app service to be deployed. | {ProjectName}| No |
| AllowAutomaticRename | Set whether to allow the provider to rename the app service if the app service name is not available  | -| No |
| DeploymentSlot | The deployment slot of the app service | -| No |
| ConnectionString | The connection string that shall be set to the app settings to be used by the application | -| No |