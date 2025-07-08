using Command_Interpreter;
using Command_Interpreter.Aplication;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

Commands com = new();
string _Int			= "Function accepting int numbers";
string _String		= "Function accepting string chains";
string _Array		= "Function accepting Arrays of int";
string _Float		= "Function accepting float numbers";
string _Bool		= "Function accepting bool values";
string _IntRest		= "Function resta dos numeros dados";
string _DoubleRest	= "Function resta dos numeros dados";

try
{
	//add try catch to deal with non parsable parameters
	com.AddFunc("Exit", () => Commands.terminate = true, "Quit the program");
	com.AddFunc("String", Ci.String, _String);
	com.AddFunc("Float", Ci.Float, _Float);
	com.AddFunc("Int", Ci.Int, _Int);
	com.AddFunc("Bool", Ci.Bool, _Bool);
	com.AddFunc("IntRest", Ci.IntRest, _IntRest);
	com.AddFunc("Array", Ci.Array, _Array);
	com.AddFunc("Double", Ci.NoValid, _DoubleRest);
}
catch (FormatException ex)
{
	Console.WriteLine(ex.Message);
}


HttpListener listener = new HttpListener(); // I create a listener to be able to listen to what comes through the port.
listener.Prefixes.Add("http://localhost:7000/");// I configure the port I want to use.
listener.Start();// I initialize the listening
Console.WriteLine("Websocket server started at ws://localhost:7000/");

while (true)
{
	HttpListenerContext context = await listener.GetContextAsync(); // I store in "context" what arrives through the listener.
	if (context.Request.IsWebSocketRequest)// If there is a request.
	{
		HttpListenerWebSocketContext wsConstext = await context.AcceptWebSocketAsync(null); // We access the Listener information
																							// We put it in a separate Task. This way, we don't lose responses (if we put an await, we might lose it).
		Task.Run(() => ReadWebSocket(wsConstext.WebSocket, com));
	}
	else // We only expect websocket requests. We return an error for non-websocket requests.
	{
		context.Response.StatusCode = 400;
		context.Response.Close();
	}
}


// Method for handling the Byte string that reaches us through the webSocket
static async Task ReadWebSocket(WebSocket socket, Commands com)
{
	byte[] buffer = new byte[10234]; // Creating the byte array where we will save the request.
	while (socket.State == WebSocketState.Open) // As long as the websocket is open.
	{
		// We save in 'result' what we receive from the WebSocket, as an Array segment of what is in the 'buffer'
		WebSocketReceiveResult webCommand = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
		string command = Encoding.UTF8.GetString(buffer, 0, webCommand.Count);// We encode a String in UTF8

		CommandReply result = com.Command(command);
		Console.WriteLine($"Message received: {command}");
		
		string xmlOutput = WriterOfNewXmlString(result);
		string response =  XmlToText(xmlOutput);// Response string
		byte[] responseBytes = Encoding.UTF8.GetBytes(response); // Conversion of String to Array of Bytes
		await socket.SendAsync(new ArraySegment<byte>(responseBytes), WebSocketMessageType.Text, true, CancellationToken.None); // Enviamos la respuesta.
	}
}

//TODO: Write a unit test for a good call and bad call.
//Create a function for create a new suported type. Like Double or Vector2.

//Console.ForegroundColor = ConsoleColor.DarkGreen;
//Console.WriteLine("Command Interpreter\n\rVersion Alfa-0.5\n");
//while (!Commands.terminate)
//{
//	Console.ForegroundColor = ConsoleColor.White;
//	Console.Write("Ci console..:> ");
//	string? verb = Console.ReadLine();
//	if (verb != null)
//	{
//		CommandReply result = com.Command(verb);
//		//XmlToText(WriterOfNewXmlString(result));
//		Console.WriteLine(XmlToText(WriterOfNewXmlString(result)));
//	}
//}

static string XmlToText(string xml)
{
	XslCompiledTransform xslTranslater = new();
	var xslFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XSL_HTMLTFile.xslt");
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
	// Declare the needed variables
	//var _serializerFor_Log = new XmlSerializer(typeof(Log));
	string consoleOutput;
	//Log LogContainer = new();

	StringWriter logEntryWriter = new();
	XmlSerializer _serializerFor_LogEntry = new(typeof(T));
	//try
	//{
		_serializerFor_LogEntry.Serialize(logEntryWriter, newxmlmessage);
	//}
	//catch (Exception ex)
	//{
	//	Console.WriteLine(ex.ToString());
	//}

	// Create a log root class with a List of LogEntry objects, and update it with new XML messages.
//	var updatedLogs = new List<T>(LogContainer.logEntries) { newxmlmessage };
	//LogContainer.logEntries = updatedLogs.ToArray();

	//WriteToDisk(_serializerFor_Log, LogContainer);

	//using StringWriter writer = new();
	//_serializerFor_Log.Serialize(writer, LogContainer);

	consoleOutput = logEntryWriter.ToString();
	return consoleOutput;
}