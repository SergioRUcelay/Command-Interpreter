using System.Reflection;

namespace Command_Interpreter
{
    public class Commands
    {
        public static bool terminate = false;

        private readonly List<(string name, Delegate func, string info)> tuple_commands = [];

        private Delegate _CalledFunc;
        private Parameters _parameters;

        private readonly string list = "Function for list all functions of all program";
        private readonly string help = "The help text of the Command Interpreter";

        public Commands()
        {
            tuple_commands.Add(("List", List, list));
            tuple_commands.Add(("Help", Help, help));

            ErrorFileLog.WriteNoErrorXml();
        }

        /// <summary>
        /// Retrieves the array on the command line and sorts the Delegate and its parameters.
        /// </summary>
        /// <param name="textConsole"></param>
        public void Command(string[] textConsole)
        {
            if (textConsole.Length == 0)
                return;
            //try
            //{
            // Seeking the function.
            string command = textConsole[0].ToLower();
            _CalledFunc = tuple_commands.Find(func => func.name.ToLower() == command).func;

            // Seeking parameters. In this case: Int, Float, String, Bool and Array.
            if (_CalledFunc != null)
            {
                MethodInfo methodInfo = _CalledFunc.GetMethodInfo();
                _parameters = new Parameters(methodInfo, textConsole);

                if (methodInfo.GetParameters().Length == textConsole.Length - 1)
                {
                    var function = methodInfo.Invoke(_CalledFunc.Target, _parameters.SeekParams());
                }
                else if (methodInfo.GetParameters().Length < textConsole.Length - 1)
                {
                    ErrorFileLog.ErrorXmlLogFile(null, _CalledFunc, "The number of arguments is less than necessary");
#if DEBUG
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" Too many arguments\n");
#endif
                }
                else
                {
                    ErrorFileLog.ErrorXmlLogFile(null, _CalledFunc, "The number of arguments is higher than necessary");
#if DEBUG
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" Few arguments\n");
#endif
                }
            }

            else
            {
#if DEBUG
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Command does not exist");
#endif
                ErrorFileLog.ErrorXmlLogFile(null, "The string does not represent any function");
            }
            //}

            // This would be called when the arguments do not exist.
            //catch (IndexOutOfRangeException notexist)
            //{
            //    Console.ForegroundColor = ConsoleColor.Red;
            //    Console.WriteLine(" Sorry, incomplete command! Function arguments required.");
            //    ErrorXmlLogFile(notexist, "Function argument wrong, missing or incompleted");
            //}

            // This would be called when the types of arguments are not correct.
            //catch (FormatException correctarguments)
            //{
            //    Console.ForegroundColor = ConsoleColor.Red;
            //    Console.WriteLine("Sorry, incorrect type format!");
            //    ErrorXmlLogFile(correctarguments, "The type of arguments was wrong");

            //}

            // Null referent?
            //catch (NullReferenceException nullreference)
            //{
            //    Console.ForegroundColor = ConsoleColor.Red;
            //    Console.WriteLine(" 2 - Sorry, incorrect type format!");
            //    ErrorXmlLogFile(nullreference, "Fuction dosen't exist");
            //}

            // This would be called when the strings arguments are not correct.
            //catch (TargetParameterCountException stringsformats)
            //{
            //    Console.ForegroundColor = ConsoleColor.Red;
            //    Console.WriteLine("Sorry, incorrect strings type format!.");
            //    ErrorXmlLogFile(stringsformats, "The string parameter needs '-' as a prefix");
            //}
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
                throw new InvalidOperationException($"Function with name {name} don't exist");
            else
            {
                var index = tuple_commands.FindIndex(y => y.name.ToLower() == name.ToLower());
                tuple_commands.Remove(tuple_commands[index]);
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

    }
}
