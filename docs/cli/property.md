# Add
Add a project data model property

Usage: 
<code>dotnet pc.dll property add --project [project] --model [model] --name [name]</code>

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
    <summary>model (mandatory)</summary>
    <p>
        <code>--model</code> (alias: <code>-m</code>)
    </p>
    <p>
        Name of the data model
    </p>
</details>
<details>
    <summary>name (mandatory)</summary>
    <p>
        <code>--name</code> (alias: <code>-n</code>)
    </p>
    <p>
        Name of the data model property
    </p>
</details>
<details>
    <summary>label</summary>
    <p>
        <code>--label</code> (alias: <code>-l</code>)
    </p>
    <p>
        Label of the data model property
    </p>
</details>
<details>
    <summary>type</summary>
    <p>
        <code>--type</code> (alias: <code>-t</code>)
    </p>
    <p>
        Data type of the property
    </p>
    <p>
        Default value: <code>string</code>
    </p>
    <p>
        Allowed values: <code>string</code> | <code>int</code> | <code>short</code> | <code>float</code> | <code>double</code> | <code>decimal</code> | <code>bool</code> | <code>datetime</code> |<code>byte</code> | <code>guid</code> | <code>dbgeography</code>
    </p>
</details>
<details>
    <summary>controltype</summary>
    <p>
        <code>--controltype</code> (alias: <code>-ct</code>)
    </p>
    <p>
        Control type of the property
    </p>
    <p>
        Default value: <code>input-text</code>
    </p>
    <p>
        Allowed values: <code>input-text</code> | <code>input-number</code> | <code>input-file</code> | <code>textarea</code> | <code>checkbox</code> | <code>select</code> | <code>radio</code> | <code>calendar</code> |<code>image</code>
    </p>
</details>
<details>
    <summary>relational</summary>
    <p>
        <code>--relational</code> (alias: <code>-r</code>)
    </p>
    <p>
        Name of the related data model
    </p>
</details>
<details>
    <summary>relationaltype</summary>
    <p>
        <code>--relationaltype</code> (alias: <code>-rt</code>)
    </p>
    <p>
        Relationship type
    </p>
    <p>
        Default value: <code>one-to-one</code>
    </p>
    <p>
        Allowed values: <code>one-to-one</code> | <code>one-to-many</code> | <code>many-to-many</code>
    </p>
</details>

# List
List properties in a data model

Usage: 
<code>dotnet pc.dll property list --project [project] --model [model</code>

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
    <summary>model (mandatory)</summary>
    <p>
        <code>--model</code> (alias: <code>-m</code>)
    </p>
    <p>
        Name of the data model
    </p>
</details>

# Remove
Remove a project data model property

Usage: 
<code>dotnet pc.dll property remove --project [project] --model [model] --name [name]</code>

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
    <summary>model (mandatory)</summary>
    <p>
        <code>--model</code> (alias: <code>-m</code>)
    </p>
    <p>
        Name of the data model
    </p>
</details>
<details>
    <summary>name (mandatory)</summary>
    <p>
        <code>--name</code> (alias: <code>-n</code>)
    </p>
    <p>
        Name of the data model property
    </p>
</details>

# Update
Update a project data model property

Usage: 
<code>dotnet pc.dll property update --project [project] --name [name] --rename [newname]</code>

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
    <summary>model (mandatory)</summary>
    <p>
        <code>--model</code> (alias: <code>-m</code>)
    </p>
    <p>
        Name of the data model
    </p>
</details>
<details>
    <summary>name (mandatory)</summary>
    <p>
        <code>--name</code> (alias: <code>-n</code>)
    </p>
    <p>
        Name of the data model property
    </p>
</details>
<details>
    <summary>rename</summary>
    <p>
        <code>--rename</code> (alias: <code>-r</code>)
    </p>
    <p>
        New name of the data model property
    </p>
</details>
<details>
    <summary>label</summary>
    <p>
        <code>--label</code> (alias: <code>-l</code>)
    </p>
    <p>
        Label of the data model property
    </p>
</details>
<details>
    <summary>type</summary>
    <p>
        <code>--type</code> (alias: <code>-t</code>)
    </p>
    <p>
        Data type of the property
    </p>
    <p>
        Allowed values: <code>string</code> | <code>int</code> | <code>short</code> | <code>float</code> | <code>double</code> | <code>decimal</code> | <code>bool</code> | <code>datetime</code> |<code>byte</code> | <code>guid</code> | <code>dbgeography</code>
    </p>
</details>
<details>
    <summary>controltype</summary>
    <p>
        <code>--controltype</code> (alias: <code>-ct</code>)
    </p>
    <p>
        Control type of the property
    </p>
    <p>
        Allowed values: <code>input-text</code> | <code>input-number</code> | <code>input-file</code> | <code>textarea</code> | <code>checkbox</code> | <code>select</code> | <code>radio</code> | <code>calendar</code> |<code>image</code>
    </p>
</details>
<details>
    <summary>relational</summary>
    <p>
        <code>--relational</code> (alias: <code>-r</code>)
    </p>
    <p>
        Name of the related data model
    </p>
</details>
<details>
    <summary>relationaltype</summary>
    <p>
        <code>--relationaltype</code> (alias: <code>-rt</code>)
    </p>
    <p>
        Relationship type
    </p>
    <p>
        Allowed values: <code>one-to-one</code> | <code>one-to-many</code> | <code>many-to-many</code>
    </p>
</details>