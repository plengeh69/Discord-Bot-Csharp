using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Discord_Bot
{
    public class ModSample : ModuleBase<SocketCommandContex>
    {
        [Command("kick")]
        [Summary("Menendang pengguna dari server.")]
        [RequireBotPermissions(GuildPermissions.KickMembers)]
        [RequireUserPermissions(GuildPermissions.KickMembers)]
        public async Task Kick(SocketGuildUser targetUser, [Remainder]string reason = "No reason provided.")
        {
            await targetUser.KickAsync(reason);
            await ReplyAsync($"**{targetUser}** telah ditendang. Bye bye :wave:");
        }

        [Command("ban")]
        [Summary("Ban pengguna dari server.")]
        [RequireUserPermissions(GuildPermissions.BanMembers)]
        [RequireBotPermissions(GuildPermissions.BanMembers)]
        public async Task Ban(SocketGuildUser targetUser, [Remainder]string reason = "No reason provided.")
        {
            await Context.Guild.AddBanAsync(targetUser.Id, 0, reason);
            await ReplyAsync($"**{targetUser}** telah diban. Bye bye :wave:");
        }

        [Command("unban")]
        [Summary("Unban pengguna dari server.")]
        [RequireBotPermissions(GuildPermissions.BanMembers)]
        [RequireUserPermissions(GuildPermissions.BanMembers)]
        public async Task Unban(ulong targetUser)
        {
            await Context.Guild.RemoveBanAsync(targetUser);
            await Context.Channel.SendMessageAsync($"Pengguna telah diunbanned :clap:");
        }

        [Command("purge")]
        [Summary("Menghapus pesan secara masal dalam obrolan")]
        [RequireBotPermissions(GuildPermissions.ManageMessages)]
        [RequireUserPermissions(GuildPermissions.ManageMessages)]
        public async Task Purge(int delNumber)
        {
            var channel = Context.Channel as SocketTextChannel;
            var items = await Context.Channel.GetMessagesAsync(delNumber + 1).FlattenAsync();
            await channel.DeleteMessagesAsync(items);
        }

        [Command("reloadconfig")]
        [Summary("Muat ulang config dan terapkan perubahan.")]
        [RequireOwner]
        public async Task ReloadConfig()
        {
            await Functions.SetBotStatus(Context.Client);
            await ReplyAsync("Memuat ulang!");
        }
    }
}