using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Command_Interpreter
{
    internal class Commands
    {
        public static bool terminate = false;

        public readonly List<(string name, Delegate func, string info)> tuple_commands = [];

        private readonly string list = "Function for list all functions of all program";
        private readonly string addFunc = "Add a function to the console";
        private readonly string removeFunc = "Remove a function from the console";
        private readonly string help = "The help text of the Command Interpreter";

        public Commands()
        {
            tuple_commands.Add(("List", List, list));
            tuple_commands.Add(("AddFunc", AddFunc, addFunc));
            tuple_commands.Add(("RemoveFunc", RemoveFunc, removeFunc));
            tuple_commands.Add(("Help", Help, help));
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
                // Seeking the function.
                string command = textConsole[0].ToLower();
                Delegate delegateFind = tuple_commands.Find(a => a.name.ToLower() == command).func;

                // Seeking parameters. In this case: Int, Float, String, Bool and Array.
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
                        {
                            if (textConsole[currentToken].Contains('-'))
                                ps.Add(string.Join(" ", textConsole[currentToken++].Trim('-')));
                        }
                        else if (param.ParameterType == typeof(bool))
                            ps.Add(bool.Parse(textConsole[currentToken++]));
                        else if (param.ParameterType.IsArray)
                        {
                            string newtype = "";
                            foreach (var item in textConsole[currentToken])
                            {
                                if (item != '[' && item != ']')
                                {
                                    newtype += item;
                                }
                            }

                            var newarray = newtype.Split(',');

                            if (param.ParameterType == typeof(int[]))
                            {
                                List<int> ints = [];
                                for (int i = 0; i < (newarray.Length); i++)
                                {
                                    if (param.ParameterType == typeof(int[]))
                                        ints.Add(int.Parse(newarray[i]));
                                }
                                ps.Add(ints.ToArray());

                            }

                            if (param.ParameterType == typeof(float[]))
                            {
                                List<float> floats = [];
                                for (int i = 0; i < (newarray.Length); i++)
                                {
                                    if (param.ParameterType == typeof(float[]))
                                        floats.Add(float.Parse(newarray[i]));
                                }
                                ps.Add(floats.ToArray());
                            }
                        }
                    }
                    if (methodInfo.GetParameters().Length == textConsole.Length - 1 )
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
                //Console.ForegroundColor = ConsoleColor.Yellow;
                //Console.WriteLine($"\n" + ex.Message);
                //Console.WriteLine(ex);
            }
            catch (FormatException fo)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Sorry, incorrect type format!");
                //Console.ForegroundColor = ConsoleColor.Yellow;
                //Console.WriteLine($"\n" + fo.Message);
            }
            catch (NullReferenceException refr)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Sorry, incorrect type format!");
                //Console.ForegroundColor = ConsoleColor.Yellow;
                //Console.WriteLine($"\n" + refr.Message);
            }
            catch (TargetParameterCountException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Sorry, incorrect type format!.");
            }
        }
      
        public void AddFunc(string command, Delegate func, string info)
        {
            if (tuple_commands.Exists(x => x.name == command))
               throw new InvalidOperationException($"Function with name {command} already registered");
            else
               tuple_commands.Add((command, func, info));

        }

        public void RemoveFunc(string name)
        {
            if (!tuple_commands.Exists(x => x.name.ToLower() == name.ToLower()))
            {
                //throw new InvalidOperationException($"Function with name {name} don't exist");
                Console.ForegroundColor= ConsoleColor.Red;
                Console.WriteLine($" Function with name {name} don't exist");
                Console.ResetColor();

            }
            else
            {
                var index = tuple_commands.FindIndex(y  => y.name.ToLower() == name.ToLower());
                tuple_commands.Remove(tuple_commands[index]);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($" Function with name {name} has been removed");
                Console.ResetColor();
            }
        }

        public void List()
        {
            int maxW = tuple_commands.Max(command => command.name.Length);

            foreach (var command in tuple_commands)
            {
                if (command.name != "AddFunc" && command.name != "RemoveFunc")
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"   " + command.name.PadRight(maxW));
                    Console.ResetColor ();
                    Console.WriteLine($" - " + command.info);
                }
            }
        }

        public void Help()
        {
            List();
            Console.WriteLine("Here the help text of the Command Interpreter");
        }
    }
}
