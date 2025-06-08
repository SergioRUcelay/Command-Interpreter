using System.Reflection;

namespace Command_Interpreter
{
    internal class Parameters
    {
        private static readonly Dictionary<string, Delegate> _params;

        static Parameters()
        {
            _params = new Dictionary<string, Delegate>()
            {
                { "Int32", IntType }, { "String", StringType }, { "Boolean", BoolType }, { "Single", FloatType }
            };
        }

        //TODO: remove return
        public static bool ValidateParams(MethodInfo _methodInfo)
        {
			foreach (var currentParam in _methodInfo.GetParameters())
			{
				if(!_params.TryGetValue(currentParam.ParameterType.Name, out Delegate? dictionaryFunc))
                    throw new FormatException($"Can't parse parameter {currentParam.Name} of type {currentParam.ParameterType.Name} in function {_methodInfo.Name}");
			}
			return true;
        }

        public static object[] SeekParams(MethodInfo _methodInfo, string[] _parameters)
        {
			int currentToken = 0;
			List<object> arrayparams = [];
            var funcParam = _methodInfo.GetParameters();
			//first, we want to make sure we can match all the paramters that the function requires, which doesn't have to be all of them as some might
			//have default values
			//if not all required paramters are match, throw "not enough params" or something
            
            if (funcParam.Length == _parameters.Length)
            {
                foreach (var currentParam in funcParam)
                {
                    if (_params.TryGetValue(currentParam.ParameterType.Name, out Delegate? dictionaryFunc))
                    {
                        var parseParam = dictionaryFunc.DynamicInvoke(_parameters[currentToken++]);

                        if (parseParam != null)
                            arrayparams.Add(parseParam);
                    }
                }
			    return arrayparams.ToArray();
            }
            else
            {
				return arrayparams.ToArray();// This Array will be null.
			}
           
		
		}
      
		public static int IntType(string _parameter) => int.Parse(_parameter);

		public static bool BoolType(string _parameter) => bool.Parse(_parameter);

		public static float FloatType(string _parameter) => float.Parse(_parameter);

		public static string StringType(string _parameter)
		{
			string stringtype;

			if (_parameter.Contains('-'))
				stringtype = _parameter.Trim('-');
			else throw new FormatException();
			return stringtype;
		}

        public List<object> ArrayType()
        {
            List<object> arraytype = [];
            return arraytype;
        }
        //    else if (param.ParameterType.IsArray)
        //    {
        //        string newtype = "";
        //        foreach (var item in textConsole[currentToken])
        //        {
        //            if (item != '[' && item != ']')
        //            {
        //                newtype += item;
        //            }
        //        }

        //        var newarray = newtype.Split(',');

        //        if (param.ParameterType == typeof(int[]))
        //        {
        //            List<int> ints = [];
        //            for (int i = 0; i < (newarray.Length); i++)
        //            {
        //                if (param.ParameterType == typeof(int[]))
        //                    ints.Add(int.Parse(newarray[i]));
        //            }
        //            ps.Add(ints.ToArray());

        //        }

        //        if (param.ParameterType == typeof(float[]))
        //        {
        //            List<float> floats = [];
        //            for (int i = 0; i < (newarray.Length); i++)
        //            {
        //                if (param.ParameterType == typeof(float[]))
        //                    floats.Add(float.Parse(newarray[i]));
        //            }
        //            ps.Add(floats.ToArray());
        //        }
        //    }
    }
}
