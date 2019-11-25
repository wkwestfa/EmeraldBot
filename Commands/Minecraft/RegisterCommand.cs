using Discord;
using Discord.Commands;
using System.Linq;
using System.Threading.Tasks;

public class RegisterCommand : CommandBase
{
    [Command("RegisterMinecraft")]
    public async Task Register(string minecraftUsername)
    {
        var user = Bot.GetUser(Context.User.ToString());
        var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "minecraft verified");

        user.minecraftUsername = minecraftUsername;

        Bot.UpdateUsersFile();
        await (user as IGuildUser).AddRoleAsync(role);

        EmbedBuilder embedMessage = CreateEmbedMessage($"MINECRAFT USERNAME SET", $"{Context.User.Username} has set their minecraft username to: **{minecraftUsername}** {System.Environment.NewLine} {System.Environment.NewLine}" +
                                                                                  $"This information is case sensitive - Make sure it is set properly", Color.Blue);
        embedMessage.WithThumbnailUrl(Context.User.GetAvatarUrl());

        await Context.Channel.SendMessageAsync("", false, embedMessage.Build());
    }
}