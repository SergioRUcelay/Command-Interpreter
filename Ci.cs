using System.Reflection;

namespace Command_Interpreter
{
    internal class Ci
    {
        
        public static bool terminate = true;
        static public void List()
        {
            Type typeCi = typeof(Ci);
            Type typeCo = typeof(Co);

            MethodInfo[] methodInfoCi = typeCi.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
            MethodInfo[] methodInfoCo = typeCo.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
            var allM = methodInfoCi.Concat(methodInfoCo);

            Console.WriteLine("\n Command info: \n");

            foreach (var methodInfo in allM)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"   " + methodInfo.Name);
                string methodName = methodInfo.Name.ToString();
                foreach (var tuplaHelp in Commands.tuple_commands)
                {
                    if (tuplaHelp.func.Method.Name == methodName)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"  - " + tuplaHelp.help);
                    }
                }
            }
        }
        static public string Sql(string SQLRequest)
        {
            Console.WriteLine(SQLRequest);
            return SQLRequest;
        }

        static public int Add(int p)
        {
            Console.WriteLine("Addition method have been call");
            return p;
        }

        static public int[] Sub(int[] code)
        {
            Console.WriteLine("Subtraction method have been call");
            return code;
        }
        static public float Multi(float code)
        {
            Console.WriteLine("Multiply method have been call");
            return code;
        }
        static public void Exit()
        {
            Environment.Exit(0);
            terminate = true;
        }
    }
    public static class Co
    {
        static public float Fov(float fov)
        {
            Console.WriteLine("Fov method have been call");
            return fov;
        }

        public static void Clear()
        {
            Console.Clear();
        }
    }
}
