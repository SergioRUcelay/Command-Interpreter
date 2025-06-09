using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace Command_Interpreter
{
	/// <summary>
	/// Class that creates the log file. Where we collect menssage a success, error and problem data.
	/// </summary>
	public static class Loghandler
	{
		private readonly static string _logsDirectory;
		private readonly static string _nameLogFile;
		private readonly static string _logFile;
		
		static Loghandler()
		{
			_logsDirectory = Directory.CreateDirectory("logs").ToString();
			_nameLogFile = $"logfile_{DateTime.Now:dd_MM_yyy_hh'h'_mm'm'}.xml";
			_logFile = Path.Combine(_logsDirectory, _nameLogFile);
		}

		/// <summary>
		/// Create a serializable Xml successfully log class.
		/// </summary>
		public static void SuccessLog()
		{
			LogEntry success = new()
			{
				Type = LogEntry.LogType.Success,
				Timestamp = DateTime.Now.ToString("dd/MM/yyyy HH'h':mm'm':ss'sg'"),
				Message = "Session successfully started."
				
			};
			ConsoleWriteNewXmlString(success);
		}

		/// <summary>
		/// Create a serializable Xml successfully log class.
		/// </summary>
		/// <param name="calledfunc">The name of the executed function</param>
		public static string SuccessCall(Delegate calledfunc)
		{
			LogEntry success = new()
			{
				Type = LogEntry.LogType.Success,
				Timestamp = DateTime.Now.ToString("dd/MM/yyyy HH'h':mm'm':ss'sg'"),
				FunctionCalled = calledfunc.GetMethodInfo().Name.ToString(),
				Message = "Function has been executed correctly."
			};
			return ConsoleWriteNewXmlString(success);
		}

		/// <summary>
		/// Create a serializable Xml error log class, reporting which Function, exception and menssage caused it.
		/// </summary>
		/// <param name="exception">Type of exception thown</param>
		/// <param name="calledfunc">Name of the executed function</param>
		/// <param name="error">String describing the type of error</param>
		/// <returns></returns>
		public static string ErrorXmlLog(Exception? exception, Delegate calledfunc, string error)
		{
			LogEntry newError = new ()
			{
				Type = LogEntry.LogType.Error,
				Timestamp = DateTime.Now.ToString("dd/MM/yyyy HH'h':mm'm':ss'sg'"),
				FunctionCalled = calledfunc.GetMethodInfo().Name.ToString(),
				Message = error,
				ThrowError = exception?.Message
			};
			return ConsoleWriteNewXmlString(newError);
		}

		/// <summary>
		/// Create a serializable Xml error log class, reporting which exception and menssage caused it.
		/// </summary>
		/// <param name="exception">Type of exception thown</param>
		/// <param name="error">String describing the type of error</param>
		public static string ErrorXmlLog(Exception? exception, string? error)
		{
			LogEntry newError = new ()
			{
				Type = LogEntry.LogType.Error,
				Timestamp = DateTime.Now.ToString("dd/MM/yyyy HH'h':mm'm':ss'sg'"),
				Message = error,
				ThrowError = exception?.Message,
				
			};
			return ConsoleWriteNewXmlString(newError);

		}
		/// <summary>
		/// Create a Xml an error or success menssage when the function is called. 
		/// </summary>
		/// <param name="newxmlmenssage">Serializable XmlClass that contain data.</param>
		/// <returns></returns>
		public static string ConsoleWriteNewXmlString(LogEntry newxmlmenssage)
		{
			// Declare the needed variables
			var _serializerFor_Log = new XmlSerializer(typeof(Log));
			XslCompiledTransform xslTranslater = new();
			var xslFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XSLTFile1.xslt");
			string consoleOutput;
			Log LogContainer;

			
			if (File.Exists(_logFile))
			{
				using FileStream localXmlFile = new(_logFile, FileMode.Open);
				LogContainer = _serializerFor_Log.Deserialize(localXmlFile) as Log ?? new Log(); // Recover a file into log's variable or if not exist it'll create it.
			}
			else
			{
				LogContainer = new Log();
			}

			// Create a log root class with a List of LogEntry objects, and update it with new XML messages.
			var updatedLogs = new List<LogEntry>(LogContainer.logEntries) {newxmlmenssage};
			LogContainer.logEntries = updatedLogs.ToArray();

			//WriteToDisk(_serializerFor_Log, LogContainer);

			using StringWriter writer = new();
			_serializerFor_Log.Serialize(writer, LogContainer);

			// Loads from memory to store new XmlLog in memory throw a variable.
			using XmlReader xmlMemory = XmlReader.Create(new StringReader(writer.ToString()));
			// Loads the Xlst file for translate.
			xslTranslater.Load(xslFile);
			using (StringWriter texOutput = new())
			{
				xslTranslater.Transform(xmlMemory,null, texOutput);
				consoleOutput = texOutput.ToString();

			}
			//return menssage;
			return consoleOutput;
			
		}

		/// <summary>
		/// Writes the log success menssage file to disk.
		/// </summary>
		/// <param name="log">Serializable class required to write the file</param>
		/// <param name="menssage">String representing the type of error</param>
		//public static void WriteToDisk(XmlSerializer serializer, Log logE)
		//{

		//	if (!File.Exists(_logFile))
		//	{
		//		var _serializerFor_Log = new XmlSerializer(logE.GetType());
		//		using var writer = XmlWriter.Create(_logFile, new XmlWriterSettings { Indent = true, NewLineOnAttributes = true });
		//		_serializerFor_Log.Serialize(writer, logE);
		//	}
		//	XmlFop
		//	else
		//		AddToAnExistingFile(menssage);
		//}

		/// <summary>
		/// Adds log information to an existing file.
		/// </summary>
		/// <param name="menssage">String representing the type of error</param>
		public static void AddToAnExistingFile(string menssage)
		{
			
			// Load the existing file.
			XmlDocument existingLogFile = new();
			existingLogFile.Load(_logFile);

			// Instantiate a new temporal File, where we will save the new error.
			XmlDocument tempLog = new();

			// Load the serialize XmlClass in the temporal file.
			tempLog.LoadXml(menssage);

			// Instantiate a new Node.
			if (tempLog.DocumentElement != null)
			{
				XmlNode xmlNewLogNode = existingLogFile.ImportNode(tempLog.DocumentElement, true); // The ImporNode() method-> Imports a Node of other file, to the current file.
																								   // Add the new Node to the log file.
				//XmlNode? xmlOldLogNode = existingLogFile.SelectSingleNode("/Logs");
				existingLogFile?.DocumentElement?.AppendChild(xmlNewLogNode);
				//xmlOldLogNode.AppendChild(xmlOldLogNode.FirstChild);
				existingLogFile?.Save(_logFile);
			}
		}
		// TODO: Fix how add diferent nodes to file. Now it is added inside the first Node, usually <Success>
		
    }
}
