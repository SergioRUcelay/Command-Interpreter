using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Command_Interpreter
{
	/// <summary>
	/// Class that creates the log file. Where we collect error and problem data.
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
		/// <param name="calledfunc">The name of the executed function</param>
		public static void SuccessLog()
		{
			LogSuccess success = new()
			{
				DateTimeLog = DateTime.Now,
				Notification = "Session successfully started."
			}; 
			ConsoleWriteNewXmlString(success);
		}

		/// <summary>
		/// Create a serializable Xml successfully log class.
		/// </summary>
		/// <param name="calledfunc">The name of the executed function</param>
		public static string SuccessCall(Delegate? calledfunc)
		{
			LogSuccess success = new()
			{
				DateTimeLog = DateTime.Now,
				DelegateCalled = calledfunc.GetMethodInfo().Name.ToString(),
				Notification = "Function had been executed correctly."
			};
			
			return ConsoleWriteNewXmlString(success);
		}

		/// <summary>
		/// Create a serializable Xml error log class.
		/// </summary>
		/// <param name="exception">Type of exception thown</param>
		/// <param name="calledfunc">Name of the executed function</param>
		/// <param name="error">String describing the type of error</param>
		/// <returns></returns>
		public static string ErrorXmlLog(Exception? exception, Delegate calledfunc, string error)
		{
			LogError newError = new LogError()
			{
				ThrowError = exception?.Message,
				Notification = error,
				DelegateCalled = calledfunc.GetMethodInfo().Name.ToString(),
				DateTimeError = DateTime.Now
			};
			return ConsoleWriteNewXmlString(newError);
		}

		/// <summary>
		/// Create a serializable Xml error log class.
		/// </summary>
		/// <param name="exception">Type of exception thown</param>
		/// <param name="error">String describing the type of error</param>
		public static string ErrorXmlLog(Exception? exception, string error)
		{
			LogError newError = new LogError()
			{
				ThrowError = exception?.Message,
				Notification = error,
				DateTimeError = DateTime.Now
			};
			Console.WriteLine(error);
			return ConsoleWriteNewXmlString(newError);

		}
		/// <summary>
		/// Write in console a error or success menssage when the function is called.
		/// </summary>
		/// <param name="newxmlmenssage">Serializable XmlClass that contain data.</param>
		/// <returns></returns>
		public static string ConsoleWriteNewXmlString(object newxmlmenssage)
		{
			string menssage;
			using (StringWriter writer = new StringWriter())
			{
				var _serializerFor_Log = new XmlSerializer(newxmlmenssage.GetType());
				_serializerFor_Log.Serialize(writer, newxmlmenssage);
				menssage = writer.ToString();
				WriteToDisk(newxmlmenssage, menssage);
				return menssage;
			}
		}

		/// <summary>
		/// Writes the log success menssage file to disk.
		/// </summary>
		/// <param name="log">Serializable class required to write the file</param>
		/// <param name="menssage">String representing the type of error</param>
		public static void WriteToDisk(object log, string menssage)
		{
			if (!File.Exists(_logFile))
			{
				var _serializerFor_Log = new XmlSerializer(log.GetType());
				using (var writer = XmlWriter.Create(_logFile, new XmlWriterSettings { Indent = true, NewLineOnAttributes = true }))
					_serializerFor_Log.Serialize(writer, log);
			}
			else
				AddToAnExistingFile(menssage);
		}

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
				existingLogFile?.DocumentElement?.AppendChild(xmlNewLogNode);
				existingLogFile?.Save(_logFile);
			}
		}
		// TODO: Fix how add diferent nodes to file. Now it is added inside the first Node, usually <Success>
		
    }
}
