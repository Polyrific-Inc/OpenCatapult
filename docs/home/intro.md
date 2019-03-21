# Introduction

## What is `OpenCatapult`

`OpenCatapult` is an enterprise digital developer. It helps developers to significantly decrease application development time through intelligent automation.

`Catapult` was originally created and used internally by folks in [Polyrific](https://polyrific.com), and it has proved to be an essential tool in the success of delivering many [critical projects](https://polyrific.com/past-projects) in a relatively shorter time. However in the spirit of spreading the benefits into larger community, since October 2018, it transforms into `OpenCatapult` which is released as open source project.

## Why `OpenCatapult`

With `OpenCatapult`, you can:

* get an initial working application in relatively short time
* have highly customized application template which is unique to your requirement only
* still use your existing tools to build and deploy the application
* make changes to your application without pain
* contribute for any improvement you want to see in `OpenCatapult` because it is open source

## The Circle of Magic

![Circle of Magic](../img/circle.jpeg)

There is no magic, it's all just automation logics. But you will see what you believe, right?

## The Components

![Architecture](../img/general-arch.jpeg)

`OpenCatapult` consists of the following components:

### API

API is the central of `OpenCatapult` logics. It is an HTTP REST API system which usually acts as the bridge between `OpenCatapult` components. In real life implementation, API can be hosted in a web server (e.g. IIS), cloud service (e.g. Azure App Service), or as container application (e.g. Azure Kubernetes Service). You can find more details of the API features in [API References](../api/api.md).

### User Interface

This is the user-facing interface which is used to interact with `OpenCatapult`. User can choose to work with [Command Line Interface \(CLI\)](../cli/cli.md), [Web UI](../web/web.md), or Mobile App \(coming soon\). User Interface doesn't contain any business logics. It just forwards the requests to certain API endpoints and waits for the response. CLI can be hosted in user's machine (either Windows, Mac, or Linux).

### Engine

Engine runs as a stand-alone console application which waits for any queued jobs to be executed. It orchestrates the execution of job tasks, and reports back the result to the API asynchronously. It can be hosted in any machine (either Windows, Mac, or Linux) which has ability to reach the API endpoint. You can find more details about its capability in the [Engine References](../engine/engine.md).

### Task Providers

OpenCatapult's Engine is a platform-agnostic system. It means that Engine actually knows nothing about the concrete work that the job tasks do.
Task Provider is an `OpenCatapult` extension which provides specific implementation of a job task.

For example, Engine actually knows nothing about GitHub repository. All Engine knows is just it wants to push some code to a remote repository via `Push` task. So we need to provide a Task Provider, e.g. `Polyrific.Catapult.TaskProviders.GitHub`, which will handle the source code delivery in a specific way to GitHub. If you want to submit the code into another repository service, you can just plug in a new Task Provider, and reconfigure the `Push` task to use it.

`OpenCatapult` is packed with some built-in Task Providers. Please check them in the [Task Provider References](../task-providers/task-provider.md).
