using Mint.Common.Config;
using Mint.Common;
using Mint.Protocol.Database;
using System.Net.Sockets;
using System.Net;

namespace Mint.Protocol.Listener;

public abstract class ListenerContext
{
    // Dependencies
    public IConfiguration config { internal set; get; }
    public Logger logger { internal set; get; }

    // Listener Data
    public CancellationTokenSource cancellation { internal set; get; }
    public CancellationTokenRegistration callback { internal set; get; }
    public IPEndPoint address { internal set; get; }
    public TcpListener listener { internal set; get; }
    public ListenerPipeline pipeline { internal set; get; }

    // Protocol Data
    public PacketDatabase database { internal set; get; }
    public string[] versions { internal set; get; }

    // State
    public bool running { internal set; get; } = false;
    public State state { internal set; get; }

    public ListenerContext(Action<ListenerContext> initializer, Action<ListenerPipeline> pipelineinitializer)
    {
        initializer.Invoke(this);
        pipelineinitializer.Invoke(pipeline);
    }

    public ListenerContext GetContext()
    {
        return this;
    }
}
