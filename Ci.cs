using System.Reflection;
using System.Runtime.InteropServices;

namespace Command_Interpreter
{
    internal class Ci
    {
        
        //public static bool terminate = true;
            //static public void List()
            //{
            //    Type typeCi = typeof(Ci);
            //    Type typeCo = typeof(Co);

            //    MethodInfo[] methodInfoCi = typeCi.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
            //    MethodInfo[] methodInfoCo = typeCo.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
            //    var allM = methodInfoCi.Concat(methodInfoCo);

            //    Console.WriteLine("\n Command info: \n");

            //    int maxW = allM.Max(methodInfo => methodInfo.Name.Length);

            //    foreach (var methodInfo in allM)
            //    {
            //        Console.ForegroundColor = ConsoleColor.Blue;
            //        Console.Write($"   " + methodInfo.Name.PadRight(maxW));
            //        string methodName = methodInfo.Name.ToString();
            //        foreach (var tuplaHelp in Commands.tuple_commands)
            //        {
            //            if (tuplaHelp.func.Method.Name == methodName)
            //            {
            //                Console.ForegroundColor = ConsoleColor.White;
            //                Console.WriteLine($" - " + tuplaHelp.help);
            //            }
            //        }
            //    }
            //}
        static public string Sql(string SQLRequest)
        {
            Console.WriteLine(SQLRequest);
            return SQLRequest;
        }

        static public int Add(int p, int a, int h)
        {
            Console.WriteLine("Addition method have been call");
            Console.WriteLine(p+a+h);
            return p;
        }

        static public int[] Sub(int[] code)
        {
            Console.WriteLine("Subtraction method have been call");
            return code;
        }
        static public float Multi(float code, float cod3)
        {
            Console.WriteLine("Multiply method have been call");
            return code;
        }
     
    }
    public static class Co
    {
        static public void Fov()
        {
            Console.WriteLine("Fov method have been call");
            //return fov;
        }

        public static void Clear()
        {
            Console.Clear();
        }
    }
}
