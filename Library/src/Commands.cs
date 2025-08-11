
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
# endregion

using System.Reflection;
using System.Xml.Linq;

namespace Command_Interpreter
{
	/// <summary>
	/// Provides functionality for managing and executing commands, including adding, removing, validating, and listing
	/// commands.
	/// </summary>
	/// <remarks>The <see cref="Commands"/> class allows users to define commands, execute commands based on user input.</remarks>
	public class Commands
	{
		public static bool terminate = false;

		// Function container.
		private readonly List<(string name, List<Delegate> funcs, string info)> tuple_commands = new();

		// This is a string that describes the List function.
		private readonly string list = "Function for list all functions of all program";

		public Commands()
		{
			tuple_commands.Add(("List", new List<Delegate> { List }, list));
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
					Message = "No function has been called"
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
				List<Delegate> _CalledFunc = tuple_commands.Find(entry => entry.name.ToLower() == verb).funcs;

				if (_CalledFunc is not null)
				{
					foreach (Delegate del in _CalledFunc)
					{
						//find if any delegate has the same number of parameters as the incoming string array (the parsed parameters from the command line)
						MethodInfo methodInfo = del.GetMethodInfo();
						object[] parameters;
						try
						{
							parameters = Parameters.SeekParams(methodInfo, consoleParameter);
						}
						catch (TargetParameterCountException)
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
				else
					return new CommandReply
					{
						Type = CommandReply.LogType.Error,
						Message = $"Command \"{verb}\" doesn't exist.",
					};
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
		/// Add a new function to the functions tuple.
		/// </summary>
		/// <param name="command">The key for the Dictionary or Map</param>
		/// <param name="func">The Function that have been add</param>
		/// <param name="info">The string that describe the function that has been added</param>
		/// <exception cref="InvalidOperationException"></exception>
		public void AddFunc(string command, Delegate func, string info)
		{
			//Checking that parameters exist and can be parsed
			if (Parameters.ValidateParams(func.GetMethodInfo()))
			{
				if (!tuple_commands.Exists(x => x.name == command))
					tuple_commands.Add((command, new List<Delegate> { func }, info));

				else
				{
					var passFuncparameters = func.GetMethodInfo().GetParameters();
					var existingFunction = tuple_commands.FirstOrDefault(x => x.name == command).funcs.Find(x => x.Method.Name == func.Method.Name);
					if (existingFunction != null)
					{
						var exisFuncParamm = existingFunction.GetMethodInfo().GetParameters();
						if (passFuncparameters.Length != exisFuncParamm.Length)
						{
							tuple_commands.Add((command, new List<Delegate> { func }, info));
						}
					}
					else
						throw new InvalidOperationException($"Function with name {func.ToString()} don't exist");

					//tuple_commands.Add((command, new List<Delegate> { func }, info));
				}
				
			}
			//if (tuple_commands.Exists(x => x.name == command))
			//	{
			//		tuple_commands.Find(x => x.name == command).funcs.Add(func);
					//TODO:"add to the exising array of functions, unless the number of parameters is the same, in which case throw an error with "function with the same number of parameters alreaady registered"
					//throw new InvalidOperationException($"Function with name {command} already registered.");
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
		/// Retrieves a list of available commands and their associated information.
		/// </summary>
		/// <remarks>The returned <see cref="CommandReply"/> includes a list of tuples, where each tuple
		/// contains the name of a command and its corresponding information. This method is useful for discovering the
		/// available commands and their details.</remarks>
		/// <returns>A <see cref="CommandReply"/> object containing a collection of command names and their descriptions.</returns>
		private FuncList List()
		{
			FuncList list = new();

			foreach (var command in tuple_commands)
			{
				foreach (Delegate del in command.funcs)
				{
					var ret = del.GetMethodInfo().ReturnType.ToString();
					List<string> args = new List<string>();
					foreach (var parameter in del.GetMethodInfo().GetParameters())
					{
						args.Add(parameter.ParameterType.Name);
					}
					list.Entries.Add(new FunctionEntry(command.name, command.info, args.ToArray(), ret));
				}
			}
			return list;
		}
	}
}
