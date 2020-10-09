using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Discord_Bot
{
    public class FunSample : ModuleBase<SocketCommandsContext>
    {
        [Command("hello")] // command name.
        [Summary("Ucapkan hello ke bot.")] // summary of the command.
        public async Task Hello()
            => await ReplyAsync($"Hallo, **{Context.User.Username}**!");

        [Command("pick")]
        [Alias("choose")] // Aliases which will also trigger the command.
        [Summary("pick something.")]
        public async Task Pick([Remainder]string message = "")
        {
            string[] options = message.Split(new string[] { " or " }, StringSplitOptions.RemoveEmptyEntries); // Split the string at every ' or ' in the message.
            string selection = options[new Random().Next(options.Length)]; // Selec a random option.

            await ReplyAsync($"Saya memilih **{selection}**");


        }

        [Command("cookie")]
        [Summary("Beri seseorang kue.")]
        public async Task Cookie(SocketGuildUser user)
        {
            if (Context.Message.Author == user)
                await ReplyAsync($"{Context.User.Mention} tidak memiliki siapa pun untuk berbagi kue...:(");
            else
                await ReplyAsync($"{Context.User.Mention} berbagi kue dengan **{User.Username}** :cookie:");
        }

        [Command("amiadmin")]
        [Summary("Periksa status administrator anda")]
        public async Task AmIAdmin()
        {
            if ((Context.User as SocketGuildUser).GuildPermissions.Administrator)
                await ReplyAsync($"Iya, **{Context.User.Username}**, anda adalah seorang admin!");
            else
                await ReplyAsync($"Tidak, **{Context.User.Username}**, anda **bukan** admin!");
        }
    }
}