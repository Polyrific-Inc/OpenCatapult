# Add
Add job to queue

Usage: 
<code>dotnet pc.dll queue add --project [project] --job [job]</code>

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

# List
List queued jobs

Usage: 
<code>dotnet pc.dll queue list --project [project]</code>

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

# Get
Get complete log of a queued job

Usage: 
<code>dotnet pc.dll queue get --project [project] --number [number]</code>

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
    <summary>number (mandatory)</summary>
    <p>
        <code>--number</code> (alias: <code>-n</code>)
    </p>
    <p>
        Queue number
    </p>
</details>

# Restart
Restart the pending queue

Usage: 
<code>dotnet pc.dll queue restart --project [project] --number [number]</code>

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
    <summary>number (mandatory)</summary>
    <p>
        <code>--number</code> (alias: <code>-n</code>)
    </p>
    <p>
        Queue number
    </p>
</details>