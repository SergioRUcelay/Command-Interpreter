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
		public delegate object Parser(GroupCollection groups);

		 public static readonly Dictionary<string, (string regex, Parser parser)> _params;

		static Parameters() 
		{
			_params = new() {
				{ "Int32",		(@"^-?\d+",									groups => int.Parse(groups[0].Value))},
				{ "String",		(@"""([^""]*)""",   groups => groups[1].Value )},
				{ "Boolean",	(@"^\b(?:true|false)\b",					groups => bool.Parse(groups[0].Value))},
				//{ "Single", FloatType },
				//{ "Int32[]", ArrayIntType},
				//{ "Double", DoubleType },
				//{ "Single[]", ArrayFloatType }
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
				if (!_params.TryGetValue(currentParam.ParameterType.Name, out (string regex, Parser parser) dictionaryFunc))
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

		//public static object[] SeekParams(ParameterInfo[] _methodParams, string[] commandParameters)
		//{
		//	int currentToken = 0;
		//	List<object> arrayparams = [];

		//	if (_methodParams.Length == commandParameters.Length)
		//	{
		//		foreach (var currentParam in _methodParams)
		//		{
		//			if (_params.TryGetValue(currentParam.ParameterType.Name, out (string regex, Parser parser) dictionaryFunc))
		//			{
		//				Match match = Regex.Match(commandParameters[currentToken++], dictionaryFunc.regex);
		//				if (match.Success)
		//				{
		//					try
		//					{
		//						var parseParam = dictionaryFunc.parser.DynamicInvoke(match.Groups);

		//						if (parseParam != null)
		//							arrayparams.Add(parseParam);
		//					}
		//					catch (Exception)
		//					{ 
		//						throw new FormatException("Number must be an correct int.");
		//					}
		//				}
		//			}
		//		}
		//		return arrayparams.ToArray();
		//	}
		//	throw new TargetParameterCountException();
		//}

		/// <summary>
		/// The method that pases the int type.
		/// </summary>
		/// <param name="_parameter"> The string to parse to the int type. </param>
		/// <returns> The string that matches the int type.</returns>

		//public static int IntType(GroupCollection groups) 
		//{
		//		return int.Parse(groups[0].Value);

		//}

		public static double DoubleType(string _parameter) 
		{
			string doubleOk = @"^-?\d+(\.\d+)?$";
			Match match = Regex.Match(_parameter, doubleOk);
			if (match.Success)
				return double.Parse(match.Groups[0].Value);

			throw new FormatException("Number must be an correct decimal.");
			
		}

		/// <summary>
		/// The method that pases the float type.
		/// </summary>
		/// <param name="_parameter"> The string to parse to the float type. </param>
		/// <returns> The string that matches the float type. </returns>
		public static float FloatType(string _parameter)
		{
			string floatOk = @"^(\d+(\.\d+)?)f$";
			Match match = Regex.Match(_parameter, floatOk);
			
			if (match.Success)
				return float.Parse(match.Groups[1].Value);

			throw new FormatException("Number must be an integer or decimal and end with 'f'.");
		}

		/// <summary>
		/// The method that pases the bool type.
		/// </summary>
		/// <param name="_parameter"> The string to parse to the bool type. </param>
		/// <returns> The string that matches the bool type. </returns>
		//public static bool BoolType(string _parameter) => bool.Parse(_parameter);

		/// <summary>
		/// Extracts the string value by removing the leading hyphen ('-') from the specified parameter.
		/// </summary>
		/// <param name="_parameter">The input string, which must begin with a hyphen ('-').</param>
		/// <returns>A string with the leading hyphen removed from the input parameter.</returns>
		/// <exception cref="FormatException">Thrown if the input string does not contain a leading hyphen ('-').</exception>
		//public static string StringType(string _parameter)
		//{
	//		string pattern = @"^""([^""\\ \t]*(?:\\.[^""\\ \t]*)*)""$";
		//	Match match = Regex.Match(_parameter,pattern);
		//	if (match.Success)
		//		return match.Groups[1].ToString();

		//	throw new FormatException("Strings must be enclosed in quotation marks.");
		//}

		/// <summary>
		/// Converts a string representation of an array of integers into an actual integer array.
		/// </summary>
		/// <param name="_parameter">A string containing the array of integers, formatted as a comma-separated list enclosed in square brackets
		/// (e.g., "[1,2,3]"). The string must not contain spaces or periods.</param>
		/// <returns>An array of integers parsed from the input string.</returns>
		/// <exception cref="FormatException">Thrown if the input string is not properly formatted. The string must be enclosed in square brackets ("[ ]")
		/// and the elements must be separated by commas (",") without spaces or periods.</exception>
		//public static int[] ArrayIntType(string _parameter)
		//{
		//	Match match = Regex.Match(_parameter, @"^\[(-?\d+)(?:,(-?\d+))*\]$");
		//	int[] returnArraytype;

		//	if (match.Success)
		//	{
		//		returnArraytype = new int [match.Groups[1].Length + match.Groups[2].Captures.Count];
		//		returnArraytype[0] = IntType(match.Groups[1].Value);
		//		var groupTwoCapture = match.Groups[2].Captures;
		//		for (int i = 0; i < groupTwoCapture.Count; i++)
		//		{
		//			var converttoint = IntType(groupTwoCapture[i].Value);
		//			returnArraytype[i + 1] = converttoint;
		//		}
		//	}
		//	else
		//		throw new FormatException("The array must be wrapped with \"[ ]\" and separeted with \",\". And contain only Int types");

		//	return returnArraytype;
		//}

		public static float[] ArrayFloatType(string _parameter)
		{
			Match match = Regex.Match(_parameter, @"^\[(-?\d+(\.\d+)?f)(?:,(-?\d+(\.\d+)?f))*\]$");
			float[] returnArraytype;

			if (match.Success)
			{
				returnArraytype = new float[match.Groups[1].Length + match.Groups[2].Captures.Count];
				returnArraytype[0] = FloatType(match.Groups[1].Value);
				var groupTwoCapture = match.Groups[2].Captures;
				for (int i = 0; i < groupTwoCapture.Count; i++)
				{
					var converttoint = FloatType(groupTwoCapture[i].Value);
					returnArraytype[i + 1] = converttoint;
				}
			}
			else
				throw new FormatException("The array must be wrapped with \"[ ]\" and separeted with \",\". And contain only Float types");

			return returnArraytype;
		}

		/// <summary>
		/// Compares the parameter signatures of two methods to determine if they are identical.
		/// </summary>
		/// <param name="newFunc">An array of <see cref="ParameterInfo"/> objects representing the parameters of the first method.</param>
		/// <param name="exixtFunc">An array of <see cref="ParameterInfo"/> objects representing the parameters of the second method.</param>
		/// <returns><see langword="true"/> if both methods have the same number of parameters and their corresponding parameter types
		/// match; otherwise, <see langword="false"/>.</returns>
		public static bool SignatureCompare(ParameterInfo[] newFunc, ParameterInfo[] exixtFunc)
		{
			if (newFunc.Length == exixtFunc.Length)
			{
				return newFunc.Zip(exixtFunc, (p1, p2) => p1.ParameterType == p2.ParameterType).All(match => match);
			}
			return false;
		}
	}
}
