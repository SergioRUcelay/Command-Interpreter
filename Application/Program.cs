using Command_Interpreter;
using ConsoleWeb;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

Commands com = new();
string _Int = "function that adds the given numbers.";
string _String = "Function accepting string chains";
string _Array = "Function accepting arrays of int";
string _Float = "Function accepting float numbers";
string _Bool = "Function accepting bool values";
string _IntRest = "Function subtracts two given numbers";
string _Double = "Function accepting double numbers";

try
{
	com.AddFunc("Exit", () => Ci.Terminate = true, "Quit the program");
	com.AddFunc("String", (string SQLRequest, string SQLRequest2) => SQLRequest, _String);
	com.AddFunc("Float", (float a, string b) => a + b, _Float);
	com.AddFunc("Float", (float a) => a + 100, _Float);
	com.AddFunc("ArrayFloat", (float[] a) => a[1], _Float);
	com.AddFunc("Int", (int p, int f) => p + f, _Int);
	com.AddFunc("Bool", (bool value) => value, _Bool);
	com.AddFunc("IntRest", (int p, int f) => p - f, _IntRest);
	com.AddFunc("Array", (int[] code) => code, _Array);
	com.AddFunc("test", new Func<int, int>(Ci.Test), "Add 100 to int");
	com.AddFunc("test", new Func<float, float>(Ci.Test), "Add 100 to int");
	com.AddFunc("test", new Func<int, int, int>(Ci.Test), "Add int numbers");
	com.AddFunc("test", new Func<float, int, float>(Ci.Test), "Subtracs int to float");
	com.AddFunc("double", (double d) => d, _Double);
	com.AddFunc("test", new Func<float, double, double>(Ci.Test), "Subtracs float to double");
	com.AddFunc("test", new Action(Ci.Test), "Get you a 100");
	com.AddFunc("test", new Action(Ci.Test), "Get you a 100");
	
}
catch (Exception ex)
{
	Console.WriteLine(ex.Message);
}

Console.ForegroundColor = ConsoleColor.DarkGreen;
Console.WriteLine("Command Interpreter\n\rVersion 1.0\n");
while (!Ci.Terminate)
{
	Console.ForegroundColor = ConsoleColor.White;
	Console.Write("Ci console..:> ");
	string? verb = Console.ReadLine();
	if (verb != null)
	{
		CommandReply result = com.Command(verb);
		var a = WriterOfNewXmlString(result);
		var ed = XmlToText(WriterOfNewXmlString(result));
		Console.WriteLine(ed);
	}
}

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
static string WriterOfNewXmlString<T>(T newxmlmessage)
{
	string consoleOutput;

	StringWriter logEntryWriter = new();
	XmlSerializer _serializerFor_LogEntry = new(typeof(T));
	_serializerFor_LogEntry.Serialize(logEntryWriter, newxmlmessage);
	consoleOutput = logEntryWriter.ToString();
	return consoleOutput;
}