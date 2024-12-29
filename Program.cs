using Command_Interpreter;

Commands com = new();
Console.WriteLine("Wellcome to SQL Command Interpreter\n Version Alfa-0.1");

while (Ci.terminate)
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("\nCi console..:> ");
    string verb = Console.ReadLine();
    string[] texConsole = verb.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    com.Command(texConsole);
}