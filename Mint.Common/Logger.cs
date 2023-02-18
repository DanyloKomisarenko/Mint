﻿using Mint.Common.Config;
using System.Runtime.CompilerServices;

namespace Mint.Common;

public class Logger
{
    private readonly IConfiguration configuration;

    public Logger(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public void Info(string msg, [CallerMemberName] string caller = "") => Log(Level.INFO, msg, caller);
    public void Warning(string msg, [CallerMemberName] string caller = "") => Log(Level.WARNING, msg, caller);
    public void Error(string msg, [CallerMemberName] string caller = "") => Log(Level.ERROR, msg, caller);
    public void Fatal(string msg, [CallerMemberName] string caller = "") => Log(Level.FATAL, msg, caller);

    public void Debug(string msg, [CallerMemberName] string caller = "")
    {
        if (configuration.Debug()) Log(Level.DEBUG, msg, caller);
    }

    public void Log(Level level, string msg, [CallerMemberName] string caller = "") => Log("", level, msg, caller);

    void Log(string prefix, Level level, string msg, [CallerMemberName] string caller = "")
    {
        level.Init();
        Console.WriteLine($"{prefix}[{DateTime.Now:HH:mm:ss}] [{level.text}] [{caller}] {msg}");
        Console.ResetColor();
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
