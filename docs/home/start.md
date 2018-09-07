# Quick Start

This document will guide you to get started with Polyrific Catapult.

## Setup from release library

Please go to <https://opencatapult.net/releases> to download the latest release of Polyrific Catapult.

Unzip the release package into your local directory, and run the components.

## Build from source code

Clone Polyrific Catapult source code from the repository:
``` shell
git clone https://github.com/Polyrific-Inc/OpenCatapult.git
```

Go to the root folder:
``` shell
cd OpenCatapult
```

Run the API:
``` shell
dotnet run -p .\src\API\Polyrific.Catapult.Api\Polyrific.Catapult.Api.csproj -c Release
```

Open new shell, go to the root folder, build and start the Engine:
``` shell
dotnet build .\src\Engine\Polyrific.Catapult.Engine\Polyrific.Catapult.Engine.csproj -c Release
dotnet .\src\Engine\Polyrific.Catapult.Engine\bin\Release\PCEngine.dll config set -n ApiUrl -v https://localhost:5001
dotnet .\src\Engine\Polyrific.Catapult.Engine\bin\Release\PCEngine.dll start
```

Open new shell, go to the root folder, build and run a CLI command:
``` shell
dotnet build .\src\CLI\Polyrific.Catapult.Cli\Polyrific.Catapult.Cli.csproj -c Release
dotnet .\src\CLI\Polyrific.Catapult.Cli\bin\Release\PC.dll --help
```

## Create your first project

