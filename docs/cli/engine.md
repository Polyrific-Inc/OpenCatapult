# Activate
Activate a suspended engine

Usage: 
<code>dotnet pc.dll engine activate --name [name]</code>

**Options**
<details>
    <summary>name (mandatory)</summary>
    <p>
        <code>--name</code> (alias: <code>-n</code>)
    </p>
    <p>
        Name of the engine to be activated
    </p>
</details>

# Get
Get a single engine record

Usage: 
<code>dotnet pc.dll engine get --name [name]</code>

**Options**
<details>
    <summary>name (mandatory)</summary>
    <p>
        <code>--name</code> (alias: <code>-n</code>)
    </p>
    <p>
        Name of the engine to get
    </p>
</details>

# List
List registered engines

Usage: 
<code>dotnet pc.dll engine list --status [status]</code>

**Options**
<details>
    <summary>status</summary>
    <p>
        <code>--status</code> (alias: <code>-s</code>)
    </p>
    <p>
        Filter the engines by their status.
    </p>
    <p>
        Allowed values: <code>all</code> | <code>active</code> | <code>suspended</code> | <code>running</code>.
    </p>
    <p>
        Default value: <code>all</code>.
    </p>
</details>

# Register
Register a new engine

Usage: 
<code>dotnet pc.dll engine register --name [name]</code>

**Options**
<details>
    <summary>name (mandatory)</summary>
    <p>
        <code>--name</code> (alias: <code>-n</code>)
    </p>
    <p>
        Name of the engine to be registered
    </p>
</details>

# Remove
Remove an engine

Usage: 
<code>dotnet pc.dll engine remove --name [name]</code>

**Options**
<details>
    <summary>name (mandatory)</summary>
    <p>
        <code>--name</code> (alias: <code>-n</code>)
    </p>
    <p>
        Name of the engine to be removed
    </p>
</details>

# Suspend
Suspend an engine

Usage: 
<code>dotnet pc.dll engine suspend --name [name]</code>

**Options**
<details>
    <summary>name (mandatory)</summary>
    <p>
        <code>--name</code> (alias: <code>-n</code>)
    </p>
    <p>
        Name of the engine to be suspended
    </p>
</details>

# Token
Generate a token for the engine

Usage: 
<code>dotnet pc.dll engine token --name [name]</code>

**Options**
<details>
    <summary>name (mandatory)</summary>
    <p>
        <code>--name</code> (alias: <code>-n</code>)
    </p>
    <p>
        Name of the engine
    </p>
</details>