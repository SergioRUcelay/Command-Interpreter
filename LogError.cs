
namespace Command_Interpreter
{
    [Serializable]
    public class LogError
    {
        public DateTime DateTimeError { get; set; }
        public string? DelegateCalled { get; set; }
        public string? Error { get; set; }
        public string? ThrowError { get; set; }
    }
}
