using System.Xml.Serialization;

namespace Command_Interpreter
{
	/// <summary>
	/// Success Class for serielize in XML file.
	/// </summary>
	[Serializable, XmlRoot("Success")]
	public class LogSuccess
	{
		public DateTime DateTimeLog { get; set; }
		public string? DelegateCalled { get; set; }
		public string? Notification { get; set; }

	}
}
