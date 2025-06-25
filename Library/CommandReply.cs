namespace Command_Interpreter
{
	/// <summary>
	/// Success Class for serielize in XML file.
	/// </summary>
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

		public string Return = string.Empty;
		public LogType Type { get; set; }
		public DateTime Timestamp { get ; set; }
		public string? FunctionCalled { get; set; }
		public string? Message { get; set; }
		public string? ThrowError { get; set; }

	}
}
