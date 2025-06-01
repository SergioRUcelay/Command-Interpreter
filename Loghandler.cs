using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Command_Interpreter
{
    /// <summary>
    /// Class that creates the log file. Where we collect error and problem data.
    /// </summary>
    public static class Loghandler
    {
        private readonly static XmlSerializer _serializerFor_Log_ErrorClass = new (typeof(LogError));
        private readonly static XmlSerializer _serializerFor_Log_SuccessClass = new (typeof(LogSuccess));

        private readonly static string _logsDirectory = Directory.CreateDirectory("logs").ToString();
        private readonly static string _nameLogErrorFile = "logfile.xml";
        private readonly static string _logErrorFile = Path.Combine(_logsDirectory, _nameLogErrorFile);

        /// <summary>
        /// Create a serializable Xml successfully log class.
        /// </summary>
        /// <param name="calledfunc">The name of the executed function</param>
        public static string SuccessLog(Delegate calledfunc)
        {
            LogSuccess success = new()
            {
                DateTimeLog = DateTime.Now,
                DelegateCalled = calledfunc.GetMethodInfo().Name.ToString(),
                Notification = "Function had been executed correctly."
            };
            return ConsoleWriteNewXmlString(success);
        }
		/// <summary>
		/// Create a serializable Xml error log class.
		/// </summary>
		/// <param name="exception">Type of exception thown</param>
		/// <param name="calledfunc">Name of the executed function</param>
		/// <param name="error">String describing the type of error</param>
		/// <returns></returns>
		public static string ErrorXmlLog(Exception? exception, Delegate calledfunc, string error)
        {
            LogError newError = new LogError()
            {
                ThrowError = exception?.Message,
                Notification = error,
                DelegateCalled = calledfunc.GetMethodInfo().Name.ToString(),
                DateTimeError = DateTime.Now
            };
            return ConsoleWriteNewXmlString(newError);
        }
		/// <summary>
		/// Create a serializable Xml error log class.
		/// </summary>
		/// <param name="exception">Type of exception thown</param>
		/// <param name="error">String describing the type of error</param>
		public static string ErrorXmlLog(Exception? exception, string error)
        {
            LogError newError = new LogError()
            {
                ThrowError = exception?.Message,
                Notification = error,
                DateTimeError = DateTime.Now
            };
            return ConsoleWriteNewXmlString(newError);
        }
        /// <summary>
        /// Write in console a error or success menssage when the function is called.
        /// </summary>
        /// <param name="newxmlmenssage">Serializable XmlClass that contain data.</param>
        /// <returns></returns>
        public static string ConsoleWriteNewXmlString(LogError newxmlmenssage)
        {
            string menssage;
            using (StringWriter writer = new StringWriter())
            {
                _serializerFor_Log_ErrorClass.Serialize(writer, newxmlmenssage);
                menssage = writer.ToString();
				WriteToDisk(menssage);
				return menssage;
            }
        }
		/// <summary>
		/// Write in console a error or success menssage when the function is called.
		/// </summary>
		/// <param name="newxmlmenssage">Serializable XmlClass that contain data.</param>
		/// <returns></returns>
		public static string ConsoleWriteNewXmlString(LogSuccess newxmlmenssage)
        {
            string menssage;
            using (StringWriter writer = new ())
            {
                _serializerFor_Log_SuccessClass.Serialize(writer, newxmlmenssage);
                menssage = writer.ToString();
				WriteToDisk(menssage);
				return menssage;
            }
        }
        /// <summary>
        /// Writes the log menssage to a disk file.
        /// </summary>
        /// <param name="menssage">Log menssage</param>
        /// <exception cref="NullReferenceException"></exception>
        public static void WriteToDisk(string menssage)
        {
            // Load the existing file.
            XmlDocument existingErrorFile = new();
            existingErrorFile.Load(_logErrorFile);

            // Instantiate a new temporal File, where we will save the new error.
            XmlDocument tempError = new();

            // Load the serialize XmlClass in the temporal file.
            tempError.LoadXml(menssage);

            // Instantiate a new Node.
            if (tempError.DocumentElement != null)
            {
                XmlNode xmlNewErrorNode = existingErrorFile.ImportNode(tempError.DocumentElement, true); // The ImporNode() method-> Imports a Node of other file, to the current file.
                // Add the new Node to the log file.
                existingErrorFile?.DocumentElement?.AppendChild(xmlNewErrorNode);
                existingErrorFile?.Save(_logErrorFile);
            }
            //TODO: The final user must recibe a exception?
            else
            {
                throw new NullReferenceException("The LogFile don't exist");
            }
        }
    }
}
