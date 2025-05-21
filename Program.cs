using Command_Interpreter;


Commands com = new();
string Int = "Function accepting int numbers";
string String = "Function accepting string chains";
string Array = "Function accepting Arrays of int";
string Float = "Function accepting float numbers";
string Bool = "Function accepting bool values";
string IntRest = "Function resta dos numeros dados";
string DoubleRest = "Function resta dos numeros dados";


//add try catch to deal with non parsable parameters
com.AddFunc("Exit", () => Commands.terminate = true, "Quit the program");
com.AddFunc("String", Ci.String,String);
//com.AddFunc("Array", Ci.Array, Array);
com.AddFunc("Float", Ci.Float, Float);
com.AddFunc("Int", Ci.Int, Int);
com.AddFunc("Bool", Ci.Bool, Bool);
com.AddFunc("inRest", Ci.IntRest, IntRest);
com.AddFunc("Double", Ci.NoValid, DoubleRest);
//com.AddFunc("Array", Ci.Array, Array);


Console.ForegroundColor = ConsoleColor.DarkGreen;
Console.WriteLine("Command Interpreter\n Version Alfa-0.1");

while (!Commands.terminate)
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("\nCi console..:> ");
    string? verb = Console.ReadLine();
    if (verb != null)
        com.Command(verb);
}