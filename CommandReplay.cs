namespace Command_Interpreter
{
	/// <summary>
	/// Success Class for serielize in XML file.
	/// </summary>
	public class CommandReplay
	{
		public enum LogType
		{
			Success, Error, Void
		}
		public CommandReplay()
		{
			Timestamp = DateTime.Now;
		}

		public LogType Type { get; set; }
		public DateTime Timestamp { get ; set; }
		public string? FunctionCalled { get; set; }
		public string? Message { get; set; }
		public string? ThrowError { get; set; }

	}
}
