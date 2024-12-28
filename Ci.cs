using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Command_Interpreter
{
    internal class Ci
    {
        static public void List()
        {
            Type typeCi = typeof(Ci);
            Type typeCo = typeof(Co);

            MethodInfo[] methodInfoCi = typeCi.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
            MethodInfo[] methodInfoCo = typeCo.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
            var allM = methodInfoCi.Concat(methodInfoCo);

            Console.WriteLine("Command info: ");
            Console.ForegroundColor = ConsoleColor.Blue;

            foreach (var methodInfo in allM)
            {
                Console.WriteLine(methodInfo.Name);
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
    }
    public static class Co
    {
        static public float Fov(float fov)
        {
            Console.WriteLine("Fov method have been call");
            return fov;
        }
    }
}
