namespace Mint.Common;

public class Logger
{
    public void Info(string loc, string msg) => Log(ConsoleColor.White, "INFO", loc, msg);
    public void Important(string loc, string msg) => Log(ConsoleColor.Magenta, "IMPORTANT", loc, msg);
    public void Error(string loc, string msg) => Log(ConsoleColor.Red, "ERROR", loc, msg);
    public void Debug(string loc, string msg) => Log(ConsoleColor.Green, "DEBUG", loc, msg);

    void Log(ConsoleColor color, string level, string loc, string msg)
    {
        Console.ForegroundColor = color;
        Console.WriteLine($"[{level}] [{loc}] {msg}");
        Console.ForegroundColor = ConsoleColor.White;
    }
}
