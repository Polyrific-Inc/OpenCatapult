# Build Scripts

If you want to build and run OpenCatapult components on your local machine, we have provided some PowerShell scripts to help you do it easily.

Each of the following scripts should be able to run on Windows, Linux, or Mac machines as long as you have PowerShell (either version 5 or 6) installed in it.

## build-all.ps1

This script will build and run all components. The API component will run in current shell, while the Engine, CLI, Web components will be opened and run in different shells.

Underneath, it is just a wrapper for the other component's build scripts. So if you have trouble in using it, you can always run each component's build script individually.

Usage:
```sh
.\builds\build-all.ps1
```

Options:
| Argument | Description | Value | Default Value |
| --- | --- | --- | --- |
| -configuration | Build configuration | Debug / Release | Release |
| -connString | Database connection string for API | [connection string] | |
| -environment | Dotnet host environment | Development / Production | Development |
| -http | Base URL of HTTP protocol | [http url] | http://localhost:8005 |
| -https | Base URL of HTTPS protocol | [https url] | https://localhost:44305 |
| -webHost | The host name of the Web UI | [web host name] | localhost |
| -webPort | The port number of the Web UI | [web port number] | 44300 |
| -noConfig | Do not replace the config values of the components | true / false | false |
| -noCli | Do not build and run CLI component | true / false | false |
| -noPrompt | Run the script without user interaction (just run everything with its default value) | true / false | false |
| -noRun | Do not run the API directly after the build | true / false | false |
| -noWeb | Do not build and run Web UI component | true / false | false |
| -terminal | Type of the terminal to be used to run components in Linux | [terminal type] | gnome-terminal |

## build-api.ps1

This script will build and run API component.

Usage:
```sh
.\builds\build-api.ps1
```

Options:
| Argument | Description | Value | Default Value |
| --- | --- | --- | --- |
| -configuration | Build configuration | Debug / Release | Release |
| -connString | Database connection string for API | [connection string] | |
| -environment | Dotnet host environment | Development / Production | Development |
| -http | Base URL of HTTP protocol | [http url] | http://localhost:8005 |
| -https | Base URL of HTTPS protocol | [https url] | https://localhost:44305 |
| -noPrompt | Run the script without user interaction (just run everything with its default value) | true / false | false |
| -noRun | Do not run the API directly after the build | true / false | false |

## build-cli.ps1

This scripts will build and run CLI component.

Usage:
```sh
.\builds\build-cli.ps1
```

Options:
| Argument | Description | Value | Default Value |
| --- | --- | --- | --- |
| -configuration | Build configuration | Debug / Release | Release |
| -noConfig | Do not replace the config values | true / false | false |
| -noOpenShell | Do not go to publish directory and modify shell window title | true / false | false |
| -noBuild | Do not perform build operation | true / false | false |
| -url | Base URL of API endpoint | [https url] | https://localhost:44305 |

## build-engine.ps1

This script will build and run Engine component.

Usage:
```sh
.\builds\build-engine.ps1
```

Options:
| Argument | Description | Value | Default Value |
| --- | --- | --- | --- |
| -configuration | Build configuration | Debug / Release | Release |
| -noConfig | Do not replace the config values | true / false | false |
| -noOpenShell | Do not go to publish directory and modify shell window title | true / false | false |
| -noBuild | Do not perform build operation | true / false | false |
| -url | Base URL of API endpoint | [https url] | https://localhost:44305 |

## build-web.ps1

This script will build and run Web UI component.

Usage:
```sh
.\builds\build-web.ps1
```

Options:
| Argument | Description | Value | Default Value |
| --- | --- | --- | --- |
| -host | Host name | [host name] | localhost |
| -port | Host port number | [port number] | 44300 |