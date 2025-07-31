using Command_Interpreter;
using System.Net.WebSockets;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

Commands com = new();
string _Int		= "function that adds the given numbers.";
string _String	= "Function accepting string chains";
string _Array	= "Function accepting Arrays of int";
string _Float	= "Function accepting float numbers";
string _Bool	= "Function accepting bool values";
string _IntRest = "Function subtracts two given numbers";

try
{
	com.AddFunc("Exit", () => Commands.terminate = true, "Quit the program");
	com.AddFunc("String", (string SQLRequest, string SQLRequest2) => SQLRequest, _String);
	com.AddFunc("Float", (float a, float b) => a + b, _Float);
	com.AddFunc("Int", (int p, int f) => p + f, _Int);
	com.AddFunc("Bool", (bool value) => value, _Bool);
	com.AddFunc("IntRest", (int p, int f) => p - f, _IntRest);
	com.AddFunc("Array", (int[] code) => code, _Array);
	com.AddFunc("Invalid", (double d) => d, "Function with invalid parameters");
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
			await ReadWebSocket(webSocket, com);
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
		string response = XmlToText(xmlOutput);// Response string
		byte[] responseBytes = Encoding.UTF8.GetBytes(response); // Conversion of String to Array of Bytes
		await socket.SendAsync(new ArraySegment<byte>(responseBytes), WebSocketMessageType.Text, true, CancellationToken.None); // Enviamos la respuesta.
	}
}
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
	string consoleOutput;

	StringWriter logEntryWriter = new();
	XmlSerializer _serializerFor_LogEntry = new(typeof(T));
	_serializerFor_LogEntry.Serialize(logEntryWriter, newxmlmessage);
	consoleOutput = logEntryWriter.ToString();
	return consoleOutput;
}
