using System.Xml.Serialization;

namespace Command_Interpreter
{

	/// <summary>
	/// Represents the result of a command execution, including its status, return value, and additional metadata.
	/// </summary>
	/// <remarks>This class provides information about the outcome of a command, such as whether it succeeded or
	/// failed, the time the command was executed, and any associated messages or errors.It can also include a return
	/// value from the command, if applicable.</remarks>
	[XmlInclude(typeof(FuncList))]
	public class CommandReply
	{
		public enum LogType
		{
			Success, Error, Void
		}
		public CommandReply()
		{
			Timestamp = DateTime.Now;
		}

		public object? Return = null;
		public LogType Type { get; set; }
		public DateTime Timestamp { get ; set; }
		public string? FunctionCalled { get; set; }
		public string? Message { get; set; }
		public string? ThrowError { get; set; }

	}

	public struct FunctionEntry
	{
		public string Function = string.Empty;
		public string Description = string.Empty;

		// Constructor to initialize Function and Description.
		public FunctionEntry(string function, string description)
		{
			Function = function;
			Description = description;
		}
	}
	public class FuncList
	{
		public List<FunctionEntry> Entries  = new();
	}
}
