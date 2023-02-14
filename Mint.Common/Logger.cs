namespace Mint.Common;

public class Logger
{
    public void Info(string loc, string msg) => Log(ConsoleColor.White, loc, msg);
    public void Important(string loc, string msg) => Log(ConsoleColor.Magenta, "IMPORTANT", loc, msg);
    public void Error(string loc, string msg) => Log(ConsoleColor.Red, loc, msg);
    public void Debug(string loc, string msg) => Log(ConsoleColor.Green, loc, msg);

    void Log(ConsoleColor color, string loc, string msg)
    {
        Console.ForegroundColor = color;
        Console.WriteLine($"{loc}: {msg}");
        Console.ForegroundColor = ConsoleColor.White;
    }

    void Event(string msg) {
        Console.WriteLine($"> {msg}");
    }
}
