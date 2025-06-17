using System.Reflection;
using System.Xml.Serialization;

namespace Command_Interpreter
{
	/// <summary>
	/// Class that manages the log message in different ways. Where we collect message a success, error and problem data.
	/// </summary>
	public static class Loghandler
	{
		/// <summary>
		/// Create a serializable Xml successfully log class.
		/// </summary>
		/// <param name="calledfunc">The name of the executed function</param>
		//public static string SuccessCall(Delegate calledfunc)
		//{
		//	LogEntry success = new()
		//	{
		//		Type = LogEntry.LogType.Success,
		//		FunctionCalled = calledfunc.GetMethodInfo().Name.ToString(),
		//		Message = "Function has been executed correctly."
		//	};
		//	return WriterOfNewXmlString(success);
		//}

		///// <summary>
		///// Create a serializable Xml error log class, reporting which Function, exception and message caused it.
		///// </summary>
		///// <param name="exception">Type of exception thown</param>
		///// <param name="calledfunc">Name of the executed function</param>
		///// <param name="error">String describing the type of error</param>
		///// <returns></returns>
		//public static string ErrorXmlLog(Exception? exception, Delegate calledfunc, string error)
		//{
		//	LogEntry newError = new ()
		//	{
		//		Type = LogEntry.LogType.Error,
		//		FunctionCalled = calledfunc.GetMethodInfo().Name.ToString(),
		//		Message = error,
		//		ThrowError = exception?.Message
		//	};
		//	return WriterOfNewXmlString(newError);
		//}

		///// <summary>
		///// Create a serializable Xml error log class, reporting which exception and message caused it.
		///// </summary>
		///// <param name="exception">Type of exception thown</param>
		///// <param name="error">String describing the type of error</param>
		//public static string ErrorXmlLog(Exception? exception, string? error)
		//{
		//	LogEntry newError = new ()
		//	{
		//		Type = LogEntry.LogType.Error,
		//		Message = error,
		//		ThrowError = exception?.Message,
				
		//	};
		//	return WriterOfNewXmlString(newError);

		//}
		/// <summary>
		/// Create a Xml an error or success message when the function is called. 
		/// </summary>
		/// <param name="newxmlmessage">Serializable XmlClass that contain data.</param>
		/// <returns> A String </returns>
		public static string WriterOfNewXmlString(CommandReplay newxmlmessage)
		{
			// Declare the needed variables
			var _serializerFor_Log = new XmlSerializer(typeof(Log));
			string consoleOutput;
			Log LogContainer = new();

			StringWriter logEntryWriter = new();
			XmlSerializer _serializerFor_LogEntry = new(typeof(CommandReplay));
			_serializerFor_LogEntry.Serialize(logEntryWriter, newxmlmessage);

			// Create a log root class with a List of LogEntry objects, and update it with new XML messages.
			var updatedLogs = new List<CommandReplay>(LogContainer.logEntries) {newxmlmessage};
			LogContainer.logEntries = updatedLogs.ToArray();

			//WriteToDisk(_serializerFor_Log, LogContainer);

			using StringWriter writer = new();
			_serializerFor_Log.Serialize(writer, LogContainer);

			consoleOutput = writer.ToString();
			return consoleOutput;
		}
	}
}
