using Mint.Common.Config;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Mint.Common;

public class Logger
{
    private readonly IConfiguration configuration;
    private readonly ConcurrentQueue<PrintedLog> logs = new();

    public Logger(IConfiguration configuration)
    {
        this.configuration = configuration;

        Task.Run(() =>
        {
            while (true)
            {
                if (!logs.IsEmpty)
                {
                    var result = logs.TryDequeue(out PrintedLog? log);
                    if (result && log is not null)
                    {
                        log.Level.Init();
                        Console.WriteLine(log.Msg);
                        Console.ResetColor();
                    }
                }
            }
        });
    }

    public void Info(string msg, [CallerMemberName] string caller = "") => 
        Log(Level.INFO, msg, caller);
    public void Warning(string msg, [CallerMemberName] string caller = "") => 
        Log(Level.WARNING, msg, caller);
    public void Error(string msg, [CallerMemberName] string caller = "") => 
        Log(Level.ERROR, msg, caller);
    public void Fatal(string msg, [CallerMemberName] string caller = "") => 
        Log(Level.FATAL, msg, caller);

    public void Debug(string msg, [CallerMemberName] string caller = "")
    {
        if (configuration.Debug()) Log(Level.DEBUG, msg, caller);
    }

    void Log(Level level, string msg, [CallerMemberName] string caller = "") => 
        logs.Enqueue(new(level, $"[{DateTime.Now:HH:mm:ss}] [{level.text}] [{caller}] {msg}"));

    record PrintedLog
    {
        public PrintedLog(Level level, string msg)
        {
            this.Level = level;
            this.Msg = msg;
        }

        public Level Level { private set; get; }
        public string Msg { private set; get; }
    }

    public record Level
    {
        public static readonly Level INFO = new("INFO", ConsoleColor.White, null);
        public static readonly Level DEBUG = new("DEBUG", ConsoleColor.DarkGray, null);
        public static readonly Level WARNING = new("WARNING", ConsoleColor.Yellow, null);
        public static readonly Level ERROR = new("ERROR", ConsoleColor.Red, null);
        public static readonly Level FATAL = new("FATAL", ConsoleColor.White, ConsoleColor.Red);

        public readonly string? text;
        private readonly ConsoleColor foreground = ConsoleColor.White;
        private readonly ConsoleColor? background;

        public Level(string text, ConsoleColor foreground, ConsoleColor? background)
        {
            this.text = text;
            this.foreground = foreground;
            this.background = background;
        }

        public void Init()
        {
            Console.ForegroundColor = foreground;
            if (background is not null) Console.BackgroundColor = (ConsoleColor)background;
        }
    }
}
