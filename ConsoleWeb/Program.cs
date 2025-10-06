using Command_Interpreter;
using ConsoleWeb;
using System.Net.WebSockets;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

Interpreter interpreter = new();
string _Int = "function that adds the given numbers.";
string _String = "Function accepting string chains";
string _Array = "Function accepting Arrays of int";
string _Float = "Function accepting float numbers";
string _Bool = "Function accepting bool values";
string _IntRest = "Function subtracts two given numbers";


try
{

	// Registering a new parser type:
	interpreter.Parameters.RegisterCustomType(@"^(-?\d+(?:\.\d+)?)([dD])", groups => double.Parse(groups[1].Value));

	// Registration of different functions:
	interpreter.RegisterFunction("String", (string SQLRequest, string SQLRequest2) => SQLRequest, _String);
	interpreter.RegisterFunction("Float", (float a, string b) => a + b, _Float);
	interpreter.RegisterFunction("Float", (float a) => a + 100, _Float);
	interpreter.RegisterFunction("Int", (int p, int f) => p + f, _Int);
	interpreter.RegisterFunction("Bool", (bool value) => value, _Bool);
	interpreter.RegisterFunction("IntRest", (int p, int f) => p - f, _IntRest);
	interpreter.RegisterFunction("Array", (int[] code) => 0, _Array);
	interpreter.RegisterFunction("test", new Func<int, int>(Ci.Test), "Add 100 to int");
	interpreter.RegisterFunction("test", new Func<int, int, int>(Ci.Test), "Add int numbers");
	interpreter.RegisterFunction("test", new Action(Ci.Test), "Get you a 100");
	interpreter.RegisterFunction("Invalid", (double d) => d, "Function with invalid parameters");

}
catch (FormatException ex)
{
	Console.WriteLine(ex.Message);
}

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// WebSocket Activation
var webSocketOptions = new WebSocketOptions
{
	KeepAliveInterval = TimeSpan.FromSeconds(120)
};

app.UseWebSockets(webSocketOptions);

// Middleware to handle WebSocket in /ws
app.Use(async (context, next) =>
{
	if (context.Request.Path == "/ws")
	{
		if (context.WebSockets.IsWebSocketRequest)
		{
			using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
			await ReadWebSocket(webSocket, interpreter);
		}
		else
		{
			context.Response.StatusCode = 400;
		}
	}
	else
	{
		await next();
	}
});
app.Run();

// Method for handling the Byte string that reaches us through the webSocket
static async Task ReadWebSocket(WebSocket socket, Interpreter com)
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
		string response = XmlToText(xmlOutput);// Response string
		byte[] responseBytes = Encoding.UTF8.GetBytes(response); // Conversion of String to Array of Bytes
		await socket.SendAsync(new ArraySegment<byte>(responseBytes), WebSocketMessageType.Text, true, CancellationToken.None); // Enviamos la respuesta.
	}
}

// XSLT XML Conversion into flat text, to show in console.
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