using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mint.Protocol.Listener;
using Mint.Server.Config;
using Mint.Server;
using Mint.Common;

IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            services.AddSingleton<Mint.Common.Config.IConfiguration, MockConfiguration>()
            .AddSingleton<Logger>()
            .AddSingleton<PacketListener>()
            .AddSingleton<Server>();
        })
        .Build();
await host.RunAsync();