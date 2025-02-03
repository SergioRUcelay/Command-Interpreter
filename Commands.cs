using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Command_Interpreter
{
    internal class Commands
    {
        public static bool terminate = false;

        public readonly List<(string name, Delegate func, string info)> tuple_commands = [];
        private LogError _noError;
        private Delegate _CalledFunc;
        private XmlSerializer _serializerFor_LogError_Class;
        private readonly string _logsDirectory;
        private readonly string _logErrorFile;
        private readonly string _nameLogErrorFile;

        private readonly string list = "Function for list all functions of all program";
        private readonly string help = "The help text of the Command Interpreter";

        public Commands()
        {

            tuple_commands.Add(("List", List, list));
            tuple_commands.Add(("Help", Help, help));

            _logsDirectory = Directory.CreateDirectory("logs").ToString();
            _serializerFor_LogError_Class = new XmlSerializer(typeof(LogError));
            _nameLogErrorFile = "logfile.xml";
            _logErrorFile = Path.Combine(_logsDirectory, _nameLogErrorFile);

            _noError = new LogError
            {
                DateTimeError = DateTime.Now,
                Error = "Session created successfully"
            };

            WriteNoErrorXml(_noError);
        }

        /// <summary>
        /// Retrieves the array on the command line and sorts the Delegate and its parameters.
        /// </summary>
        /// <param name="textConsole"></param>
        public void Command(string[] textConsole)
        {
            if (textConsole.Length == 0)
                return;
            try
            {
                // Seeking the function.
                string command = textConsole[0].ToLower();
                //Delegate delegateFind = tuple_commands.Find(a => a.name.ToLower() == command).func;
                _CalledFunc = tuple_commands.Find(func => func.name.ToLower() == command).func;

                // Seeking parameters. In this case: Int, Float, String, Bool and Array.
                if (_CalledFunc != null)
                {
                    MethodInfo methodInfo = _CalledFunc.GetMethodInfo();
                    List<object> ps = [];
                    int currentToken = 1;
                    foreach (var param in methodInfo.GetParameters())
                    {
                        if (param.ParameterType == typeof(int))
                            ps.Add(int.Parse(textConsole[currentToken++]));
                        else if (param.ParameterType == typeof(float))
                            ps.Add(float.Parse(textConsole[currentToken++]));
                        else if (param.ParameterType == typeof(string))
                        {
                            //.       if (textConsole[currentToken].Contains('-'))
                            //        ps.Add(string.Join(" ", textConsole[currentToken++].Trim('-')));
                            ps.Add(string.Join(" ", textConsole[currentToken++]));
                        }
                        else if (param.ParameterType == typeof(bool))
                            ps.Add(bool.Parse(textConsole[currentToken++]));
                        else if (param.ParameterType.IsArray)
                        {
                            string newtype = "";
                            foreach (var item in textConsole[currentToken])
                            {
                                if (item != '[' && item != ']')
                                {
                                    newtype += item;
                                }
                            }

                            var newarray = newtype.Split(',');

                            if (param.ParameterType == typeof(int[]))
                            {
                                List<int> ints = [];
                                for (int i = 0; i < (newarray.Length); i++)
                                {
                                    if (param.ParameterType == typeof(int[]))
                                        ints.Add(int.Parse(newarray[i]));
                                }
                                ps.Add(ints.ToArray());

                            }

                            if (param.ParameterType == typeof(float[]))
                            {
                                List<float> floats = [];
                                for (int i = 0; i < (newarray.Length); i++)
                                {
                                    if (param.ParameterType == typeof(float[]))
                                        floats.Add(float.Parse(newarray[i]));
                                }
                                ps.Add(floats.ToArray());
                            }
                        }
                    }
                    if (methodInfo.GetParameters().Length == textConsole.Length - 1)
                    {
                        var ret = methodInfo.Invoke(_CalledFunc.Target, ps.ToArray());
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" Too many arguments. The ");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(textConsole[0].ToString());
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" function only supports ");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write((methodInfo.GetParameters().Length).ToString());
                        Console.WriteLine();
                        ErrorXmlLogFile(null, "Too many arguments");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Command not found");
                    Console.ResetColor();
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Sorry, incomplete command! Function arguments required.");
                ErrorXmlLogFile(ex, "Function argument missing");
            }
            catch (FormatException fo)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Sorry, incorrect type format!");
                ErrorXmlLogFile(fo, "Invalid argument");
         
            }
            catch (NullReferenceException refr)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Sorry, incorrect type format!");
                ErrorXmlLogFile(refr, "Fuction dosen't exist");
            }
            catch (TargetParameterCountException target)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Sorry, incorrect type format!.");
                ErrorXmlLogFile(target, "Function dosen't exist");
            }
        }

        public void AddFunc(string command, Delegate func, string info)
        {
            if (tuple_commands.Exists(x => x.name == command))
                throw new InvalidOperationException($"Function with name {command} already registered");
            else
                tuple_commands.Add((command, func, info));
        }

        public void RemoveFunc(string name)
        {
            if (!tuple_commands.Exists(x => x.name.ToLower() == name.ToLower()))
            {
                //throw new InvalidOperationException($"Function with name {name} don't exist");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($" Function with name {name} don't exist");
                Console.ResetColor();

            }
            else
            {
                var index = tuple_commands.FindIndex(y => y.name.ToLower() == name.ToLower());
                tuple_commands.Remove(tuple_commands[index]);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($" Function with name {name} has been removed");
                Console.ResetColor();
            }
        }

        public void List()
        {
            int maxW = tuple_commands.Max(command => command.name.Length);

            foreach (var command in tuple_commands)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"   " + command.name.PadRight(maxW));
                Console.ResetColor();
                Console.WriteLine($" - " + command.info);
            }
        }

        public void Help()
        {
            List();
            Console.WriteLine("Here the help text of the Command Interpreter");
        }

        private void WriteNoErrorXml(LogError noerror)
        {
            using (var writer = XmlWriter.Create(_logErrorFile, new XmlWriterSettings { Indent = true, NewLineOnAttributes = true }))
                _serializerFor_LogError_Class.Serialize(writer, noerror);
        }

        public void ErrorXmlLogFile(Exception? exception, string error)
        {
            LogError newError = new LogError()
            {
                ThrowError = exception?.Message,
                Error = error,
                DelegateCalled = _CalledFunc.GetMethodInfo().Name.ToString(),
                DateTimeError = DateTime.Now
            };
            WriteNewErrorXml(newError);
        }

        private void WriteNewErrorXml(LogError newerror)
        {
            string errorClassXmlLog;
            using (StringWriter writer = new StringWriter())
            {
                _serializerFor_LogError_Class.Serialize(writer, newerror);
                errorClassXmlLog = writer.ToString();
            }

            // Cargamos el archivo de log que ya tenemos.
            // Load the existing file.
            XmlDocument existingErrorFile = new XmlDocument(); 
            existingErrorFile.Load(_logErrorFile);

            // Instanciamos el nuevo "archivo temporal" donde vamos a guardar el nuevo error
            // Instantiate a new temporal File, where we will save the new error.
            XmlDocument tempError = new XmlDocument();
            
            // Cargamos la clase serializada en Xml en el archivo temporal.
            // Load the serialize XmlClass in the temporal file.
            tempError.LoadXml(errorClassXmlLog);
            //temperror.LoadXml(writer.ToString());

            // Instanciamos un nuevo Nodo.
            // Instantiate a new Nodo.
            XmlNode xmlNewErrorNode = existingErrorFile.ImportNode(tempError.DocumentElement, true); // El método ImporNode() -> Importa un nodo de otro documento al documento actual.

            // Añadimos el nodo creado al archivo existente.
            // Add the new Nodo to the log file.
            existingErrorFile?.DocumentElement?.AppendChild(xmlNewErrorNode);
            existingErrorFile?.Save(_logErrorFile);
        }

    }
}
