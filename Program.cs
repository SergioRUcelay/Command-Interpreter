using Command_Interpreter;


Commands com = new();
string Int = "Function accepting int numbers";
string String = "Function accepting string chains";
string Array = "Function accepting Arrays of int";
string Float = "Function accepting float numbers";
string Bool = "Function accepting bool values";

    com.AddFunc("Exit", () => Commands.terminate = true, "Quit the program");
    com.AddFunc("String", Ci.String,String);
    com.AddFunc("Array", Ci.Array, Array);
    com.AddFunc("Float", Ci.Float, Float);
    com.AddFunc("Int", Ci.Int, Int);
    com.AddFunc("Bool", Ci.Bool, Bool);
    //com.AddFunc("Array", Ci.Array, Array);


Console.ForegroundColor = ConsoleColor.DarkGreen;
Console.WriteLine("Command Interpreter\n Version Alfa-0.1");

while (!Commands.terminate)
{
    Console.ResetColor();
    Console.Write("\nCi console..:> ");
    string verb = Console.ReadLine();
    string[] texConsole = verb.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    com.Command(texConsole);
}

