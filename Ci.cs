using System.Numerics;

namespace Command_Interpreter
{
    internal class Ci
    {

		static public double NoValid(double doubleParam)
		{
			Console.WriteLine("Double method have been call");
			Console.WriteLine(doubleParam + "," + doubleParam);
			return doubleParam;
		}
		static public string String(string SQLRequest, string SQLRequest2)
        {
            Console.WriteLine("String method have been call");
            Console.WriteLine(SQLRequest +"," + SQLRequest2);
            return SQLRequest;
        }

        static public int Int(int p, int f)
        {
            Console.WriteLine("Int method have been call");
            Console.WriteLine(p+f);
            return p + f ;
        }

        static public int IntRest(int p, int f)
        {
            Console.WriteLine("Int method have been call");
            Console.WriteLine(p - f);
            return p - f;
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
        static public float Float(float code)//, Vector2 er)
        {
            Console.WriteLine("Float method have been call");
            Console.WriteLine(code);
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
