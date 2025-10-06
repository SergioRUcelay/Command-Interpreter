using Command_Interpreter;
using ConsoleWeb;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

Interpreter interpreter = new();
string _Int = "function that adds the given numbers.";
string _String = "Function accepting string chains";
string _Array = "Function accepting arrays of int";
string _Float = "Function accepting float numbers";
string _Bool = "Function accepting bool values";
string _Double = "Function accepting double numbers";

try
{
	// Registering a new parser type:
	interpreter.Parameters.RegisterCustomType(@"^(-?\d+(?:\.\d+)?)([dD])", groups => double.Parse(groups[1].Value));

	// Registration of different functions:
	interpreter.RegisterFunction("Exit", () => Ci.Terminate = true, "Quit the program");
	interpreter.RegisterFunction("Int", (int p, int f) => p + f, _Int);
	interpreter.RegisterFunction("String", (string SQLRequest, string SQLRequest2) => SQLRequest, _String);
	interpreter.RegisterFunction("Bool", (bool value) => value, _Bool);
	interpreter.RegisterFunction("Float", (float a, float b) => a + b, _Float);
	interpreter.RegisterFunction("double", (double d, double c) => d + c, _Double);
	interpreter.RegisterFunction("Arratesy", (int[] code) => code, _Array);
	interpreter.RegisterFunction("test", new Func<int, int>(Ci.Test), "Add 10 to int");
	interpreter.RegisterFunction("test", new Func<float, float>(Ci.Test), "Add 100 to int");
	interpreter.RegisterFunction("test", new Func<int, int, int>(Ci.Test), "Add int numbers");
	interpreter.RegisterFunction("test", new Func<float, int, float>(Ci.Test), "Subtracs int to float");
	interpreter.RegisterFunction("test", new Func<float, double, double>(Ci.Test), "Subtracs float to double");
	interpreter.RegisterFunction("test", new Action(Ci.Test), "Get you a 100");

}
catch (Exception ex)
{
	Console.WriteLine(ex.Message);
}


// Loop to listen to the console.
Console.ForegroundColor = ConsoleColor.DarkGreen;
Console.WriteLine("Command Interpreter\n\rVersion 1.0\n");
while (!Ci.Terminate)
{
	Console.ForegroundColor = ConsoleColor.White;
	Console.Write("Ci console..:> ");
	string? verb = Console.ReadLine();
	if (verb != null)
	{
		CommandReply result = interpreter.Command(verb);
		var a = WriterOfNewXmlString(result);
		var ed = XmlToText(WriterOfNewXmlString(result));
		Console.WriteLine(ed);
	}
}

// XSLT XML Conversion into flat text, to show in console.
static string XmlToText(string xml)
{
	XslCompiledTransform xslTranslater = new();
	var xslFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XSLTFile.xslt");
	xslTranslater.Load(xslFile);
	using (StringWriter texOutput = new())
	{
		using XmlReader xmlMemory = XmlReader.Create(new StringReader(xml));
		xslTranslater.Transform(xmlMemory, null, texOutput);
		xml = texOutput.ToString();
	}
	return xml.Replace("\\x1B", "\x1b"); // Replace the \x1b with the escape character for color as .NET can not generate escape characters from Xslt.
}

// Serializable class conversion returned by CI, in XML format.
static string WriterOfNewXmlString<T>(T newxmlmessage)
{
	string consoleOutput;

	StringWriter logEntryWriter = new();
	XmlSerializer _serializerFor_LogEntry = new(typeof(T));
	_serializerFor_LogEntry.Serialize(logEntryWriter, newxmlmessage);
	consoleOutput = logEntryWriter.ToString();
	return consoleOutput;
}