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

1. **Download the DLL**:
   - Clone this repository:
     ```bash
     git clone https://github.com/SergioRUcelay/Command-Interpreter.git
     ```
   - Alternatively, download the latest release from the [Releases](#) page (coming soon).

2. **Add to Your Project**:
   - Reference the `CommandInterpreter.dll` in your project (e.g., in C# or other .NET-compatible projects).
   - Ensure the DLL is included in your project’s build path.

3. **Dependencies**:
   - .NET Framework 4.5 or higher (or .NET Core, depending on the build).
   - No additional dependencies required.

## 📖 Usage

Here’s a quick example to get you started with Command Interpreter:
To register a function, you must call the `RegisterFunction` method, which takes three parameters: the first is the name of the function being called from the CLI, the second is the function to be invoked and finally, the string describing the function being called.

### Example: Registering a Function
``` Csharp
using CommandInterpreter;

class Program
{
    static void Main(string[] args)
    {
        // Initialize the Command Interpreter
        Interpreter interpreter = new();

        // Define a sample function
        string _Int = "Function that adds the given numbers.";
        interpreter.RegisterFunction("AddNumbers", (int a, int b) => a + b), _Int);
    }
}
```
<img height="200" alt="Ci-console-addnumbers" src="https://github.com/user-attachments/assets/0fd20f09-121f-45b8-9f4e-8e1403fc3085" />

### Command Syntax
- Commands follow the format: `FunctionName param1 param2 ...`
- Parameters are validated for type and count before execution.
- Results are returned in XML format for easy parsing. The above result was made into plain text with one of the included sample XSLT sheets.

## Custom Types:
Command Interpreter allows you to define custom parameter types using regular expressions (regex) for precise validation. This is useful for both enforcing specific formats, such as email addresses, phone numbers, or extending through custom string patterns.
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

#### How It Works
- **RegisterCustomType**: Use this method to define a custom type with a regex pattern.
- **Validation**: The interpreter checks each parameter against the regex pattern during command execution.
- **Error Handling**: If a parameter fails validation, an XML-formatted error message is returned.

For more detailed examples, check the [ConsoleApplication/](Application/) and [ConsoleWeb/](ConsoleWeb/) directories.

### Predefined Functions
Command Interpreter comes with a set of built-in functions to enhance usability out of the box. Below is an overview of key predefined functions:

  - **Help**: Displays all registered functions available in the Command Interpreter instance.
      - **Syntax**: `help`
      - **Output**: Returns an XML-formatted list of function names and their parameter requirements.
  - **Parsing**: Displays all registered functions available in the Command Interpreter instance.
      - **Syntax**: `parsing`
      - **Output**: Returns an XML-formatted list of types that can be parsed.
   
### Classes
 #### `Interpreter`:
  - **RegisterFunction(string command, Delegate func, string info)**: Adds a command and its associated delegate function to the command
 #### `Parameters` (as a member of the Interpreter class):
  - **RegisterCustomType<T>(string regex, Parser<T> parser)**: Associates a regular expression with a parser for a specific type.
  - **ClearParser<T>(T key)**: Removes the parser associated with the specified key type.

### Application to a project:

As an example of applying CI to an external project, I used my own ArkaClone project. (You can visit it to see an application of an FSM and
linear algebra in collisions.) In the project, I created various functions to alter the game's behavior at runtime. I implemented a very simple
console in a small WebApp to communicate with Arkanoid.
   From there, the first function I call is `help` to list all the registered functions, as seen in the example.

 <img height="500" alt="Arkanoid_Ci_01" src="https://github.com/user-attachments/assets/98afcc37-ac2f-4c80-9bf9-866d3ce11b09" /> 
 
 
Another function called `cap` takes a screenshot of the game. And several others that modify its visual appearance:

 
 <img  height="500" alt="Arkanoid_Ci_03" src="https://github.com/user-attachments/assets/f914f306-9080-4c67-969f-d0bd71a7352c" />

 

 In this link to the [Video](https://youtu.be/zr9vkhosWPQ)., you can see a complete example of the registered functions and how they behave and interact with the console:


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
