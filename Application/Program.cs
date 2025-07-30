using Command_Interpreter;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

Commands com = new();
string _Int		= "function that adds the given numbers.";
string _String  = "Function accepting string chains";
string _Array	= "Function accepting Arrays of int";
string _Float	= "Function accepting float numbers";
string _Bool	= "Function accepting bool values";
string _IntRest = "Function subtracts two given numbers";
try
{
	//add try catch to deal with non parsable parameters
	com.AddFunc("Exit", () => Commands.terminate = true, "Quit the program");
	com.AddFunc("String", (string SQLRequest, string SQLRequest2) => SQLRequest, _String);
	com.AddFunc("Float", (float a, float b) => a + b, _Float);
	com.AddFunc("Int", (int p, int f) => p + f, _Int);
	com.AddFunc("Bool", (bool value) => value, _Bool);
	com.AddFunc("IntRest", (int p, int f) => p - f, _IntRest);
	com.AddFunc("Array", (int[] code) => code, _Array);
}
catch (FormatException ex)
{
	Console.WriteLine(ex.Message);
}

Console.ForegroundColor = ConsoleColor.DarkGreen;
Console.WriteLine("Command Interpreter\n\rVersion Alfa-0.5\n");
while (!Commands.terminate)
{
	Console.ForegroundColor = ConsoleColor.White;
	Console.Write("Ci console..:> ");
	string? verb = Console.ReadLine();
	if (verb != null)
	{
		CommandReply result = com.Command(verb);
		//XmlToText(WriterOfNewXmlString(result));
		Console.WriteLine(XmlToText(WriterOfNewXmlString(result)));
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
	return xml.Replace("\\x1b", "\x1b"); // Replace the \x1b with the escape character for color as .NET can not generate escape characters from Xslt.
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