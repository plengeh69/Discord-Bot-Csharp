using System;
using System.Threading.Tasks;
using Discord;
using Discord.Websocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace Discord_Bot
{
    public class Program
    {
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            using var services = ConfigureServices();
            
            Console.WriteLine("Ready for takeoff...");
            var Client = services.GetRequiredService<DiscordSocketClient>();

            Client.Log += LogAsync;
            services.GetRequiredService<CommandService>().Log += LogAsync;

            JObject config = Functions.GetConfig();
            string Token = config["token"].Value<string>();

            await Client.LoginAsync(TokenType.Bot, Token);
            await Client.StartAsync();

            await services.GetRequiredService<CommandHandlingService>().InitializeAsync();
            await Task.Delay(-1);
        }

        public ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
            .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {
                MessageCacheSize = 250,
                LogLevel = LogSeverity.Info
            }))
            .AddSingleton(new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Info,
                DefaultRunMode = RunMode.Async,
                CaseSensitiveCommands = false
            }))
            .AddSingleton<CommandHandlingService>()
            .BuildServiceProvider();
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }
    }
}