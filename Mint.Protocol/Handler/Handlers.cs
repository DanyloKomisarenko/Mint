using Mint.Common.Buffer;

namespace Mint.Protocol.Handler;

public class Handlers
{
    public static readonly Func<HandlerContext, ByteBuf, int> SEND_PING_RESPONSE = Register("SendPingResponse", (ctx, buf) => 0);

    private static readonly Dictionary<string, Func<HandlerContext, ByteBuf, int>> HANDLERS = new();
    static Func<HandlerContext, ByteBuf, int> Register(string name, Func<HandlerContext, ByteBuf, int> func) => HANDLERS[name] = func;
}
