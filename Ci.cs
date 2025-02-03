using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Command_Interpreter
{
    internal class Ci
    {
        static public string String(string SQLRequest, string SQLRequest2)
        {
            Console.WriteLine("String method have been call");
            Console.WriteLine(SQLRequest +"," + SQLRequest2);
            return SQLRequest;
        }

        static public int Int(int p)
        {
            Console.WriteLine("Int method have been call");
            //Console.WriteLine(p+a+h);
            return p;
        }

        static public int[] Array(int[] code)
        {
            Console.WriteLine("Array method have been call");
            Console.WriteLine(code);
            foreach (int i in code)
            {
                Console.WriteLine(i);
            }
            return code;
        }
        static public float Float(float code, Vector2 er)
        {
            Console.WriteLine("Float method have been call");
            return code;
        }
        static public bool Bool(bool value)
        {
            Console.WriteLine("Bool method have been call");
            Console.WriteLine(value.ToString());
            return value;
        }
    }
}
