using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mint.Protocol.Listener;
using Mint.Server.Config;
using Mint.Server;
using Mint.Common;
using Mint.Protocol.Database;
using Mint.Server.Command;

IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            services
                .AddSingleton<Mint.Common.Config.IConfiguration, MockConfiguration>()
                .AddSingleton<Logger>();

            services.AddSingleton<Server>()
                .AddSingleton<PacketListener>()
                .AddSingleton<PacketDatabase>()
                .AddSingleton<CommandManager>();
        })
        .Build();

host.Services.GetService<Server>()?.Run();