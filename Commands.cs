using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Channels;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Command_Interpreter
{
    internal class Commands
    {
        public static bool terminate = false;

        public readonly List<(string name, Delegate func, string help)> tuple_commands = new();

        private readonly string list = "Function for list all functions of all program";
        private readonly string exit = "Quit the program";
        private readonly string clear = "Clear the screen";

        public Commands()
        {
            tuple_commands.Add(("List", List, list));
            //tuple_commands.Add(("Exit", Exit, exit));
            tuple_commands.Add(("Clear", Clear, clear));
        }

        /// <summary>
        /// Retrieves the array on the command line and sorts the Delegate and its parameters.
        /// </summary>
        /// <param name="textConsole"></param>
        public void Command(string[] textConsole)
        {
            if (textConsole.Length == 0)
                return;
            try
            {
                string command = textConsole[0].ToLower();
                Delegate delegateFind = tuple_commands.Find(a => a.name.ToLower() == command).func;
                if (delegateFind != null)
                {
                    MethodInfo methodInfo = delegateFind.GetMethodInfo();
                    List<object> ps = [];
                    int currentToken = 1;
                    foreach (var param in methodInfo.GetParameters())
                    {
                        if (param.ParameterType == typeof(int))
                            ps.Add(int.Parse(textConsole[currentToken++]));
                        else if (param.ParameterType == typeof(float))
                            ps.Add(float.Parse(textConsole[currentToken++]));
                        else if (param.ParameterType == typeof(string))
                            ps.Add(string.Join(" ", textConsole[1..]));
                        else if (param.ParameterType.IsArray)
                        {
                            string newtext = "";
                            foreach (var item in textConsole[currentToken])
                            {
                                if (item != '[' && item != ']')
                                {
                                    newtext += item;
                                }
                            }

                            var peich = newtext.Split(',');

                            //if (param.ParameterType == typeof(int[]))
                            //    ps.AddRange(ParseArray<int>(peich));
                            //if (param.ParameterType == typeof(float[]))
                            //    ps.Add(ParseArray<float>(peich));
                            if (param.ParameterType == typeof(int[]))
                            {
                                List<int> ints = [];
                                for (int i = 0; i < (peich.Length); i++)
                                {
                                    if (param.ParameterType == typeof(int[]))
                                        ints.Add(int.Parse(peich[i]));
                                }
                                ps.Add(ints.ToArray());

                            }

                            if (param.ParameterType == typeof(float[]))
                            {
                                List<float> floats = [];
                                for (int i = 0; i < (peich.Length); i++)
                                {
                                    if (param.ParameterType == typeof(float[]))
                                        floats.Add(float.Parse(peich[i]));
                                }
                                ps.Add(floats.ToArray());

                                //}
                                // ps.Add(ints.ToArray());


                                //if (textConsole.Length <= 1)
                                //{
                                //    ints = null;
                                //}
                           // }
                        }
                    if (methodInfo.GetParameters().Length >= textConsole.Length - 1)// || ps.Count != 0)
                    {
                        var ret = methodInfo.Invoke(delegateFind.Target, ps.ToArray());
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" Too many arguments. The ");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(textConsole[0].ToString());
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" function only supports ");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write((methodInfo.GetParameters().Length).ToString());
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" Command not found");
                    Console.ResetColor();
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Sorry, incomplete command! Function arguments required.");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n" + ex.Message);
            }
            catch (FormatException fo)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Sorry, incorrect type format! Number required.");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n" + fo.Message);
            }
            catch (NullReferenceException refr)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Sorry, incorrect type format! Number required.");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n" + refr.Message);
            }
        }
        List<object> ParseArray<T>(string[] peich)
        {
            List<object> floats = [];
            for (int i = 0; i < (peich.Length); i++)
            {
                var jarl = (T)Convert.ChangeType(peich[i], typeof(T));
                floats.Add(jarl);
            }
            return floats;
        }
public void Add(string command, Delegate func, string info)
        {
            if (tuple_commands.Exists(x => x.name == command))
               throw new InvalidOperationException($"Function with name {command} already registered");
            else
               tuple_commands.Add((command.ToLower(), func, info));
        }

        public void List()
        {
            //Type typeCi = typeof(Ci);
            //Type typeCo = typeof(Co);

            //MethodInfo[] methodInfoCi = typeCi.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
            //MethodInfo[] methodInfoCo = typeCo.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
            //var allM = methodInfoCi.Concat(methodInfoCo);

            //Console.WriteLine("\n Command info: \n");

            int maxW = tuple_commands.Max(command => command.name.Length);

            foreach (var command in tuple_commands)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"   " + command.name.PadRight(maxW));
                Console.ResetColor ();
                Console.WriteLine($" - " + command.help);
            }
        }

        public void Exit()
        {
            terminate = true;
        }

        public static void Clear()
        {
            Console.Clear();
        }
    }
}
