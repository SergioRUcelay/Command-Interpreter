namespace Command_Interpreter
{
    internal class Ci
    {

		static public double NoValid(double doubleParam)
		{
			Console.WriteLine(doubleParam + "," + doubleParam);
			return doubleParam;
		}
		static public string String(string SQLRequest, string SQLRequest2)
        {
            Console.WriteLine(SQLRequest +"," + SQLRequest2);
            return SQLRequest;
        }

        static public int Int(int p, int f)
        {
            Console.WriteLine(p+f);
            return p + f ;
        }

        static public int IntRest(int p)
        {
            Console.WriteLine(p);
            return p;
        }

        static public int[] Array(int[] code)
        {
            Console.WriteLine(code);
            foreach (int i in code)
            {
                Console.WriteLine(i);
            }
            return code;
        }
        static public float Float(float code)//, Vector2 er)
        {
            Console.WriteLine(code);
            return code;
        }
        static public bool Bool(bool value)
        {
            Console.WriteLine(value.ToString());
            return value;
        }

    }
}
