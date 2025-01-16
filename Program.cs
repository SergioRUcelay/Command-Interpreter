using Command_Interpreter;


Commands com = new();
string add = "Function for addition of two numbers";
string sql = "Function for Sql call to BBDD";
string multi = "Function for multiply two numbers";
string sub = "Function for subtraction of two numbers";
string fov = "Function for example to do something";
//string clear = "Function for list all functions of all program";

com.Add("Exit", () => Commands.terminate = true, "Quit the program");
com.Add("Sql", Ci.Sql, sql);
com.Add("Multi", Ci.Multi, multi);
com.Add("Sub", Ci.Sub, sub);
//com.Add("multi", Co.Fov, fov);
com.Add("Add", Ci.Add, add);
////com.Add("list", Co.Clear, clear);
com.Add("Fov", Co.Fov, fov);

Console.ForegroundColor = ConsoleColor.DarkGreen;
Console.WriteLine("Wellcome to SQL Command Interpreter\n Version Alfa-0.1");

while (!Commands.terminate)
{
    Console.ResetColor();
    Console.Write("\nCi console..:> ");
    string verb = Console.ReadLine();
    string[] texConsole = verb.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    com.Command(texConsole);
}

