using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Globalization;

namespace Discord_Bot
{
    public class UtilitySample : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Summary("Tunjukkan ping bot saat ini.")]
        public async Task Ping()
            => await ReplyAsync($"Ping: {Context.Client.Latency}ms");

        [Command("avatar")]
        [Alias("getavatar")]
        [Summary("Dapatkan avatar pengguna.")]
        public async Task GetAvatar([Remainder]SocketGuildUser user = null)
        {
            if (user is null)
                user = Context.User as SocketGuildUser;

            await ReplyAsync($":frame_photo: **{user.Username}** avatar\n{Functions.GetAvatarUrl(user)}");
        }

        [Command("info")]
        [Alias("server", "serverinfo")]
        [Summary("Tamplikan informasi server.")]
        [RequireBotPermissions(GuildPermissions.EmbedLinks)]
        public async Task ServerEmbed()
        {
            double Percentage = Context.Guild.Users.Count(x => x.IsBot) / Context.Guild.MemberCount * 100d;
            double RoundedPercentage = Math.Round(Percentage, 2);

            EmbedBuilder embed = new EmbedBuilder();
            embed.WithColor(0, 225, 225)
                 .WithDescription(
                $"🏷️\n**Nama server:** {Context.Guild.Name}\n" +
                $"**Server ID:** {Context.Guild.Id}\n" +
                $"**Tanggal dibuat:** {Context.Guild.CreatedAt.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)}\n" +
                $"**Owner:** {Context.Guild.Owner}\n\n" +
                $"💬\n" +
                $"**User:** {Context.Guild.MemberCount - Context.Guild.Users.Count(x => x.IsBot)}\n" +
                $"**Bot:** {Context.Guild.Users.Count(x => x.IsBot)} [ {RoundedPercentage}% ]\n" +
                $"**Channel:** {Context.Guild.Channels.Count}\n" +
                $"**Roles:** {Context.Guild.Roles.Count}\n" + 
                $"**Emotes: ** {Context.Guild.Emotes.Count}\n\n" + 
                $"🌎 **Wilayah:** {Context.Guild.VoiceRegionId}\n\n" + 
                $"🔒 **Keamanan level:** {Context.Guild.VerificationLevel}")
                 .WithImageUrl(Context.Guild.IconUrl);

            await ReplyAsync($":information_source: server info untuk **{Context.Guild.Name}**", embed: embed.Build());
        }

        [Command("role")]
        [Alias("roleinfo")]
        [Summary("Menunjukan informasi tentang suatu peran.")]
        public async Task RoleInfo([Remainder]SocketRole role)
        {
            if (role == Context.Guild.EveryoneRole) return;

            await ReplyAsync(
                $":flower_playing_cards: **{role.Name}** information```ini" +
                $"\n[Members]             {role.Members.Count()}" +
                $"\n[Role ID]             {role.Id}" +
                $"\n[Hoisted status]      {role.IsHoisted}" +
                $"\n[Created at]          {role.CreatedAt.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)}" +
                $"\n[Hierarchy position]  {role.Position}" +
                $"\n[Color Hex]           {role.Color}```");
        }

        [Command("source")]
        [Alias("sourcecode")]
        [Summary("Tautkan kode sumber yang digunakan untuk bot ini.")]
        public async Task Source()
            => await ReplyAsync($":heart: **{Context.Client.CurrentUser}** didasarkan pada kode sumber ini:\nhttps://github.com/plengeh69/Discord-Bot-Csharp");            
    } 
}