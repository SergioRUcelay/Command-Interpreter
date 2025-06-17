using Command_Interpreter;
using System.Xml;
using System.Xml.Xsl;

Commands com = new();
string _Int = "Function accepting int numbers";
string _String = "Function accepting string chains";
string _Array = "Function accepting Arrays of int";
string _Float = "Function accepting float numbers";
string _Bool = "Function accepting bool values";
string _IntRest = "Function resta dos numeros dados";
string _DoubleRest = "Function resta dos numeros dados";

try
{
    //add try catch to deal with non parsable parameters
    com.AddFunc("Exit", () => Commands.terminate = true, "Quit the program");
    com.AddFunc("String", Ci.String,_String);
    //com.AddFunc("Array", Array, Array);
    com.AddFunc("Float", Ci.Float, _Float);
    com.AddFunc("Int", Ci.Int, _Int);
    com.AddFunc("Bool", Ci.Bool, _Bool);
    com.AddFunc("inRest", Ci.IntRest, _IntRest);
    com.AddFunc("Double", Ci.NoValid, _DoubleRest);
    //com.AddFunc("Array", Ci.Array, Array);

}
catch (FormatException ex)
{
	Console.WriteLine(ex.Message);
}

// TODO: Write a unit test for a good call and bad call.
// Create a function for create a new suported type. Like Double or Vector2.

Console.ForegroundColor = ConsoleColor.DarkGreen;
Console.WriteLine("Command Interpreter\n\rVersion Alfa-0.5\n");
while (!Commands.terminate)
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("Ci console..:> ");
	string? verb = Console.ReadLine();
    if (verb != null)
    {
        CommandReplay result = com.Command(verb);
        //XmlToText(result);
        Console.WriteLine(Loghandler.WriterOfNewXmlString(result));
	}
}
/// <summary>  
/// <BR></BR>  
static void  XmlToText(string xml)
{
	XslCompiledTransform xslTranslater = new();
	var xslFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Aplication", "XSLTFile.xslt");
	xslTranslater.Load(xslFile);
	using (StringWriter texOutput = new())
	{
		using XmlReader xmlMemory = XmlReader.Create(new StringReader(xml));
		xslTranslater.Transform(xmlMemory, null, texOutput);
		xml = texOutput.ToString();
	}
	Console.WriteLine(xml.Replace("\\x1b", "\x1b")); // Replace the \x1b with the escape character for color as .NET can not generate escape characters from Xslt.
}

// TODO: Eliminate the two log class and merge in one.
// Handle the sintaxis error like: int 2 2/ (that will throw an error)
// look for other type of Exception that could exist.