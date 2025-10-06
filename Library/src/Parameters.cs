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
	public delegate T Parser<T>(GroupCollection groups);

	/// <summary>
	/// This class handles parameters. It receives an array of strings, compares them with parameter type maps, and parses them to their appropriate types.
	/// </summary>
	public class Parameters
	{
		public readonly Dictionary<Type, (string regex, Delegate parser)> Parsers = [];

		public Parameters()
		{
			RegisterCustomType(@"^\d+(?=\s|$)", groups => int.Parse(groups[0].Value));
			RegisterCustomType(@"^\b(?:true|false)\b", groups => bool.Parse(groups[0].Value));
			RegisterCustomType(@"^(-?\d+(?:\.\d+)?)([fF])", groups => float.Parse(groups[1].Value));
			RegisterCustomType(@"""([^""]*)""", groups => groups[1].Value);
			RegisterCustomType(@"^\[(-?\d+)(?:,(-?\d+))*\]$", ArrayIntType);
		}

		/// <summary>
		/// Associates a regular expression with a parser for a specific type.
		/// </summary>
		/// <typeparam name="T">The type for which the parser is being set.</typeparam>
		/// <param name="regex">The regular expression used to match input for the parser.</param>
		/// <param name="parser">The parser instance that processes input matching the specified regular expression.</param>
		/// <exception cref="Exception">Thrown if a parser for the specified type <typeparamref name="T"/> has already been set.</exception>
		public void RegisterCustomType<T>(string regex, Parser<T> parser)
		{
			if (!Parsers.ContainsKey(typeof(T)))
				Parsers[typeof(T)] = (regex, parser);
			else
				throw new Exception($"Can't add key mamed: {typeof(T).Name}, because already exist.");
		}

		/// <summary>
		/// Removes the parser associated with the specified key type.
		/// </summary>
		/// <typeparam name="T">The type of the key used to identify the parser.</typeparam>
		/// <param name="key">The key whose associated parser is to be removed.</param>
		/// <exception cref="Exception">Thrown if the parser associated with the specified key type does not exist.</exception>
		public void ClearParser<T>(T key)
		{
			if (!Parsers.Remove(typeof(T)))
				throw new Exception($"Can't remove. Key mamed: {key}, doesn't exist.");
		}

		/// <summary>
		/// Ensure that the parameters can be parsed by looking in parameters dictionary to see if the type exist.
		/// </summary>
		/// <param name="_methodInfo">The method that contains the parameters to compare.</param>
		/// <exception cref="FormatException"></exception>
		public bool ValidateParams(MethodInfo _methodInfo)
		{
			foreach (var currentParam in _methodInfo.GetParameters())
			{
				if (!Parsers.ContainsKey(currentParam.ParameterType))
					throw new FormatException($"Can't parse parameter {currentParam.Name} of type {currentParam.ParameterType.Name} in function {_methodInfo.Name}");
			}
			return true;
		}

		/// <summary>
		/// Converts a string representation of an array of integers into an actual integer array.
		/// </summary>
		/// <param name="_parameter">A string containing the array of integers, formatted as a comma-separated list enclosed in square brackets
		/// (e.g., "[1,2,3]"). The string must not contain spaces or periods.</param>
		/// <returns>An array of integers parsed from the input string.</returns>
		/// <exception cref="FormatException">Thrown if the input string is not properly formatted. The string must be enclosed in square brackets ("[ ]")
		/// and the elements must be separated by commas (",") without spaces or periods.</exception>
		public int[] ArrayIntType(GroupCollection groups)
		{
			int[] returnArraytype;

			returnArraytype = new int[groups[1].Length + groups[2].Captures.Count];
			returnArraytype[0] = int.Parse(groups[1].Value);
			var groupTwoCapture = groups[2].Captures;
			for (int i = 0; i < groupTwoCapture.Count; i++)
			{
				var converttoint = int.Parse(groupTwoCapture[i].Value);
				returnArraytype[i + 1] = converttoint;
			}

			return returnArraytype;
		}

		/// <summary>
		/// Compares the parameter signatures of two methods to determine if they are identical.
		/// </summary>
		/// <param name="newFunc">An array of <see cref="ParameterInfo"/> objects representing the parameters of the first method.</param>
		/// <param name="exixtFunc">An array of <see cref="ParameterInfo"/> objects representing the parameters of the second method.</param>
		/// <returns><see langword="true"/> if both methods have the same number of parameters and their corresponding parameter types
		/// match; otherwise, <see langword="false"/>.</returns>
		public bool SignatureCompare(ParameterInfo[] newFunc, ParameterInfo[] exixtFunc)
		{
			if (newFunc.Length == exixtFunc.Length)
			{
				return newFunc.Zip(exixtFunc, (p1, p2) => p1.ParameterType == p2.ParameterType).All(match => match);
			}
			return false;
		}
	}
}
