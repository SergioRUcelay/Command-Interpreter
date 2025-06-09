using System.Reflection;

namespace Command_Interpreter
{
	/// <summary>
	/// Class that receives the string. It parses it, validates the verb (Delegate), and the parameters.
	/// </summary>
	public class Commands
    {
        public static bool terminate = false;

        // Function container.
        private readonly List<(string name, Delegate func, string info)> tuple_commands = [];

        // String for describe the inplement function in the CI.
        private readonly string list = "Function for list all functions of all program";
        private readonly string help = "The help text of the Command Interpreter";

        public Commands()
        {
            tuple_commands.Add(("List", List, list));
            //tuple_commands.Add(("Help", Help, help));
        }

		/// <summary>
		/// Retrieves the array on the command line and sorts the Delegate and it's parameters.
		/// </summary>
		/// <param name="verb">The user's string</param>
		public string Command(string verb)
        {
            Delegate? _CalledFunc = null;

            // Checks if verb is null. And return to command line if it is true.
            if (string.IsNullOrEmpty(verb))
            {
                List(); // That will disappear in DLL version.
				return Loghandler.ErrorXmlLog(null, "No function has been called.\n");;
			}
            else
            {
                // Separates in two string, one it's the Function and the other are the Parameters
				string[] textConsole = verb.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				string[] consoleParameter = [.. verb.Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1)];

				// Seek the Delegate in the list. Command is the first string of the splited array and is therefore supposed to be the function. 
				string command = textConsole[0].ToLower();
				try
				{
					_CalledFunc = tuple_commands.Find(func => func.name.ToLower() == command).func;
                    if (_CalledFunc is not null)
                    {
                        MethodInfo methodInfo = _CalledFunc.GetMethodInfo();
					    var function = methodInfo.Invoke(_CalledFunc.Target, Parameters.SeekParams(methodInfo, consoleParameter));
					    return Loghandler.SuccessCall(_CalledFunc);
                    }
                    else
					    return Loghandler.ErrorXmlLog(null, $"The \"{command}\" dosen't exist.");
				}
				catch (TargetParameterCountException ex)
				{
					return Loghandler.ErrorXmlLog(ex, "The number of expected parameters differs from the required number.");
				}
                catch (TargetInvocationException ex)
                {
                    return Loghandler.ErrorXmlLog(ex, "The strings must be preceded with \"-\".");
				}
				
			}

		}
        /// <summary>
        /// Adds the functions to the Tuple of Delegates.
        /// </summary>
        /// <param name="command">The key for the Dictionary or Map</param>
        /// <param name="func">The Function that have been add</param>
        /// <param name="info">The string that describe the function that has been added</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void AddFunc(string command, Delegate func, string info)
        {
            //TODO: (DONE) check that all parameters in func are parseable by the system (Tru ValidateParams func)
            //if some parameter can't be matched, give a warning to the user. How can we do it?
            //Checking that function called or parameters exist
            //Wrap all AddFunc in to TryCach to recover a log string.

            if (tuple_commands.Exists(x => x.name == command))
                throw new InvalidOperationException($"Function with name {command} already registered.");
            else
            {
                Parameters.ValidateParams(func.GetMethodInfo());
                tuple_commands.Add((command, func, info));
            }
        }
        /// <summary>
        /// Delete the funtion-Delegate from the tuple.
        /// </summary>
        /// <param name="name">Name function</param>
        /// <exception cref="InvalidOperationException"></exception>
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
        /// <summary>
        /// Function that list all function added
        /// </summary>
        public void List()
        {
            int maxW = tuple_commands.Max(command => command.name.Length);

            foreach (var command in tuple_commands)
            {
                //Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"   " + "\x1b[91m" +command.name.PadRight(maxW));
                Console.ResetColor();
                Console.WriteLine($" - " + command.info);
            }
        }
		///TODO: Call a text file and list it in console. This text file describes how CI works. Could we implemet it, in Spectre-Console?
		/// <summary>
		/// Call a string that discribes how CI work and how it can used it
		/// </summary>
		public void Help()
        {
            Console.WriteLine("Here the help text of the Command Interpreter");
        }

        public void Help(Delegate values)
		{
			Console.WriteLine("Here the help text of the Command Interpreter");
		}

	}
}
