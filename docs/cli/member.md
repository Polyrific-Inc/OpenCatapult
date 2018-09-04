# Add
Add user as a project member

Usage: 
<code>dotnet pc.dll member add --project [project] --user [user] --role [role]</code>

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
    <summary>user (mandatory)</summary>
    <p>
        <code>--user</code> (alias: <code>-u</code>)
    </p>
    <p>
        Username (email) of the user
    </p>
</details>
<details>
    <summary>role</summary>
    <p>
        <code>--role</code> (alias: <code>-r</code>)
    </p>
    <p>
        Role of the project member
    </p>
    <p>
        Default value: <code>member</code>
    </p>
    <p>
        Allowed values: <code>member</code> | <code>contributor</code> | <code>maintainer</code> | <code>owner</code>
    </p>
</details>

# List
List members of the project

Usage: 
<code>dotnet pc.dll member list --project [project]</code>

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
    <summary>role</summary>
    <p>
        <code>--role</code> (alias: <code>-r</code>)
    </p>
    <p>
        Role of the project member
    </p>
    <p>
        Default value: <code>all</code>
    </p>
    <p>
        Allowed values: <code>all</code> | <code>member</code> | <code>contributor</code> | <code>maintainer</code> | <code>owner</code>
    </p>
</details>

# Remove
Remove a project member

Usage: 
<code>dotnet pc.dll member remove --project [project] --user [user]</code>

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
    <summary>user (mandatory)</summary>
    <p>
        <code>--user</code> (alias: <code>-u</code>)
    </p>
    <p>
        Username (email) of the user
    </p>
</details>

# Update
Update the role of a project member

Usage: 
<code>dotnet pc.dll member update --project [project] --name [name]</code>

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
    <summary>user (mandatory)</summary>
    <p>
        <code>--user</code> (alias: <code>-u</code>)
    </p>
    <p>
        Username (email) of the user
    </p>
</details>
<details>
    <summary>role</summary>
    <p>
        <code>--role</code> (alias: <code>-r</code>)
    </p>
    <p>
        Role of the project member
    </p>
    <p>
        Allowed values: <code>member</code> | <code>contributor</code> | <code>maintainer</code> | <code>owner</code>
    </p>
</details>