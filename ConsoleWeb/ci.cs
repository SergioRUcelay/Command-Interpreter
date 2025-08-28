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
		public static float Test(float a, int b)
		{
			return b - a;
		}

		public static double Test(float a, double b)
		{
			return a - b;
		}
		public static int Test(int a)
		{
			return a - 100;
		}
		public static float Test(float a)
		{
			return a - 100;
		}
		public static void Test()
		{
			Console.WriteLine(100);
		}
	}
}
