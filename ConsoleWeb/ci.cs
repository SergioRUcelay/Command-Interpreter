namespace ConsoleWeb
{
	static public class Ci
	{
		private static bool terminate = false;
		public static bool Terminate
		{
			set { terminate = value; }
			get { return terminate; }
		}

		public static int Test(int a, int b)
		{
			return a + b;
		}
		public static int Test(int a)
		{
			return a - 100;
		}
		public static void Test()
		{
			Console.WriteLine(100);
		}
	}
}
