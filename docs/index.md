# Welcome to Polyrific Catapult

This is the official documentation for open source Polyrific Catapult project.

## What is Polyrific Catapult

Polyrific Catapult is an enterprise digital developer. It will help developers to significantly decrease the development time by intelligently automate things.

## Why Polyrific Catapult

With Polyrific Catapult, you can:

- get an initial working application in relatively short time
- have highly customized application template which is unique to your requirement only
- still use your existing tools to build and deploy the application
- make changes to your application without pain
- contribute for any improvement you want to see in Polyrific Catapult because it is open source

## High Level Architecture

![Architecture](img/general-arch.jpeg)

Polyrific Catapult consists of the following components:

### API

API is the central of Polyrific Catapult logics. It is an HTTP REST API system which often acts as the bridge between Polyrific Catapult components.

### User Interface

This is the front-facing interface which is used by user to interact with Polyrific Catapult. User can choose to work with Command Line Interface (CLI), 
Web, or Mobile. User Interface doesn't contain any logics. It just forwards the requests to API endpoints and waits for the response.

### Engine

Engine runs as a stand-alone application which waits for any queued jobs to be executed. It orchestrates the execution of job tasks, and reports back the result
to the API asynchronously.

### Service Providers (Plugins)

Service Provider is a specific implementation of job tasks. As Engine is a platform-agnostic system, it is Service Provider's job to work with external services.

For example, Engine actually knows nothing about GitHub repository. All Engine knows is just it wants to push some code to a remote repository via Push Task.
So we need to provide a plugin for the Push Task, e.g. `GitHubRepositoryProvider`, which will handle the source code delivery in a specific way to GitHub.