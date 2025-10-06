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
using System.Text.RegularExpressions;

namespace Command_Interpreter
{
	/// <summary>
	/// Provides functionality for managing and executing commands, including adding, removing, validating, and listing
	/// commands.
	/// </summary>
	/// <remarks>The <see cref="Interpreter"/> class allows users to define commands, execute commands based on user input.</remarks>
	public class Interpreter
	{
		public Parameters Parameters = new();

		// Function container.
		private readonly Dictionary<string, List<(Delegate func, string desc)>> commands = [];

		// This is a string that describes the List function.
		private readonly string help = "Function to list all functions registered.";
		private readonly string helpCommand = "Function to list all functions for a command.";
		private readonly string helpParsing = "Function to list all parseable types.";

		/// <summary>
		/// Initializes a new instance of the <see cref="Interpreter"/> class and sets up the available commands with their
		/// corresponding functions and descriptions.
		/// </summary>
		/// <remarks>This constructor populates the internal dictionary of commands with predefined command names,
		/// their associated functions, and descriptions. Each command is mapped to a list of tuples, where each tuple
		/// contains a function and its corresponding help description.</remarks>
		public Interpreter()
		{
			commands["help"] = [(List, help), (ListCommand, helpCommand)]; // If the key doesn't exist, create a new list with the function and description.
			commands["parsing"] = [(ListParser, helpParsing)];
		}

		/// <summary>
		/// Executes a command string by parsing its verb and parameters, and invokes the corresponding registered function.
		/// </summary>
		/// <remarks>The method parses the input command string to extract the verb and parameters. It then attempts to
		/// find a registered function matching the verb and parameter signature. If a matching function is found, it is invoked
		/// with the parsed parameters. <para> If no matching function is found, or if an error occurs during invocation, an
		/// error message is returned in the <see cref="CommandReply.Message"/> property. </para> <para> This method is
		/// case-insensitive for the command verb and trims unnecessary whitespace from the input string. </para></remarks>
		/// <param name="command">The command string to execute. The string should begin with a verb, followed by optional parameters. Whitespace at
		/// the beginning and end of the string is ignored.</param>
		/// <returns>A <see cref="CommandReply"/> object containing the result of the command execution.  The <see
		/// cref="CommandReply.Type"/> property indicates the outcome of the operation: <list type="bullet">
		/// <item><description><see cref="CommandReply.LogType.Success"/> if the command was successfully
		/// executed.</description></item> <item><description><see cref="CommandReply.LogType.Error"/> if the command was
		/// invalid, no matching function was found, or an error occurred during execution.</description></item>
		/// <item><description><see cref="CommandReply.LogType.Void"/> if the input command string was empty or
		/// null.</description></item> </list></returns>
		public CommandReply Command(string _command)
		{
			var command = _command;
			// Remove all whitespace in the ends of the string.
			command = command.TrimStart().Trim();

			// Checks if verb is null. And return a error message if it is null.
			if (string.IsNullOrEmpty(command))
			{
				return new CommandReply
				{
					Type = CommandReply.LogType.Void,
					Message = "Function call failed: empty string received."
				};
			}

			var verb = Regex.Match(command, @"^\S+").Value.ToLower();
			if (!commands.TryGetValue(verb, out List<(Delegate func, string desc)>? _CalledFunctions))
			{
				return new CommandReply
				{
					Type = CommandReply.LogType.Error,
					Message = $"Command \"{verb}\" doesn't exist.",
				};
			}

			//Command parsing and paramter extraction
			List<object?> arrayparams = [];
			command = command.Substring(verb.Length);

			// Analyze and structure the command line without the verb.
			while (command.Length > 0)
			{
				// Remove tabs and white spaces.
				command = Regex.Replace(command, @"^\s+|\s+$", "");
				int length = command.Length;

				foreach ((string regex, Delegate parser) in Parameters.Parsers.Values)
				{
					var matchParam = Regex.Match(command, regex);
					if (matchParam.Success)
					{
						var parsedObject = parser.DynamicInvoke(matchParam.Groups);
						arrayparams.Add(parsedObject);

						command = command.Substring(matchParam.Length);
					}
				}

				if (command.Length == length)
				{
					var nextWord = Regex.Match(command, @"^\S+").Value;
					arrayparams.Add(nextWord);
					command = command.Substring(nextWord.Length);
				}
			}
			// Method match and actual invocation.
			foreach ((Delegate del, string desc) in _CalledFunctions)
			{
				// Find if any delegate has the same number of parameters as the incoming string array (the parsed parameters from the command line).
				MethodInfo methodInfo = del.GetMethodInfo();
				var methodInfoParams = methodInfo.GetParameters();
				object[] parameters = arrayparams.ToArray();

				if ((methodInfoParams.Length != parameters.Length) ||
					(!methodInfoParams.Zip(parameters, (p1, p2) => p1.ParameterType == p2.GetType()).All(match => match)))
					continue;

				try
				{
					var functionReply = methodInfo.Invoke(del.Target, parameters);
					return new CommandReply
					{
						Type = CommandReply.LogType.Success,
						Return = functionReply,
						FunctionCalled = _command
					};
				}
				catch (TargetInvocationException ex)
				{
					return new CommandReply
					{
						Type = CommandReply.LogType.Error,
						FunctionCalled = verb,
						Message = ex.Message
					};
				}
			}
			return new CommandReply
			{
				Type = CommandReply.LogType.Error,
				FunctionCalled = verb,
				Message = $"No matching signature for: \"{verb}\" with ({String.Join(", ", arrayparams.Select(value => value.GetType().Name))}), type help for available functions."
			};
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
		public void RegisterFunction(string command, Delegate func, string info)
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
				list.Entries.AddRange(ListCommand(key).Entries);

			return list;
		}

		/// <summary>
		/// Retrieves a list of function entries associated with the specified command.
		/// </summary>
		/// <remarks>This method processes the delegates registered under the specified command and extracts metadata
		/// such as parameter types and return types to populate the <see cref="FuncList"/>. The returned list will be empty
		/// if no functions are associated with the given command.</remarks>
		/// <param name="command">The name of the command for which to retrieve the associated function entries.</param>
		/// <returns>A <see cref="FuncList"/> containing the function entries for the specified command. Each entry includes the
		/// command name, a description, the parameter types, and the return type of the associated delegate.</returns>
		private FuncList ListCommand(string command)
		{
			FuncList list = new();
			foreach ((Delegate del, string desc) in commands[command])
			{
				var ret = del.GetMethodInfo().ReturnType.ToString();
				List<string> args = new();
				foreach (var parameter in del.GetMethodInfo().GetParameters())
					args.Add(parameter.ParameterType.Name);

				list.Entries.Add(new FunctionEntry(command, desc, args.ToArray(), ret));
			}
			return list;
		}

		/// <summary>
		/// Creates and returns a new <see cref="FuncList"/> containing entries for each key in the parameter collection.
		/// </summary>
		/// <remarks>Each entry in the returned <see cref="FuncList"/> corresponds to a key in the internal parameter
		/// collection. The entries are initialized with the key name, while other properties of the <see
		/// cref="FunctionEntry"/> are set to <see langword="null"/>.</remarks>
		/// <returns>A <see cref="FuncList"/> containing <see cref="FunctionEntry"/> objects for each key in the parameter collection.</returns>
		private FuncList ListParser()
		{
			FuncList list = new();
			foreach (var key in Parameters.Parsers.Keys)
				list.Entries.Add(new FunctionEntry(key.Name, null, null, null));

			return list;
		}


	}
}
