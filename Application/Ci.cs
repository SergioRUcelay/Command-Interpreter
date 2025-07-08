namespace Command_Interpreter.Aplication
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
            Console.WriteLine(SQLRequest + "," + SQLRequest2);
            return SQLRequest;

        }

        static public int Int(int p, int f)
        {
            Console.WriteLine(p + f);
            return p + f;
        }

        static public void IntRest(int p = default)
        {
            
        }

        static public int[] Array(int[] code)
        {
            //Console.WriteLine(code);
            foreach (int i in code)
            {
                Console.WriteLine(i);
            }
            return code;
        }
        static public float Float(float a, float b)//, Vector2 er)
        {
            Console.WriteLine(a + b);
            return a + b;
        }
        static public bool Bool(bool value)
        {
            Console.WriteLine(value.ToString());
            return value;
        }

    }
}
