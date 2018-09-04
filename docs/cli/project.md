# Archive
Archive a project

Usage: 
<code>dotnet pc.dll project archive --name [name]</code>

**Options**
<details>
    <summary>name (mandatory)</summary>
    <p>
        <code>--name</code> (alias: <code>-n</code>)
    </p>
    <p>
        Name of the project to be archived
    </p>
</details>

# Clone
Clone a project

Usage: 
<code>dotnet pc.dll project clone --project [project] --name [name]</code>

**Options**
<details>
    <summary>project (mandatory)</summary>
    <p>
        <code>--project</code> (alias: <code>-p</code>)
    </p>
    <p>
        Name of the project to be cloned
    </p>
</details>
<details>
    <summary>name (mandatory)</summary>
    <p>
        <code>--name</code> (alias: <code>-n</code>)
    </p>
    <p>
        Name of the new project
    </p>
</details>

# Create
Create a project

Usage: 
<code>dotnet pc.dll project create --name [name] --client [client] --template [template]</code>

**Options**
<details>
    <summary>name (mandatory)</summary>
    <p>
        <code>--name</code> (alias: <code>-n</code>)
    </p>
    <p>
        Name of the new project
    </p>
</details>
<details>
    <summary>client</summary>
    <p>
        <code>--client</code> (alias: <code>-c</code>)
    </p>
    <p>
        Client of the new project
    </p>
</details>
<details>
    <summary>template</summary>
    <p>
        <code>--template</code> (alias: <code>-t</code>)
    </p>
    <p>
        Project template name or path to template file (.yaml)
    </p>
</details>

# Export
Export project into a yaml file

Usage: 
<code>dotnet pc.dll project export --name [name]</code>

**Options**
<details>
    <summary>name (mandatory)</summary>
    <p>
        <code>--name</code> (alias: <code>-n</code>)
    </p>
    <p>
        Name of the project to be exported
    </p>
</details>
<details>
    <summary>output</summary>
    <p>
        <code>--output</code> (alias: <code>-o</code>)
    </p>
    <p>
        Output file location
    </p>
</details>

# List
List projects which the user has access to

Usage: 
<code>dotnet pc.dll project list --status [status]</code>

**Options**
<details>
    <summary>status</summary>
    <p>
        <code>--status</code> (alias: <code>-s</code>)
    </p>
    <p>
        Filter the projects by their status
    </p>
    <p>
        Default value: <code>all</code>
    <p>
    <p>
        Allowed values: <code>all</code> | <code>active</code> | <code>archived</code>
    <p>
</details>

# Remove
Remove a project

Usage: 
<code>dotnet pc.dll project remove --name [name]</code>

**Options**
<details>
    <summary>name (mandatory)</summary>
    <p>
        <code>--name</code> (alias: <code>-n</code>)
    </p>
    <p>
        Name of the project to be removed
    </p>
</details>

# restore
Restore an archived project

Usage: 
<code>dotnet pc.dll project restore --name [name]</code>

**Options**
<details>
    <summary>name (mandatory)</summary>
    <p>
        <code>--name</code> (alias: <code>-n</code>)
    </p>
    <p>
        Name of the project to be restored
    </p>
</details>