using Mint.Common;
using System.Net.Sockets;
using System.Net;

namespace Mint.Protocol.Listener;

public class ClientHandler
{
    private readonly ListenerContext ctx;

    public ClientHandler(ListenerContext ctx)
    {
        this.ctx = ctx;
    }

    public void HandleClient(IAsyncResult ar)
    {
        try
        {
            ctx.logger.Debug("Opening connection . . .");
            ctx.listener.BeginAcceptTcpClient(HandleClient, ctx.listener);
            using var client = ctx.listener.EndAcceptTcpClient(ar);

            var sock = client.Client;
            if (sock is null) throw new NullReferenceException($"Failed to open socket");
            if (sock.RemoteEndPoint is not IPEndPoint connectaddress) throw new NullReferenceException($"Failed to fetch ip");
            ctx.logger.Info($"Succefully connected [IP: {connectaddress.Address}:{connectaddress.Port}]");

            sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            ctx.logger.Debug("Enabled Keep-Alive");

            while (sock.Connected && !ctx.cancellation.IsCancellationRequested)
            {
                using var stream = client.GetStream();
            }
        }
        catch (Exception ex)
        {
            ctx.logger.Fatal($"Listener failed: {ex}");
        }

        ctx.logger.Debug("Connection closed");
    }
}
