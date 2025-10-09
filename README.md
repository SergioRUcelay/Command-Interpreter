# Command Interpreter

**Command Interpreter (CI)** is a lightweight and versatile library designed to simplify the execution of functions through text-based commands in any console or command-line interface within your application. Whether you're working with custom consoles, operating system terminals, or browser-based interfaces, CI enables dynamic function invocation with ease.

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

## 🚀 Overview

Command Interpreter provides a robust solution for executing functions dynamically via textual input. It parses commands, validates parameters, and returns structured results in XML format, making it ideal for developers building flexible, command-driven applications.

### Key Features
- **Dynamic Function Invocation**: Execute functions by sending text-based commands from any interface.
- **Parameter Validation**: Automatically verifies the number and type of parameters required by the function.
- **Custom Parameter Types**: Define custom parameter types with regex-based validation for precise control.
- **Structured Output**: Returns results in a consistent XML format for easy integration.
- **Cross-Platform Compatibility**: Works with custom consoles, OS terminals, browsers, or any text-based interface.
- **Lightweight and Modular**: Delivered as a DLL, easily integrated into your projects.

## 🛠️ Installation

To integrate Command Interpreter into your project, follow these steps:

1. **Get the source or download the .DLL**:
   - Clone this repository:
     ```bash
     git clone https://github.com/SergioRUcelay/Command-Interpreter.git
     ```
   - Alternatively, download the latest release from the [Releases](#) page (coming soon).

2. **Add to Your Project**:
   - Embed the source.
   - Reference the `CommandInterpreter.dll` in your project (e.g., in C# or other .NET-compatible projects)...
   - ...or ensure the DLL is included in your project’s build path.
   
   - <img height="300" alt="Arkanoid_Ci_05" src="https://github.com/user-attachments/assets/2f63e455-7bea-4b32-9543-d0ce5e0ce74d" />


3. **Dependencies**:
   - .NET Framework 4.5 or higher (or .NET Core, depending on the build).
   - No additional dependencies required.

## 📖 Usage

### High Level Code structure
#### Classes
 ##### `Interpreter`:
  - **RegisterFunction(string command, Delegate func, string info)**: Adds a command and its associated delegate function to the command
  - **Command (string command)**: Receives a canditate string that will be executed upon succesfully matching a registerd function. 
 ##### `Parameters` (member of the Interpreter class):
  - **RegisterCustomType<T>(string regex, Parser<T> parser)**: Associates a regular expression with a parser for a specific type.
  - **ClearParser<T>(T key)**: Removes the parser associated with the specified key type.
    
### Predefined Functions
Command Interpreter comes with a set of built-in functions to enhance usability out of the box. Below is an overview of key predefined functions:

  - **Help**: Displays all registered functions available in the Command Interpreter instance.
      - **Syntax**: `help`
      - **Output**: Returns an XML-formatted list of function names and their parameter requirements.
        
  - **Parsing**: Displays all registered functions available in the Command Interpreter instance.
      - **Syntax**: `parsing`
      - **Output**: Returns an XML-formatted list of types that can be parsed.

### How to get started

We want to do three basic things: Register a function, tick the system with commmands, and handle both errors and return values, let's look at them in turn:

### Registering a simple function
To register a function, you call the `RegisterFunction` method, which takes three parameters: the first is the name of the function being called from the CLI, the second is the function to be invoked and finally, the string describing the function being called.

``` Csharp
// The mainspace include in the .DLL
using CommandInterpreter;

class Program
{
    static void Main(string[] args)
    {
        // Initialize the Command Interpreter
        Interpreter interpreter = new();

        // Define a sample function
        interpreter.RegisterFunction("AddNumbers", (int a, int b) => a + b), "Function that adds the given numbers.");
    }
}
```
### Ticking the Command Interpreter
We also need to feed the Command Line Interpreter with commands, they can be queued, retrieved from a HTTP connection (as ArkaClone does) or, in the simplest case, just grabbed from a standard terminal:
````Csharp
while (!Ci.Terminate)
{
   string? command = Console.ReadLine();

   //sending the string to the CI.
   CommandReply result = interpreter.Command(command);
[CONTINUED BELOW]
````

### Return values
Your triggered function could return something useful or, alternatively, could be there was an error somehwere in the process (may it be an unmatched command, a bad parameter or an error inside of the function that's called)

The code returns a `CommmandReply` class that can be either examined or trivially serialized to XML (it is already set up for that). Once the result is in XML a simple XSLT transformation can make it match whatever client is consuming the result:
````Csharp
[CONTINUES FROM ABOVE]
   // Serialize the response to XML.
   var a = WriterOfNewXmlString(result);
   // Transform the XML to colored Windows console text through a simple XSLT.
   var ed = XmlToText(WriterOfNewXmlString(result));
   // voila!
   Console.WriteLine(ed);
}
````
Tada! You running app!

<img height="200" alt="Ci-console-addnumbers" src="https://github.com/user-attachments/assets/0fd20f09-121f-45b8-9f4e-8e1403fc3085" />

### Command Syntax
- Commands follow the format: `FunctionName param1 param2 ...`
- **Validation**: The interpreter checks each parameter against the regex pattern during command execution.
- **Error Handling**: If a parameter fails validation the system handles and returns a description of what's wrong.
- The result can be trivially serialized to XML format for easy parsing. The above result was made into plain text with one of the included sample XSLT sheets.

## Custom parameters:
Command Interpreter allows you to define custom parameter types using regular expressions (regex) for precise validation. This is useful for both enforcing specific formats, such as email addresses, phone numbers, or adding types to the available parameter parser.
You can add new types through the `Parameters` member of the `Interpreter` class, and then the `RegisterCustomType` function that receives both the regex string used for parsing the new type and a delegate that creates the C# type from the parsed data. The return type of said delegate is, indeed, the newly defined type.

#### Example: Registering a Vector3 type parser
``` Csharp
using CommandInterpreter;
using System.Numerics;

class Program
{
    static void Main(string[] args)
    {
        // Initialize the Command Interpreter
        Interpreter interpreter = new();

        // Define a custom type for Vector3
        string RegexVector3 = @"(-?\d+(\.\d+)?),(-?\d+(\.\d+)?),(-?\d+(\.\d+)?)";
        interpreter.Parameters.RegisterCustomType(RegexVector3,(groups) => new Vector3(float.Parse(groups[1].Value),
                                                       float.Parse(groups[3].Value),float.Parse(groups[5].Value)));

        // Register a function that accepts an Vector3
        string _v3= "Function to send a Vector3";
        interpreter.RegisterFunction("Setpoint", (Vector3 v) => $"Create a new point in {v}", _v3);
    }
}
```
<img height="200" alt="Ci-console-setpoint" src="https://github.com/user-attachments/assets/5fba5085-1b91-444f-8aae-70aeffb38881" />

For more detailed examples, check the [ConsoleApplication/](Application/) and [ConsoleWeb/](ConsoleWeb/) directories.

### Custom returned types
Some times you want a function to return a type that can not, by definition, be part of the class that's serialized in XML, in those cases you can catch the return type and do whatever handling you see fit for your scenario.
In my case, in ArkaClone, I wanted a function to return a 2DTexture and convert it so that it can show, in a web-based command-line interpreter, as an inlined image.

So the function registered has this format
```Csharp
//We declare the function that generates the screenshot:
public Texture2D CaptureWindow()
{
   int w = GraphicsDevice.PresentationParameters.BackBufferWidth;
   int h = GraphicsDevice.PresentationParameters.BackBufferHeight;
   Texture2D deltaCapture = new(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color);
   Color[] pixelData = new Color[h * w];

   GraphicsDevice.GetBackBufferData(pixelData);
   deltaCapture.SetData(pixelData);
   return deltaCapture;
}
...
//And add it to the list of the registered functions
com.RegisterFunction("Cap", game.CaptureWindow, "Capture a screen of the game");
```
Then I react to that result type and convert it to base64 and embed it as text in the XML we are generating
```Csharp
//Usual command tick
CommandReply result = com.Command(command);
//if the returned object is of the Texture2D type
if (result.Return is Texture2D buffa)
{
   //read and convert to safe text and replace Result with the new base64'ed text
   using (MemoryStream ms = new MemoryStream())
   {
      buffa.SaveAsPng(ms, buffa.Width, buffa.Height);
      //Now I set the result object to be a string, that happens to be the screen capture in base64 format
      result.Return = Convert.ToBase64String(ms.ToArray());
   }
}
```
finally I conver the resulting XML (from serializing the CommandReply object), using XSLT to an embeded image with a default size, click-to-zoom and so on
```XML
<!-- Return contains a base64 image, which is prefixed by iVBOR -->
<xsl:when test="contains(Return, 'iVBOR')">
   <div>
      <img class="capture">
         <xsl:attribute name="src">
            <xsl:text>data:image/png;base64,</xsl:text>
            <xsl:value-of select="Return"/>
         </xsl:attribute>
         <xsl:attribute name="style">width:50%; height:auto;</xsl:attribute>
         <xsl:attribute name="onload">adjustScroll()</xsl:attribute>
      </img>
   </div>
</xsl:when>
```

### Using it in an existing project

As an example of including CI in an external project I used my own ArkaClone project*. In the project, I created various functions to alter the game's behavior at runtime. I communicate with CI, from ArkaClone using a small WebApp that implements a console.
*(Check it out to see an [Arkaoid](https://github.com/SergioRUcelay/ArknoidClone) that's both driven by FSM and linear algebra)

In that project I have added a number of functions, which you can see here:

<img height="300" alt="Arkanoid_Ci_01" src="https://github.com/user-attachments/assets/d47e5b7c-2a61-428f-972c-0dc9084acfec" />


Among those you can see the `cap` function that captures the screen, mentioned above, which takes a screenshot of the game.


 <img  height="500" alt="Arkanoid_Ci_04" src="https://github.com/user-attachments/assets/1614128f-213e-44c1-b168-4f0e2ee11669" />

 ...and of course, several other functions that modify its visual appearance.

 In this link to the [Video](https://youtu.be/g0YjEpLHpeQ), you can see a complete example of the registered functions and how they behave and interact with the console:


## 🤝 Contributing

Contributions are welcome! To contribute to Command Interpreter:

1. Fork the repository.
2. Create a new branch (`git checkout -b feature/your-feature`).
3. Make your changes and commit (`git commit -m "Add your feature"`).
4. Push to your branch (`git push origin feature/your-feature`).
5. Open a Pull Request.

## 📜 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## 🙌 Acknowledgments

- Built with ❤️ by [SergioRUcelay](https://github.com/SergioRUcelay).
- Inspired by the need for flexible, command-driven interfaces in modern applications.

## 📬 Contact

Have questions or suggestions? Reach out via [GitHub Issues](https://github.com/SergioRUcelay/Command-Interpreter/issues) or connect with me on [LinkedIn](https://www.linkedin.com/in/sergiorucelay/).

---

*Command Interpreter is part of my portfolio, showcasing my ability to design modular, developer-friendly libraries. Explore my other projects on [GitHub](https://github.com/SergioRUcelay)!*
