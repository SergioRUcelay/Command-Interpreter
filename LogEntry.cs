using System.Xml.Serialization;

namespace Command_Interpreter
{
	/// <summary>
	/// Success Class for serielize in XML file.
	/// </summary>
	[Serializable, XmlRoot("Logs")]
	public class Log
	{
		[XmlElement("LogEntry")]
		public LogEntry[] logEntries { get; set; } = [];
	}

	public class LogEntry
	{
		public enum LogType
		{
			Success, Error
		}

		public LogType? Type { get; set; }
		public string? Timestamp { get ; set; }
		public string? FunctionCalled { get; set; }
		public string? Message { get; set; }
		public string? ThrowError { get; set; }

	}
}
