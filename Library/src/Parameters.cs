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
	/// This class handles parameters. It receives an array of strings, compares them with parameter type maps, and parses them to their appropriate types.
	/// </summary>
	internal class Parameters
	{
		private static readonly Dictionary<string, Delegate> _params;

		static Parameters()
		{
			_params = new Dictionary<string, Delegate>()
			{
				{ "Int32", IntType }, { "String", StringType }, { "Boolean", BoolType }, { "Single", FloatType }, {"Int32[]", ArrayIntType}
			};
		}

		/// <summary>
		/// Ensure that the parameters can be parsed by looking in parameters dictionary to see if the type exist.
		/// </summary>
		/// <param name="_methodInfo">The method that contains the parameters to compare.</param>
		/// <exception cref="FormatException"></exception>
		public static bool ValidateParams(MethodInfo _methodInfo)
		{
			foreach (var currentParam in _methodInfo.GetParameters())
			{
				if (!_params.TryGetValue(currentParam.ParameterType.Name, out Delegate? dictionaryFunc))
					throw new FormatException($"Can't parse parameter {currentParam.Name} of type {currentParam.ParameterType.Name} in function {_methodInfo.Name}");
			}
			return true;
		}

		/// <summary>
		/// Look for the parameters in the array. If they exist, parse them to the correct type.
		/// </summary>
		/// <param name="_methodParams"> The function that contains the parameter to look for. </param>
		/// <param name="commandParameters"> The array that contains the strings that match with the parameter type. </param>
		/// <returns> The array with the parameters. </returns>
		/// <exception cref="TargetParameterCountException"></exception>

		public static object[] SeekParams(ParameterInfo[] _methodParams, string[] commandParameters)
		{
			int currentToken = 0;
			List<object> arrayparams = new();

			if (_methodParams.Length == commandParameters.Length)
			{
				foreach (var currentParam in _methodParams)
				{
					if (_params.TryGetValue(currentParam.ParameterType.Name, out Delegate? dictionaryFunc))
					{
						var parseParam = dictionaryFunc.DynamicInvoke(commandParameters[currentToken++]);
						if (parseParam != null)
							arrayparams.Add(parseParam);
					}
				}
				return arrayparams.ToArray();
			}
			throw new TargetParameterCountException();
		}

		/// <summary>
		/// The method that pases the int type.
		/// </summary>
		/// <param name="_parameter"> The string to parse to the int type. </param>
		/// <returns> The string that matches the int type. </returns>
		public static int IntType(string _parameter) => int.Parse(_parameter);

		/// <summary>
		/// The method that pases the float type.
		/// </summary>
		/// <param name="_parameter"> The string to parse to the float type. </param>
		/// <returns> The string that matches the float type. </returns>
		public static float FloatType(string _parameter) => float.Parse(_parameter);

		/// <summary>
		/// The method that pases the bool type.
		/// </summary>
		/// <param name="_parameter"> The string to parse to the bool type. </param>
		/// <returns> The string that matches the bool type. </returns>
		public static bool BoolType(string _parameter) => bool.Parse(_parameter);

		/// <summary>
		/// Extracts the string value by removing the leading hyphen ('-') from the specified parameter.
		/// </summary>
		/// <param name="_parameter">The input string, which must begin with a hyphen ('-').</param>
		/// <returns>A string with the leading hyphen removed from the input parameter.</returns>
		/// <exception cref="FormatException">Thrown if the input string does not contain a leading hyphen ('-').</exception>
		public static string StringType(string _parameter)
		{
			string stringtype;

			if (_parameter.Contains('-'))
				stringtype = _parameter.Trim('-');
			else throw new FormatException("The strings must be preceded with \"-\".");

			return stringtype;
		}

		/// <summary>
		/// Converts a string representation of an array of integers into an actual integer array.
		/// </summary>
		/// <param name="_parameter">A string containing the array of integers, formatted as a comma-separated list enclosed in square brackets
		/// (e.g., "[1,2,3]"). The string must not contain spaces or periods.</param>
		/// <returns>An array of integers parsed from the input string.</returns>
		/// <exception cref="FormatException">Thrown if the input string is not properly formatted. The string must be enclosed in square brackets ("[ ]")
		/// and the elements must be separated by commas (",") without spaces or periods.</exception>
		public static int[] ArrayIntType(string _parameter)
		{
			int[] returnArraytype;
			Match match = Regex.Match(_parameter, @"(?<=\[\s*)\d+(?:,+\d+)*(?=\s*\])");
			if (match.Success)
			{
				var stringArray = _parameter.Replace("[", "").Replace("]", "").Replace(".", ",");
				var arraytype = stringArray.Split(',');
				returnArraytype = new int[arraytype.Length];
				for (int i = 0; i <= arraytype.Length - 1; i++)
				{
					var converttoint = IntType(arraytype[i]);
					returnArraytype[i] = converttoint;
				}
			}
			else
				throw new FormatException("The array must be wrapped with \"[ ]\" and separeted with \",\". And contain only Int types");

			return returnArraytype;
		}
	}
}
