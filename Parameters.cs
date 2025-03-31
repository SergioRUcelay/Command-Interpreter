using System.Reflection;

namespace Command_Interpreter
{
    internal class Parameters
    {
        private readonly Dictionary<string, Delegate> _params;
        private readonly MethodInfo _methodInfo;

        private readonly string[] _parameters;
        int currentToken = 1;
        string stringtype;

        public Parameters(MethodInfo methodInfo, string[] param)
        {
            _parameters = param;
            _methodInfo = methodInfo;
            _params = new Dictionary<string, Delegate>()
            {
                { "Int32", IntType }, { "String", StringType }, { "Boolean", BoolType }, { "Single", FloatType }
            };
        }

        public object[] SeekParams()
        {
            List<object> arrayparams = new List<object>();
            foreach (var currentFunc in _methodInfo.GetParameters())
            {
                if (_params.TryGetValue(currentFunc.ParameterType.Name, out Delegate dictionaryFunc))
                    arrayparams.Add(dictionaryFunc.DynamicInvoke(_parameters[currentToken++]));
            }
            return arrayparams.ToArray();
        }

        public int IntType(string _parameters) => int.Parse(_parameters);

        public bool BoolType(string _parameters) => bool.Parse(_parameters);

        public float FloatType(string _parameters) => float.Parse(_parameters);

        public string StringType(string _parameters)
        {
            if (_parameters.Contains('-'))
                stringtype = _parameters.Trim('-');
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
