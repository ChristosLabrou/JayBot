using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace JayBot.Commands
{
	public class SquadCommands : BaseCommandModule
	{
		[Command("squad")]
		[Description("WIP documentation. Ask Chris in the meantime")]
		public async Task Squad(CommandContext ctx, string mode, params string[] parameters)
		{
			bool isMember;
			string replyText = "";
			if (mode.ToLower() == "submit" || mode.ToLower() == "create")
			{
				//We assume that guild name is parameters[0]
				List<string> tempList = new List<string>();
				for (int i = 1; i < parameters.Length; i++)
				{
					tempList.Add(parameters[i]);
					replyText += "<@" + parameters[i] + ">\n";
				}
				Bot.squads.Add(new Squad(parameters[0], tempList, replyText));

				string output = Newtonsoft.Json.JsonConvert.SerializeObject(Bot.squads);
				File.WriteAllText(Bot.squadJsonPath, output);
				await ctx.Channel.SendMessageAsync($"Guild {parameters[0]} added with members:\n" + replyText).ConfigureAwait(false);
			}
			else if (mode.ToLower() == "summon" || mode.ToLower() == "call" || mode.ToLower() == "ping")
			{
				var guildIndex = Helpers.GuildIndex(parameters[0]);
				if (guildIndex != -1)
				{
					isMember = Helpers.IsGuildMember(guildIndex, ctx.Member.Id.ToString());
					if (isMember)
					{
						replyText = $"Guild {Bot.squads[guildIndex].name} you're summoned!\n" + Bot.squads[guildIndex].massPing;

					}
					else
					{
						replyText = "Only guild members are allowed to mass ping";
					}
				}
				else
				{
					replyText = "Guild not found";
				}
				await ctx.Channel.SendMessageAsync(replyText).ConfigureAwait(false);

			}
			else if (mode.ToLower() == "list")
			{
				for (int i = 0; i < Bot.squads.Count; i++)
				{
					replyText += $"{i + 1}) " + Bot.squads[i].name + "\n";
				}
				await ctx.Channel.SendMessageAsync(replyText).ConfigureAwait(false);
			}
			else if (mode.ToLower() == "delete")
			{
				int guildIndex = Helpers.GuildIndex(parameters[0]);
				isMember = Helpers.IsGuildMember(guildIndex, ctx.Member.Id.ToString());

				if (guildIndex != -1)
				{
					replyText = "Guild not found";
				}
				else
				{
					if (isMember)
					{
						Bot.squads.RemoveAt(guildIndex);
						replyText = "Guild deleted";
					}
					else
					{
						replyText = "Access denied. Only members can delete a guild";
					}
				}
				string output = Newtonsoft.Json.JsonConvert.SerializeObject(Bot.squads);
				File.WriteAllText(Bot.squadJsonPath, output);
				await ctx.Channel.SendMessageAsync(replyText).ConfigureAwait(false);
			}
			else if (mode.ToLower() == "addmember")
			{
				var guidIndex = Helpers.GuildIndex(parameters[0]);
				isMember = Helpers.IsGuildMember(guidIndex, ctx.Member.Id.ToString());
				if (guidIndex != -1)
				{
					if (isMember)
					{
						replyText = $"Members added to {Bot.squads[guidIndex].name} guild:\n";
						for (int i = 1; i < parameters.Length; i++)
						{
							Bot.squads[guidIndex].memberIDs.Add(parameters[i]);
							Bot.squads[guidIndex].massPing += parameters[i] + "\n";
							replyText += "<@"+parameters[i] + ">\n";
						}
					}
					else
					{
						replyText = "Access denied. Only guild members are allowed to add/remove members";
					}
					
				}
				else
				{
					replyText = "Guild not found";
				}
				string output = Newtonsoft.Json.JsonConvert.SerializeObject(Bot.squads);
				File.WriteAllText(Bot.squadJsonPath, output);
				await ctx.Channel.SendMessageAsync(replyText).ConfigureAwait(false);
			}
			else if (mode.ToLower() == "removemember")
			{
				var guidIndex = Helpers.GuildIndex(parameters[0]);
				isMember = Helpers.IsGuildMember(guidIndex, ctx.Member.Id.ToString());

				if (guidIndex != -1)
				{
					if (isMember)
					{
						replyText = $"Members removed from {Bot.squads[guidIndex].name} guild:\n";
						for (int i = 1; i < parameters.Length; i++)
						{
							Bot.squads[guidIndex].memberIDs.Remove(parameters[i]);
							replyText += "<@" + parameters[i] + ">\n";
						}
					}
					else
					{
						replyText = "Access denied. Only guild members are allowed to add/remove members";
					}
				}
				else
				{
					replyText = "Guild not found";
				}
				string output = Newtonsoft.Json.JsonConvert.SerializeObject(Bot.squads);
				File.WriteAllText(Bot.squadJsonPath, output);
				await ctx.Channel.SendMessageAsync(replyText).ConfigureAwait(false);
			}
		}

	}
}