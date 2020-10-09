using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord.Commands;
using Discord.Websocket;
using Newtonsoft.Json.Linq;
using Discord;
using System.Linq;
using Newtonsoft.Json;

namespace Discord_Bot
{
    public class CommandHandlingService
    {
        private readonly CommandService Commands;
        private readonly DiscordSocketClient Client;
        private readonly IServiceProvider Services;

        public CommandHandlingService(IServiceProvider services)
        {

            Commands = services.GetRequiredService<CommandService>();
            Client = services.GetRequiredService<DiscordSocketClient>();
            Services = services;

            Client.Ready += Client_Ready;
            Client.MessageReceived += HandleCommand;
            Client.JoinedGuild += SendJoinMessage;
        }

        public async Task HandleCommand(SocketMessage rawMessage)
        {
            if (rawMessage.Author.IsBot) return;
            if (!(rawMessage is SocketUserMessage message)) return;
            var Context = new SocketCommandContext(Client, message);

            int argPos = 0;

            JObject config = Functions.GetConfig();
            string[] prefixes = JsonConvert.DeserializeObject<string[]>(config["prefixes"].ToString());

            if (prefixes.Any(x => message.HasStringPrefix(x, ref argPos)) || message.HasMentionPrefix(Client, CurrentUser, ref argPos))
            {
                var result = await Commands.ExecuteAsync(Context, argPos, service);

                if (!result.IsSuccess && result.Error.HasValue)
                    await Context.Channel.SendMessageAsync($":x: {result.ErrorReason}");
            }
        }

        private async Task SendJoinMessage(SocketGuild guild)
        {
            foreach (var channel in guild.TextChannels.OrderBy(x => x.position))
            {
                JObject config = Functions.GetConfig();
                string joinMessage = config["join_message"]?.Value<string>();

                if (joinMessage == null || joinMessage == string.Empty) return;

                var botPerms = channel.GetPermissionOverwrite(Client.CurrentUser).GetValueorDefault();
                if (botPerms.SendMessages == PermValue.Deny) continue;
                try
                {
                    await channel.SendMessageAsync(joinMessage);
                    return;
                }
                catch { continue; }
            }
        }

        private async Task Client_Ready()
            => await Functions.SetBotStatus(Client);

        public async Task InitializeAsync()
            => await Commands.AddModuleAsync(Assembly.GetEntryAssembly(), Services);
    }
}