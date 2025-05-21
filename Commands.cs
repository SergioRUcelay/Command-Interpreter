using System.Reflection;

namespace Command_Interpreter
{
	/// <summary>
	/// Class that gets the string. It parses it, validates the verb (Delegate), and the parameters.
	/// </summary>
	public class Commands
    {
        public static bool terminate = false;

        private readonly List<(string name, Delegate func, string info)> tuple_commands = [];

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
		/// <param name="verb">The user's string</param>
		public void Command(string verb)
        {
            string[] textConsole = verb.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // Checks if it's null.
            if (textConsole.Length == 0)
                return;
           
            // Seek the Delegate.
            string command = textConsole[0].ToLower();
            Delegate _CalledFunc = tuple_commands.Find(func => func.name.ToLower() == command).func;

			// Seek the parameters of the Delegate. In this case: Int, Float, String, Bool and Array.
			if (_CalledFunc != null)
            {
                MethodInfo methodInfo = _CalledFunc.GetMethodInfo();

                if (methodInfo.GetParameters().Length == textConsole.Length - 1)
                {
                    var function = methodInfo.Invoke(_CalledFunc.Target, Parameters.SeekParams(methodInfo, textConsole));
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
                Console.WriteLine("Command does not exist");
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
        /// <summary>
        /// Adds the functions to the Tuple of Delegates.
        /// </summary>
        /// <param name="command">The key </param>
        /// <param name="func"></param>
        /// <param name="info"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void AddFunc(string command, Delegate func, string info)
        {
            //TODO: check that all parameters in func are parseable by the system (Tru ValidateParams func)
            //if some parameter can't be matched, give a warning to the user. How can we do it?
            //Checking that function called or parameters exist
            //Wrap all AddFunc in to TryCach to recover a log string.

            if (tuple_commands.Exists(x => x.name == command))
                throw new InvalidOperationException($"Function with name {command} already registered or params not exist");
            else
            {
                Parameters.ValidateParams(func.GetMethodInfo());
                tuple_commands.Add((command, func, info));
            }
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
