# Welcome to `OpenCatapult`

This is the official documentation for `OpenCatapult` project.

## What is `OpenCatapult`

`OpenCatapult` is an enterprise digital developer. It helps developers to significantly decrease the development time by intelligently automate things.

`Catapult` was originally created and used internally by folks in [Polyrific](https://polyrific.com), and it has proved to be an essential tool in the success of delivering many [critical projects](https://polyrific.com/past-projects) in a relatively shorter time. However in the spirit of spreading the benefits into larger community, since October 2018, it transforms into `OpenCatapult` which is released as open source project.

## Why `OpenCatapult`

With `OpenCatapult`, you can:

- get an initial working application in relatively short time
- have highly customized application template which is unique to your requirement only
- still use your existing tools to build and deploy the application
- make changes to your application without pain
- contribute for any improvement you want to see in `OpenCatapult` because it is open source

## The Circle of Magic

![Circle of Magic](img/circle.jpeg)

There is no magic, it's all just automation logics. But you will see what you believe, right?

## The Components

![Architecture](img/general-arch.jpeg)

`OpenCatapult` consists of the following components:

### API

API is the central of `OpenCatapult` logics. It is an HTTP REST API system which usually acts as the bridge between `OpenCatapult` components. You can find more details of the features in [API References](api/index.md).

### User Interface

This is the front-facing interface which is used by user to interact with `OpenCatapult`. User can choose to work with [Command Line Interface (CLI)](cli/index.md), 
Web (coming soon), or Mobile (coming soon). User Interface doesn't contain any logics. It just forwards the requests to API endpoints and waits for the response.

### Engine

Engine runs as a stand-alone console application which waits for any queued jobs to be executed. It orchestrates the execution of job tasks, and reports back the result to the API asynchronously. You can find more details about its capability in the [Engine References](engine/index.md).

### Service Providers (Plugins)

Service Providers are specific implementation of job tasks. As Engine is a platform-agnostic system, it is the Service Provider's job to be aware of any external services it's trying to work with.

For example, Engine actually knows nothing about GitHub repository. All Engine knows is just it wants to push some code to a remote repository via Push Task.
So we need to provide a plugin for the Push Task, e.g. `GitHubRepositoryProvider`, which will handle the source code delivery in a specific way to GitHub.