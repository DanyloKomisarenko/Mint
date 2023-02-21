using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mint.Protocol.Listener;
using Mint.Server.Config;
using Mint.Server;
using Mint.Common;
using Mint.Protocol.Database;
using Mint.Protocol.Pipeline;
using Mint.Server.Mock;
using Mint.Game;
using Mint.Game.Handler;

IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            // Dependencies
            services
                .AddSingleton<Mint.Common.Config.IConfiguration, MockConfiguration>()
                .AddSingleton<Logger>();

            // Game
            services
                .AddSingleton<GameServer>()
                .AddSingleton<PacketHandlers>();

            // Protocol
            services
                .AddSingleton<Server>()
                .AddSingleton<PacketListener>()
                .AddSingleton<PacketDatabase>()
                .AddSingleton<IPipelines, MockPipelines>();
        })
        .Build();

host.Services.GetService<Server>()?.Run();