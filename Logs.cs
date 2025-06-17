using System.Xml.Serialization;

namespace Command_Interpreter
{
	[Serializable, XmlRoot("Logs")]
	public class Log
	{
		[XmlElement("CommandReplay")]
		public CommandReplay[] logEntries { get; set; } = [];
	}
}
