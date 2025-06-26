using System.Reflection;

namespace Command_Interpreter
{
	/// <summary>
	/// This class receives a string, parses it, and validates the verb (Delegate), and its parameters.
	/// </summary>
	public class Commands
    {
        public static bool terminate = false;

        // Function container.
        private readonly List<(string name, Delegate func, string info)> tuple_commands = [];

        // This is a string that describes the List function.
        private readonly string list = "Function for list all functions of all program";

        public Commands()
        {
            tuple_commands.Add(("List", List, list));
        }

		/// <summary>
		/// Retrieves the array on the command line and sorts the Delegate and it's parameters.
		/// </summary>
		/// <param name="verb">The user's string</param>
		/// <returns></returns>
		public CommandReply Command(string verb)
        {
            Delegate? _CalledFunc = null;

            // Checks if verb is null. And return a error message if it is null.
            if (string.IsNullOrEmpty(verb))
            {
                return (new CommandReply 
                {
                    Type = CommandReply.LogType.Void,
                    Message = "No function has been called"
                });
			}
			// Search for the Delegate in the list. Command is the first string of the split array and is therefore supposed to be the function.
			else
			{
                // Create an array of strings from a console string.
				string[] textConsole = verb.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                // This is the array of the parameters.
				string[] consoleParameter = [.. verb.Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1)];
				// This is a string that contains the name of a function.
				string command = textConsole[0].ToLower();

				try
				{
                    if (!ValidateFunction(command))
                        throw new Exception();

					_CalledFunc = tuple_commands.Find(func => func.name.ToLower() == command).func;
                    if (_CalledFunc is not null)
                    {
                        MethodInfo methodInfo = _CalledFunc.GetMethodInfo();
                        var functionReply = methodInfo.Invoke(_CalledFunc.Target, Parameters.SeekParams(methodInfo, consoleParameter));
                        if (functionReply is CommandReply reply && functionReply is not null)
                        {
							return (new CommandReply
							{
                                ListFunctions = reply.ListFunctions,
								Return = functionReply.ToString() ?? string.Empty,
								Type = CommandReply.LogType.Success,
								FunctionCalled = command,
								Message = "Function has been executed correctly.",
							});
						}
                        else if(functionReply is not null)
                        {
                            return (new CommandReply
                            {
                                Return = functionReply.ToString() ?? string.Empty,
                                Type = CommandReply.LogType.Success,
                                FunctionCalled = command,
                                Message = "Function has been executed correctly.",
                            });
						}
						else
                        {
							return (new CommandReply
							{
								Type = CommandReply.LogType.Success,
								FunctionCalled = command,
								Message = "Function has been executed correctly.",
							});
						}
					}
                    else
						return (new CommandReply
                        {
                            Type = CommandReply.LogType.Void,
                            Message = $"The \"{command}\" doesn't exist.",
                        });
				}
				catch (TargetParameterCountException)
                {
                    return (new CommandReply
                    {
                        Type = CommandReply.LogType.Error,
                        FunctionCalled = command,
                        Message = "The number of expected parameters differs from the required number.",
                    });
				}
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException is not null)
                    {
                        return (new CommandReply
                        {
                            Type = CommandReply.LogType.Error,
                            FunctionCalled = command,
                            Message = ex.InnerException.Message,
                        });
                    }
                    else return (new CommandReply
                    {
                        Type = CommandReply.LogType.Error,
                        FunctionCalled = command,
                        Message = ex.Message,
                    });
				}
				catch (Exception)
				{
					return (new CommandReply
					{
						Type = CommandReply.LogType.Error,
						Message = $"The \"{command}\" doesn't exist.",
					});
				}

			}

		}

		/// <summary>
		/// Add a new function to the functions tuple.
		/// </summary>
		/// <param name="command">The key for the Dictionary or Map</param>
		/// <param name="func">The Function that have been add</param>
		/// <param name="info">The string that describe the function that has been added</param>
		/// <exception cref="InvalidOperationException"></exception>
		public void AddFunc(string command, Delegate func, string info)
        {
            //Checking that function called or parameters exist
            if (tuple_commands.Exists(x => x.name == command))
                throw new InvalidOperationException($"Function with name {command} already registered.");
            else
            {
                Parameters.ValidateParams(func.GetMethodInfo());
                tuple_commands.Add((command, func, info));
            }
        }

        /// <summary>
        /// Deletes a function from the tuple.
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
        /// Validates whether the specified command exists in the predefined list of commands.
        /// </summary>
        /// <param name="command">The command to validate. This value is case-insensitive.</param>
        /// <returns><see langword="true"/> if the command exists in the list; otherwise, <see langword="false"/>.</returns>
        public bool ValidateFunction(string command)
        {
            if (tuple_commands.Exists(x => x.name.ToLower() == command.ToLower()))
                return true;

            return false;
        }

		/// <summary>
        /// Displays a formatted list of commands and their descriptions in the console.
        /// </summary>
        /// <remarks>Each command name is displayed in a formatted style, followed by its description. 
        /// The command names are aligned to ensure a consistent appearance.</remarks>
		public void ConsoleList()
        {
            int maxW = tuple_commands.Max(command => command.name.Length);

            foreach (var command in tuple_commands)
            {
                //Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"   " + "\x1b[94m" +command.name.PadRight(maxW));
                Console.ResetColor();
                Console.WriteLine($" - " + command.info);
            }
        }

        /// <summary>
        /// Retrieves a list of available commands and their associated information.
        /// </summary>
        /// <remarks>The returned <see cref="CommandReply"/> includes a list of tuples, where each tuple
        /// contains the name of a command and its corresponding information. This method is useful for discovering the
        /// available commands and their details.</remarks>
        /// <returns>A <see cref="CommandReply"/> object containing a collection of command names and their descriptions.</returns>
        public CommandReply List()
        {
            CommandReply replay = new();

            foreach (var command in tuple_commands)
            {
                replay.ListFunctions.Add((command.name, command.info));
            }
            return replay;
        }
	}
}
