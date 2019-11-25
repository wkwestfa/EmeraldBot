using Discord;
using Discord.Commands;
using System.Threading.Tasks;

public class InvitesCommands : CommandBase
{
    [Command("Invites")]
    public async Task Invites(string username)
    {
        var user = Bot.GetUser(username.ToLower());

        await SendMessage($"INVITES", $"{user.username.Split('#')[0]} has invited {user.inviteCount} members!", Color.DarkBlue);
    }

    [Command("AddInvites")]
    public async Task AddInvites(string username, int invitesToAdd)
    {
        if (!IsAdmin())
        {
            await SendMessage("NEED PROPER PERMISSIONS", "You do not have the proper permissions to use this command!", Color.Red);
            return;
        }

        var user = Bot.GetUser(username.ToLower());

        user.inviteCount += invitesToAdd;

        await SendMessage("INVITES ADDED", $"{username} has received {invitesToAdd} invites.  They now have {user.inviteCount} invites in total!", Color.Red);

        Bot.UpdateUsersFile();
    }

    [Command("RemoveInvites")]
    public async Task RemoveInvites(string username, int invitesToRemove)
    {
        if (!IsAdmin())
        {
            await SendMessage("NEED PROPER PERMISSIONS", "You do not have the proper permissions to use this command!", Color.Red);
            return;
        }

        var user = Bot.GetUser(username.ToLower());

        user.inviteCount -= invitesToRemove;

        await SendMessage("INVITES REMOVED", $"{username} has lost {invitesToRemove} invites.  They now have {user.inviteCount} invites in total.", Color.Red);

        Bot.UpdateUsersFile();
    }

    [Command("SetInvites")]
    public async Task SetInvites(string username, int invitesValue)
    {
        if (!IsAdmin())
        {
            await SendMessage("NEED PROPER PERMISSIONS", "You do not have the proper permissions to use this command!", Color.Red);
            return;
        }

        var user = Bot.GetUser(username.ToLower());

        user.inviteCount = invitesValue;

        await SendMessage("INVITES SET", $"{username} number of invites has been set to {invitesValue} invites.  They now have {user.inviteCount} invites in total.", Color.Red);

        Bot.UpdateUsersFile();
    }

    [Command("ResetInvites")]
    public async Task ResetInvites(string username)
    {
        if (!IsAdmin())
        {
            await SendMessage("NEED PROPER PERMISSIONS", "You do not have the proper permissions to use this command!", Color.Red);
            return;
        }

        var user = Bot.GetUser(username.ToLower());

        user.inviteCount = 0;

        await SendMessage("INVITES RESET", $"You have reset {username}'s invites.  They now have {user.inviteCount} invites.", Color.Red);

        Bot.UpdateUsersFile();
    }
}

