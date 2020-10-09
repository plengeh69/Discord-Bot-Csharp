using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Discord_Bot
{
    public static class Functions
    {
        public static async Task SetBotStatus(DiscordSocketClient Client)
        {
            JObject config = GetConfig();

            // Get all needed JSON objects.
            string currently = config["currently"]?.Value<string>().ToLower();
            string statusText = config["playing_status"]?.Value<string>();
            string onlineStatus = config["status"]?.Value<string>().ToLower();

            // Set the online status.
            if (onlineStatus != null && onlineStatus != string.Empty)
            {
                UserStatus userStatus = onlineStatus switch
                {
                    "online" => UserStatus.Online,
                    "dnd" => UserStatus.DoNotDisturb,
                    "idle" => UserStatus.Idle,
                    "offline" => UserStatus.Invisible,
                    _ => throw new NotImplementedException("User status does not exist."), 
                };

                await Client.SetStatusAsync(userStatus);
                {Console.WriteLine("${DateTime.Now.TimeOfDay:hh\\:mm\\:ss} | Online status set | {userStatus}");
            }

            // Set the playing status.
            if (currently != null && currently != string.Empty &&
                statusText != null && statusText != string.Empty)
            {
                ActivityType acvtivity = currently switch
                {
                    "listening" => ActivityType.Listening,
                    "watching" => ActivityType.Watching,
                    "streaming" => ActivityType.Streaming,
                    "playing" => ActivityType.Playing,
                    _ => throw new NotImplementedException("Activity type does not exist."),
                };

                await Client.SetGameAsync(statusText, type: activity);
                Console.writeLine("${DateTime.Now.TimeOfDay:hh\\:mm\\:ss} | Playing status set | {activity}: {statusText}");
            }
        }

        public static JObject GetConfig()
        {
            // Get the config file.
            using StreamReader configjson = new StreamReader(Directory.GetCurrentDirectory() + @"/config.json");
                return (JObject)JsonConvert.DeserializeObject(configjson.ReadToEnd());
        }

        public static string GetAvatarUrl(SocketUser user)
        {
            return user.GetAvatarUrl(size: 1024) ?? user.GetDefaultAvatarUrl();
            // Get the avatar and resize it 1024x1024. if the user has no avatar, get the default discord logo.
        }
    }
}