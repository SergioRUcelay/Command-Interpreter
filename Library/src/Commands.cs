#region License
// Copyright (c) 2025 Sergio R. Ucelay
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System.Reflection;

namespace Command_Interpreter
{
	/// <summary>
	/// Provides functionality for managing and executing commands, including adding, removing, validating, and listing
	/// commands.
	/// </summary>
	/// <remarks>The <see cref="Commands"/> class allows users to define commands, execute commands based on user input.</remarks>
	public class Commands
	{
		// Function container.
		private readonly Dictionary<string, List<(Delegate func, string desc)>> commands = [];

		// This is a string that describes the List function.
		private readonly string help = "Function to list all functions registered.";

		public Commands()
		{
			commands["help"] = [(List, help)]; // If the key doesn't exist, create a new list with the function and description.
		}

		/// <summary>
		/// Retrieves the array on the command line and sorts the Delegate and it's parameters.
		/// </summary>
		/// <param name="command">The user's string</param>
		/// <returns></returns>
		public CommandReply Command(string command)
		{
			// Checks if verb is null. And return a error message if it is null.
			if (string.IsNullOrEmpty(command))
			{
				return new CommandReply
				{
					Type = CommandReply.LogType.Void,
					Message = "Function call failed: empty string received."
				};
			}
			// Search for the Delegate in the list. Command is the first string of the split array and is therefore supposed to be the function.
			// Create an array of strings from a console string.
			string[] textConsole = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			// This is the array of the parameters.
			string[] consoleParameter = textConsole[1..];
			// This is a string that contains the name of a function.
			string verb = textConsole[0].ToLower();
			try
			{
				if (!commands.TryGetValue(verb, out List<(Delegate func, string desc)>? _CalledFunctions))
				{
					return new CommandReply
					{
						Type = CommandReply.LogType.Error,
						Message = $"Command \"{verb}\" doesn't exist.",
					};
				}
				else
				{
					foreach ((Delegate del, string desc) in _CalledFunctions)
					{
						//find if any delegate has the same number of parameters as the incoming string array (the parsed parameters from the command line)
						MethodInfo methodInfo = del.GetMethodInfo();
						var methodInfoParams = methodInfo.GetParameters();
						object[] parameters;
						try
						{
							parameters = Parameters.SeekParams(methodInfoParams, consoleParameter);
							if (methodInfoParams.Length != parameters.Length)
								break;
						}
						catch (TargetParameterCountException)
						{
							continue;
						}
						catch (TargetInvocationException ex) when (ex.InnerException is FormatException)
						{
							continue;
						}

						var functionReply = methodInfo.Invoke(del.Target, parameters);

						return new CommandReply
						{
							Type = CommandReply.LogType.Success,
							Return = functionReply,
							FunctionCalled = verb
						};
					}
					throw new TargetParameterCountException();
				}

			}
			catch (TargetParameterCountException ex)
			{
				return new CommandReply
				{
					Type = CommandReply.LogType.Error,
					FunctionCalled = verb,
					Message = ex.Message.ToString()
				};
			}
			catch (TargetInvocationException ex)
			{
				if (ex.InnerException is not null)
				{
					return new CommandReply
					{
						Type = CommandReply.LogType.Error,
						FunctionCalled = verb,
						Message = ex.InnerException.Message,
					};
				}
				else return new CommandReply
				{
					Type = CommandReply.LogType.Error,
					FunctionCalled = verb,
					Message = ex.Message,
				};
			}
		}

		/// <summary>
		/// Adds a command and its associated delegate function to the command registry.
		/// </summary>
		/// <remarks>This method ensures that the provided delegate function has a valid signature and does not
		/// conflict with any existing functions registered under the same command. If the command does not already exist in
		/// the registry, it will be created. Otherwise, the function will be added to the existing command's list of
		/// associated functions, provided its signature is unique.</remarks>
		/// <param name="command">The name of the command to register. This value is case-insensitive.</param>
		/// <param name="func">The delegate function to associate with the command. The function's signature must be valid and unique for the
		/// given command.</param>
		/// <param name="info">A description of the command, providing additional context or usage information.</param>
		/// <exception cref="InvalidOperationException">Thrown if a function with the same name and signature already exists for the specified command.</exception>
		public void AddFunc(string command, Delegate func, string info)
		{
			//Checking that parameters exist and can be parsed
			if (Parameters.ValidateParams(func.GetMethodInfo()))
			{
				if (!commands.TryGetValue(command.ToLower(), out var entry))
					commands[command.ToLower()] = [(func, info)];
				else
				{
					List<bool> check = [];
					//entry is the function/description list
					var passFuncParameters = func.GetMethodInfo().GetParameters();
					foreach (var existingFunction in entry)
					{
						var exisFuncParamm = existingFunction.func.GetMethodInfo().GetParameters();
						check.Add(Parameters.SignatureCompare(passFuncParameters, exisFuncParamm));
					}
					if (!check.Any(f => f))
					{
						entry.Add((func, info));
						check.Clear();
					}
					else
						throw new InvalidOperationException($"Function with name \"{func.Method.Name}\" exist, function wasn't added.");
				}
			}
		}

		/// <summary>
		/// Retrieves a list of available commands and their associated information.
		/// </summary>
		/// <remarks>The returned <see cref="CommandReply"/> includes a list of tuples, where each tuple
		/// contains the name of a command and its corresponding information. This method is useful for discovering the
		/// available commands and their details.</remarks>
		/// <returns>A <see cref="CommandReply"/> object containing a collection of command names and their descriptions.</returns>
		private FuncList List()
		{
			FuncList list = new();

			foreach (var key in commands.Keys)
			{
				foreach ((Delegate del, string desc) in commands[key])
				{
					var ret = del.GetMethodInfo().ReturnType.ToString();
					List<string> args = new();
					foreach (var parameter in del.GetMethodInfo().GetParameters())
						args.Add(parameter.ParameterType.Name);

					list.Entries.Add(new FunctionEntry(key, desc, args.ToArray(), ret));
				}
			}
			return list;
		}
	}
}
