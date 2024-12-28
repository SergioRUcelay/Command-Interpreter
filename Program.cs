using Command_Interpreter;
using System.Reflection;

List<Delegate> l_commands = new();
l_commands.Add(Ci.Add); // Two int param, return int.
l_commands.Add(Ci.Sql); // One string, return string.
l_commands.Add(Ci.Multi); // One float, return float.
l_commands.Add(Ci.Sub); // One int-array, return int-array.
l_commands.Add(Co.Fov); // One float, return float.
l_commands.Add(Ci.List); // A method for list all method of the program.

Console.Write("Ci console..:> ");
string? verb = Console.ReadLine();

string[] ff = verb.Split(' ', StringSplitOptions.RemoveEmptyEntries);

var command = ff[0];//.ToLower();

Delegate? delegadeFind = l_commands.Find((x) => x.Method.Name == command);
MethodInfo? methodInfo = delegadeFind?.GetMethodInfo();
if (methodInfo != null)
{
    List<object> ps = new();
    int currentToken = 1;
    foreach (var param in methodInfo.GetParameters())
    {
        if (param.ParameterType == typeof(int))
            ps.Add(int.Parse(ff[currentToken++]));
        else if (param.ParameterType == typeof(float))
            ps.Add(float.Parse(ff[currentToken++]));
        else if (param.ParameterType == typeof(string))
            ps.Add(string.Join(" ", ff[1..]));
        else if (param.ParameterType == typeof(int[]))
        {
            List<int> ints = [];
            for (int i = currentToken; i < ff.Length; i++)
                ints.Add(int.Parse(ff[i]));
            ps.Add(ints.ToArray());
        }
    }
    var ret = methodInfo.Invoke(null, ps.ToArray());
    ///lllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll
    // Console.WriteLine($"result {ret}");
    // int currentToken = 1;
    // if (parsing.TryGetValue(command, out Delegate? p))
    // {
    //     var mi = RuntimeReflectionExtensions.GetMethodInfo(p); List<object> ps = new();
    //     foreach (var param in mi.GetParameters())
    //     {
    //         if (param.ParameterType == typeof(int[]))
    //         {
    //             if (ff[currentToken++] == "[")
    //             {
    //                 //  currentToken++;
    //                 //assert next character is start of array
    //                 //until end of array
    //                 //parse and add to array
    //                 //add toparam list
    //                 List<int> ps2 = new();
    //                 //for (int i = currentToken; i < ff.Length; i++)
    //                 while (ff[currentToken] != "]")
    //                 {
    //                     ps2.Add(int.Parse(ff[currentToken++]));
    //                 }
    //                 currentToken++;
    //                 ps.Add(ps2.ToArray());
    //             }
    //             else
    //                 Console.WriteLine("peeeeeenk!");
    //         }
    //         else
    //             if (param.ParameterType == typeof(int))
    //             {
    //                 ps.Add(int.Parse(ff[currentToken++]));
    //             }
    //             else if (param.ParameterType == typeof(string))
    //             {
    //                 ps.Add(string.Join(" ", ff[1..]));
    //             }
    //     }
    //         var res = mi.Invoke(null, ps.ToArray());

    //         Console.WriteLine($"result {res}");
    //}

    //else { Console.WriteLine("The command do not exist"); }

}