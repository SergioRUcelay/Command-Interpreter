using System.Reflection;

namespace Command_Interpreter
{
    internal class Commands
    {
        private readonly List<Delegate> l_commands = new();
        //public static List<(string help,Delegate func)> tuple_commands = new();
        public static List<(string help, Delegate func)> tuple_commands = new();
        private readonly string add, sql, multi, sub, fov, list, exit, clear;

        public Commands()
        {
            add = "Function for addition of two numbers";
            sql = "Function for Sql call to BBDD";
            multi = "Function for multiply two numbers";
            sub = "Function for subtraction of two numbers";
            fov = "Function for example to do something";
            list = "Function for list all functions of all program";
            exit = "Quit the program";
            clear = "Clear the screen";

            tuple_commands.Add((sql, Ci.Sql));
            tuple_commands.Add((multi, Ci.Multi));
            tuple_commands.Add((sub, Ci.Sub));
            tuple_commands.Add((fov, Co.Fov));
            tuple_commands.Add((list, Ci.List));
            tuple_commands.Add((exit, Ci.Exit));
            tuple_commands.Add((add, Ci.Add));
            tuple_commands.Add((clear, Co.Clear));

            l_commands.Add(Ci.Add); // Two int param, return int.
            l_commands.Add(Ci.Sql); // One string, return string.
            l_commands.Add(Ci.Multi); // One float, return float.
            l_commands.Add(Ci.Sub); // One int-array, return int-array.
            l_commands.Add(Co.Fov); // One float, return float.
            l_commands.Add(Ci.List); // A method for list all method of program.
            l_commands.Add(Ci.Exit); // A method for terminate the execution of program.
            l_commands.Add(Co.Clear); // A method for erase console.
        }

        public void Command(string[] textConsole)
        {
            if (textConsole.Length != 0)
            {
                var command = textConsole[0];//.ToLower();
                Delegate? delegateFind = l_commands.Find(x => x.Method.Name == command);
                //Delegate? delegateFind = tuple_commands.Find( a => a.func.Method.Name == command);
                MethodInfo? methodInfo = delegateFind?.GetMethodInfo();
                if (methodInfo != null)
                {
                    List<object> ps = new();
                    int currentToken = 1;
                    foreach (var param in methodInfo.GetParameters())
                    {
                        if (param.ParameterType == typeof(int))
                            ps.Add(int.Parse(textConsole[currentToken++]));
                        else if (param.ParameterType == typeof(float))
                            ps.Add(float.Parse(textConsole[currentToken++]));
                        else if (param.ParameterType == typeof(string))
                            ps.Add(string.Join(" ", textConsole[1..]));
                        else if (param.ParameterType == typeof(int[]))
                        {
                            List<int> ints = [];
                            for (int i = currentToken; i < (textConsole.Length); i++)
                                ints.Add(int.Parse(textConsole[i]));
                            ps.Add(ints.ToArray());
                        }
                        else if (param.ParameterType == typeof(void))
                        {
                            var retexit = methodInfo.Invoke(null, null);
                        }
                    }
                    var ret = methodInfo.Invoke(null, ps.ToArray());
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nThe command not found");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nThe command not found");
            }
        }
            //Delegate? delegateFind = l_commands.Find((x) => x.Method.Name == command);
            ////Delegate? delegateFind = tuple_commands.Find( a => a.func.Method.Name == command);
            //MethodInfo? methodInfo = delegateFind?.GetMethodInfo();
            //if (methodInfo != null)
            //{
            //    List<object> ps = new();
            //    int currentToken = 1;
            //    foreach (var param in methodInfo.GetParameters())
            //    {
            //        if (param.ParameterType == typeof(int))
            //            ps.Add(int.Parse(textConsole[currentToken++]));
            //        else if (param.ParameterType == typeof(float))
            //            ps.Add(float.Parse(textConsole[currentToken++]));
            //        else if (param.ParameterType == typeof(string))
            //            ps.Add(string.Join(" ", textConsole[1..]));
            //        else if (param.ParameterType == typeof(int[]))
            //        {
            //            List<int> ints = [];
            //            for (int i = currentToken; i < (textConsole.Length); i++)
            //                ints.Add(int.Parse(textConsole[i]));
            //            ps.Add(ints.ToArray());
            //        }
            //        else if (param.ParameterType == typeof(void))
            //        {
            //            var retexit = methodInfo.Invoke(null, null);
            //        }
            //    }
            //    var ret = methodInfo.Invoke(null, ps.ToArray());
            //}
            //else
            //{
            //    Console.ForegroundColor = ConsoleColor.Red;
            //    Console.WriteLine("\nThe command not found");
            //}
        
    }
}
