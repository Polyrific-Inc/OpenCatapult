# Add
Add a new job task definition

Usage: 
<code>dotnet pc.dll task add --project [project] --job [job] --name [name]</code>

**Options**
<details>
    <summary>project (mandatory)</summary>
    <p>
        <code>--project</code> (alias: <code>-p</code>)
    </p>
    <p>
        Name of the project
    </p>
</details>
<details>
    <summary>job (mandatory)</summary>
    <p>
        <code>--job</code> (alias: <code>-j</code>)
    </p>
    <p>
        Name of the job definition
    </p>
</details>
<details>
    <summary>name (mandatory)</summary>
    <p>
        <code>--name</code> (alias: <code>-n</code>)
    </p>
    <p>
        Name of the new job task definition
    </p>
</details>
<details>
    <summary>type</summary>
    <p>
        <code>--type</code> (alias: <code>-t</code>)
    </p>
    <p>
        Type of the task
    </p>
    <p>
        Default value: <code>generate</code>
    </p>
    <p>
        Allowed values: <code>generate</code> | <code>push</code> | <code>build</code> | <code>deploy</code> | <code>deploydb</code>
    </p>
</details>
<details>
    <summary>property</summary>
    <p>
        <code>--property</code> (alias: <code>-prop</code>)
    </p>
    <p>
        Property of the task. Several properties can be added. A key and value should be provided for each property using the format <code>key:value</code>. Sample usage: <code>--property githubUser:testuser --property githubPassword:testPassword</code>
    </p>
</details>

# List
List job task definitions

Usage: 
<code>dotnet pc.dll task list --project [project] --job [job]</code>

**Options**
<details>
    <summary>project (mandatory)</summary>
    <p>
        <code>--project</code> (alias: <code>-p</code>)
    </p>
    <p>
        Name of the project
    </p>
</details>
<details>
    <summary>job (mandatory)</summary>
    <p>
        <code>--job</code> (alias: <code>-j</code>)
    </p>
    <p>
        Name of the job definition
    </p>
</details>

# Remove
Remove a job task definition

Usage: 
<code>dotnet pc.dll task remove --project [project] --job [job] --name [name]</code>

**Options**
<details>
    <summary>project (mandatory)</summary>
    <p>
        <code>--project</code> (alias: <code>-p</code>)
    </p>
    <p>
        Name of the project
    </p>
</details>
<details>
    <summary>job (mandatory)</summary>
    <p>
        <code>--job</code> (alias: <code>-j</code>)
    </p>
    <p>
        Name of the job definition
    </p>
</details>
<details>
    <summary>name (mandatory)</summary>
    <p>
        <code>--name</code> (alias: <code>-n</code>)
    </p>
    <p>
        Name of the job task definition to be removed
    </p>
</details>

# Update
Update a job task definition

Usage: 
<code>dotnet pc.dll task update --project [project] --job [job] --name [name] --rename [newname]</code>

**Options**
<details>
    <summary>project (mandatory)</summary>
    <p>
        <code>--project</code> (alias: <code>-p</code>)
    </p>
    <p>
        Name of the project
    </p>
</details>
<details>
    <summary>job (mandatory)</summary>
    <p>
        <code>--job</code> (alias: <code>-j</code>)
    </p>
    <p>
        Name of the job definition
    </p>
</details>
<details>
    <summary>name (mandatory)</summary>
    <p>
        <code>--name</code> (alias: <code>-n</code>)
    </p>
    <p>
        Name of the data model
    </p>
</details>
<details>
    <summary>rename</summary>
    <p>
        <code>--rename</code> (alias: <code>-r</code>)
    </p>
    <p>
        New name of the job task definition
    </p>
</details>
<details>
    <summary>type</summary>
    <p>
        <code>--type</code> (alias: <code>-t</code>)
    </p>
    <p>
        Type of the task
    </p>
    <p>
        Allowed values: <code>generate</code> | <code>push</code> | <code>build</code> | <code>deploy</code> | <code>deploydb</code>
    </p>
</details>
<details>
    <summary>property</summary>
    <p>
        <code>--property</code> (alias: <code>-prop</code>)
    </p>
    <p>
        Property of the task. Several properties can be added. A key and value should be provided for each property using the format <code>key:value</code>. Sample usage: <code>--property githubUser:testuser --property githubPassword:testPassword</code>
    </p>
</details>