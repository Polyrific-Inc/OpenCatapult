# Coding Style

Polyrific Catapult uses [EditorConfig](https://editorconfig.org/) to help developers adhere with consistence coding style.

Please find the following guide https://docs.microsoft.com/en-us/visualstudio/ide/create-portable-custom-editor-options?view=vs-2017 on how to utilize EditorConfig in Visual Studio 2017. This plugin https://marketplace.visualstudio.com/items?itemName=EditorConfig.EditorConfig can be used to work with EditorConfig in Visual Studio Code.

## Naming Guidelines

* **Do** use **PascalCasing** for class names and method names

``` C#
public class ClientActivity
{
    public void ClearStatistics() {}
    public void CalculateStatistics() {}
}
```

* **Do** use **camelCasing** for method arguments and local variables

``` C#
public class UserLog
{
    public void Add(LogEvent logEvent)
    {
        int itemCount = logEvent.Items.Count;
        // ...
    }
}
```

* Do **NOT** use **Hungarian** notation or any other type identification in identifiers

``` C#
// Correct
int counter;
string name;
 
// Avoid
int iCounter;
string strName;
```

* Do **NOT** use **Screaming Caps** for constants or readonly variable

``` C#
// Correct
public static const string ShippingType = "DropShip";
 
// Avoid
public static const string SHIPPINGTYPE = "DropShip";
```

* Do **NOT** use **Underscores** in identifiers. *Exception*: you can prefix private variables 
     with an underscore.

``` C#
// Correct
public DateTime clientAppointment;
public TimeSpan timeLeft;
 
// Avoid
public DateTime client_Appointment;
public TimeSpan time_Left;
 
// Exception
private DateTime _registrationDate;
```

* **Do** use **predefined type names** instead of system type names like `Int16`, `Single`, `UInt64`, etc

``` C#
// Correct
string firstName;
int lastIndex;
bool isSaved;
 
// Avoid
String firstName;
Int32 lastIndex;
Boolean isSaved;
```

* **Do** use implicit type **var** for local variable declarations. *Exception*: primitive types (`int`, `string`, `double`, etc) use predefined names.

``` C#
var stream = File.Create(path);
var customers = new Dictionary();
 
// Exceptions
int index = 100;
string timeSheet;
bool isCompleted;
```

* **Do** use noun or noun phrases to name a class.

``` C#
public class Employee {}
public class BusinessLocation {}
public class DocumentCollection {}
```

* **Do** prefix interfaces with the letter **I**. Interface names are noun (phrases) or adjectives.

``` C#
public interface IShape {}
public interface IShapeCollection {}
public interface IGroupable {}
```

* **Do** name source files according to their main classes. *Exception*: file names with partial classes reflect their source or purpose, e.g. designer, generated, etc.

``` C#
// Located in Task.cs
public partial class Task {}

// Located in Task.generated.cs
public partial class Task {}
```

## Layout Conventions

* **Do** write only one statement per line
* **Do** write only one declaration per line
* **Do** add at least one blank line between method definitions and property definitions
* **Do** use parentheses to make clauses in an expression apparent, as shown in the following code:

``` C#
if ((val1 > val2) && (val1 > val3))
{
    // Take appropriate action.
}
```

## Type Design Guidelines

### Abstract Class

* Do **NOT** define public or protected internal constructors in abstract types.

> Constructors should be public only if users will need to create instances of the type. Because you cannot create instances of an abstract type, an abstract type with a public constructor is incorrectly designed and misleading to the users.

* **DO** define a protected or an internal constructor in abstract classes.
* **DO** provide at least one concrete type that inherits from each abstract class that you ship.

### Static CLass

* **DO** use static classes sparingly.

> Static classes should be used only as supporting classes for the object-oriented core of the framework.

* Do **NOT** treat static classes as a miscellaneous bucket.
* Do **NOT** declare or override instance members in static classes.

### Interface

* **AVOID** using marker interfaces (interfaces with no members)
* **DO** provide at least one type that is an implementation of an interface
* **DO** provide at least one API that consumes each interface you define (a method taking the interface as a parameter or a property typed as the interface)

## References

- https://www.dofactory.com/reference/csharp-coding-standards
- https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/
- https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions
