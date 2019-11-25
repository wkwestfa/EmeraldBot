using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public static class Bot
{
    static JObject jsonUsers = new JObject();

    public static void WriteToTextFile(List<string> list, string filePath)
    {
        File.WriteAllText(filePath, string.Empty);

        using (StreamWriter file = new StreamWriter(filePath, true))
        {            
            foreach (var item in list)
            {
                file.WriteLine(item);
            }
        }
    }

    public static void UpdateUsersFile()
    {
        Constants.users = Constants.users.OrderByDescending(x => x.inviteCount).ToList();

        // serialize JSON directly to a file
        using (StreamWriter file = File.CreateText(@"Data/users.json"))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;

            serializer.Serialize(file, Constants.users);
        }
    }

    public static void UpdateInvitesFile()
    {
        var invites = Constants.invites.OrderByDescending(x => x.validUses).ToList();

        // serialize JSON directly to a file
        using (StreamWriter file = File.CreateText(@"Data/invites.json"))
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;

            serializer.Serialize(file, invites);
        }
    }

    /// <summary>
    /// Convert SocketGuildUser (Discord) to our custom User class and add to our user list.
    /// </summary>
    public static void AddUser(SocketGuildUser user)
    {
        List<string> userRoles = new List<string>();

        foreach (var role in user.Roles)
        {
            userRoles.Add(role.Name);
        }

        Constants.users.Add(new User
        {
            username = user.ToString(),
            shortUsername = user.Username,
            inviteCount = 0,
            minecraftUsername = "",
            minecraftRewards = new List<string>(),
            roles = userRoles
        });
    }

    /// <summary>
    /// Convert RestInviteMetadata (AKA: An invite made on discord) into our own custom invite to keep track of.
    /// </summary>
    public static void AddInvite(RestInviteMetadata invite)
    {
        Constants.invites.Add(new Invite
        {
            inviteId = invite.Id,
            username = invite.Inviter.ToString(),
            validUses = invite.Uses,
            invalidUses = 0,
            manualUses = 0
        });
    }

    public static User GetUser(string userName)
    {
        return Constants.users.Find(x => x.username.ToLower().Contains(userName.ToLower()));
    }

    public static List<User> GetUsersFromFile()
    {
        List<User> users = new List<User>();

        using (StreamReader reader = new StreamReader(@"Data/users.json"))
        {
            string jsonString = reader.ReadToEnd();

            users = JsonConvert.DeserializeObject<List<User>>(jsonString);
        }

        return users;
    }

    public static List<Invite> GetInvitesFromFile()
    {
        List<Invite> invites = new List<Invite>();

        using (StreamReader reader = new StreamReader(@"Data/invites.json"))
        {
            string jsonString = reader.ReadToEnd();

            invites = JsonConvert.DeserializeObject<List<Invite>>(jsonString);
        }

        return invites;
    }

    public static List<Reward> GetRewardsFromFile()
    {
        List<Reward> rewards = new List<Reward>();

        string[] lines = File.ReadAllLines(@"Data/MinecraftRewards.txt");
        
        foreach (string line in lines)
        {
            rewards.Add(new Reward
            {
                invitesRequired = Convert.ToInt16(line.Split(',')[0]),
                reward = line.Split(',')[1]
            });
        }

        return rewards;
    }

    public static List<string> GetRolesFromFile()
    {
        List<string> roles = new List<string>();

        string[] lines = File.ReadAllLines(@"Data/LeaderboardRole_BlackList.txt");

        foreach (var line in lines)
        {
            roles.Add(line);
        }

        return roles;
    }

    public static void AddRewards(User user)
    {
        foreach (var reward in Constants.rewards)
        {
            if(reward.invitesRequired == user.inviteCount)
            {
                user.minecraftRewards.Add(reward.reward);
            }
        }
    }

    public static async Task UpdateUser(SocketGuildUser oldUser, SocketGuildUser updatedUser)
    {
        GetUser(oldUser.Username).roles.Clear();

        foreach (var role in updatedUser.Roles)
        {
            GetUser(oldUser.Username).roles.Add(role.Name);
        }

        GetUser(oldUser.Username).username = updatedUser.ToString();
        GetUser(oldUser.Username).shortUsername = updatedUser.Username;

        UpdateUsersFile();
    }
}

