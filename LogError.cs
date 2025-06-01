using System.Xml.Serialization;

namespace Command_Interpreter
{
    /// <summary>
    /// Error Class for serielize in XML file.
    /// </summary>
    [Serializable, XmlRoot("Error")]
    public class LogError
    {
        public DateTime DateTimeError { get; set; }
        public string? DelegateCalled { get; set; }
        public string? Notification { get; set; }
        public string? ThrowError { get; set; }
    }
}
