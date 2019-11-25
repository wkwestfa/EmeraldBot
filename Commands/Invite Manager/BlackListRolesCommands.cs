using Discord;
using Discord.Commands;
using System.Threading.Tasks;

public class BlackListRolesCommands : CommandBase
{
    [Command("ShowBlackListedRoles")]
    public async Task ShowRoles()
    {
        await SendMessage("BLACKLISTED ROLES", Constants.blacklistedLeaderboardRoles, Color.DarkerGrey);
    }

    [Command("AddRole")]
    public async Task AddRole(string role)
    {
        if(Constants.blacklistedLeaderboardRoles.Find(x => x == role) != null)
        {
            await SendMessage("ROLE ALREADY EXISTS", $"The role {role} already exists in the blacklist table.", Color.Red);
            return;
        }

        Constants.blacklistedLeaderboardRoles.Add(role);
        await SendMessage("ROLE ADDED", $"The role {role} was added to the blacklist table.", Color.Blue);

        Bot.WriteToTextFile(Constants.blacklistedLeaderboardRoles, @"Data/LeaderboardRole_BlackList.txt");
    }

    [Command("RemoveRole")]
    public async Task RemoveRole(string role)
    {
        var removedRole = Constants.blacklistedLeaderboardRoles.Find(x => x == role);

        if(removedRole == null)
        {
            await SendMessage("ROLE NOT FOUND", $"The role {role} was not found in the table and therefore cannot be removed.", Color.Red);
            return;
        }

        Constants.blacklistedLeaderboardRoles.Remove(removedRole);
        await SendMessage("ROLE REMOVED", $"The role {role} was removed from the blacklist table.", Color.Blue);

        Bot.WriteToTextFile(Constants.blacklistedLeaderboardRoles, @"Data/LeaderboardRole_BlackList.txt");
    }
}

