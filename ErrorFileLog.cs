using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Command_Interpreter
{
    public static class ErrorFileLog
    {
        private static XmlSerializer _serializerFor_LogError_Class = new XmlSerializer(typeof(LogError));

        private readonly static string _logsDirectory = Directory.CreateDirectory("logs").ToString();
        private readonly static string _nameLogErrorFile = "logfile.xml";
        private readonly static string _logErrorFile = Path.Combine(_logsDirectory, _nameLogErrorFile);

        public static void WriteNoErrorXml()
        {
            LogError _noError = new LogError
            {
                DateTimeError = DateTime.Now,
                Notification = "Session created successfully"
            };

            using (var writer = XmlWriter.Create(_logErrorFile, new XmlWriterSettings { Indent = true, NewLineOnAttributes = true }))
                _serializerFor_LogError_Class.Serialize(writer, _noError);
        }

        public static void ErrorXmlLogFile(Exception? exception, Delegate calledfunc, string error)
        {
            LogError newError = new LogError()
            {
                ThrowError = exception?.Message,
                Notification = error,
                DelegateCalled = calledfunc.GetMethodInfo().Name.ToString(),
                DateTimeError = DateTime.Now
            };
            WriteNewErrorXml(newError);
        }

        public static void ErrorXmlLogFile(Exception? exception, string error)
        {
            LogError newError = new LogError()
            {
                ThrowError = exception?.Message,
                Notification = error,
                DateTimeError = DateTime.Now
            };
            WriteNewErrorXml(newError);
        }

        static void WriteNewErrorXml(LogError newerror)
        {
            string errorClassXmlLog;
            using (StringWriter writer = new StringWriter())
            {
                _serializerFor_LogError_Class.Serialize(writer, newerror);
                errorClassXmlLog = writer.ToString();
            }

            // Load the existing file.
            XmlDocument existingErrorFile = new XmlDocument();
            existingErrorFile.Load(_logErrorFile);

            // Instantiate a new temporal File, where we will save the new error.
            XmlDocument tempError = new XmlDocument();

            // Load the serialize XmlClass in the temporal file.
            tempError.LoadXml(errorClassXmlLog);

            // Instantiate a new Nodo.
            XmlNode xmlNewErrorNode = existingErrorFile.ImportNode(tempError.DocumentElement, true); // El método ImporNode() -> Importa un nodo de otro documento al documento actual.

            // Add the new Nodo to the log file.
            existingErrorFile?.DocumentElement?.AppendChild(xmlNewErrorNode);
            existingErrorFile?.Save(_logErrorFile);
        }
    }
}
