
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

using System.Xml.Serialization;

namespace Command_Interpreter
{
	/// <summary>
	/// Represents the result of a command execution, including its status, return value, and additional metadata.
	/// </summary>
	/// <remarks>This class provides information about the outcome of a command, such as whether it succeeded or
	/// failed, the time the command was executed, and any associated messages or errors.It can also include a return
	/// value from the command, if applicable.</remarks>
	[XmlInclude(typeof(FuncList))]
	public class CommandReply
	{
		public enum LogType
		{
			Success, Error, Void
		}
		public CommandReply()
		{
			Timestamp = DateTime.Now;
		}

		public object? Return = null;
		public LogType Type { get; set; }
		public DateTime Timestamp { get; set; }
		public string? FunctionCalled { get; set; }
		public string? Message { get; set; }
		public string? ThrowError { get; set; }
	}

	/// <summary>
	///  Struct that contains the function and description strings of the class for creating the FuncList class list.
	/// </summary>
	public struct FunctionEntry
	{
		public string? Function = string.Empty;
		public string? Description = string.Empty;
		public string[]? Parameters;
		public string? Return;

		// Constructor to initialize Function and Description.
		public FunctionEntry(string? function, string? description, string[]? parameters, string? @return)
		{
			Function = function;
			Description = description;
			Parameters = parameters;
			Return = @return;
		}
	}

	/// <summary>
	/// Class that contains a list of FunctionEntry objects. Specifically designed to enumerate the different functions
	/// available in the Commands instance.
	/// </summary>
	public class FuncList
	{
		public List<FunctionEntry> Entries = [];
	}
}
