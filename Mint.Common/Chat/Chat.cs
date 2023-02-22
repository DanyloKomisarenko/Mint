namespace Mint.Common.Chat;

public class Chat
{
    class ClickEvent
    {
        public readonly Action? open_url;
        public readonly Action? open_file;
        public readonly Action? run_command;
        public readonly Action? twitch_user_info;
        public readonly Action? suggest_command;
        public readonly Action? change_page;
        public readonly Action? copy_to_clipboard;
    }

    class HoverEvent
    {
        public readonly Action? show_text;
        public readonly Action? show_item;
        public readonly Action? show_entity;
        public readonly Action? show_achievment;
    }

    class Action
    {
        public readonly string? action;
        public readonly string? value;
    }

    // Shared
    public readonly bool bold = false;
    public readonly bool italic = false;
    public readonly bool underlined = false;
    public readonly bool strikethrough = false;
    public readonly bool obfuscated = false;
    public readonly string font = "minecraft:uniform";
    public readonly string? color;
    public readonly string? insertion;

    public readonly Chat[]? extra;

    // Text Component
    public readonly string? text;
    public bool IsText() => text is not null;
}
