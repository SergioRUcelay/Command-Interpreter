using System.Xml.Serialization;

namespace Command_Interpreter.Aplication
{
	[Serializable, XmlRoot("Logs")]
	public class Log
	{
		[XmlElement("CommandReply")]
		public CommandReply[] logEntries { get; set; } = [];
	}
}
