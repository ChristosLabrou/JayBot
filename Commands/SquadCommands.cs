using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace JayBot.Commands
{
	public class SquadCommands : BaseCommandModule
	{
		[Command("squad")]
		[Description("Debug version")]
		public async Task Squad(CommandContext ctx,
			string mode,
			string name,
			params DiscordMember[] discordMembers)
		{
			string replyText = "Squad not found";
			int squadIndex = Helpers.GuildIndex(name);
			bool isMember = Helpers.IsGuildMember(squadIndex, ctx.Member.Id.ToString());
			switch (mode.ToLower())
			{
				case "create":
					{
						if (squadIndex == -1)
						{
							replyText = string.Empty;
							List<string> tempList = new List<string>();
							
							for (int i = 0; i < discordMembers.Length; i++)
							{
								tempList.Add(discordMembers[i].Id.ToString());
								replyText += "<@" + discordMembers[i].Id.ToString() + ">\n";
							}
							Bot.squads.Add(new Squad(name, tempList, replyText));
							replyText = $"Squad {name} added with members:\n" + replyText;
							Helpers.SortSquadList();
						}
						else
						{
							replyText = "A squad with that name already exists";
						}
						break;
					}
				case "ping":
					{
						if (squadIndex != -1)
						{
							isMember = Helpers.IsGuildMember(squadIndex, ctx.Member.Id.ToString());
							if (isMember)
							{
								replyText = $"Squad {Bot.squads[squadIndex].name} you're summoned!\n" + Bot.squads[squadIndex].massPing;

							}
							else
							{
								replyText = "Only squad members are allowed to mass ping";
							}
						}
						break;
					}
				case "addmember":
					{
						if (squadIndex != -1)
						{
							if (isMember)
							{
								replyText = $"Members added to {Bot.squads[squadIndex].name} squad:\n";
								for (int i = 0; i < discordMembers.Length; i++)
								{
									Bot.squads[squadIndex].memberIDs.Add(discordMembers[i].Id.ToString());
									Bot.squads[squadIndex].massPing += "<@" + discordMembers[i].Id.ToString() + ">\n";
									replyText += "<@" + discordMembers[i].Id.ToString() + ">\n";
								}
							}
							else
							{
								replyText = "Access denied. Only guild members are allowed to add/remove members";
							}
						}
						break;
					}
				case "removemember":
					{
						if (squadIndex != -1)
						{
							if (isMember)
							{
								replyText = $"Members removed from {Bot.squads[squadIndex].name} guild:\n";
								for (int i = 1; i < discordMembers.Length; i++)
								{
									Bot.squads[squadIndex].memberIDs.Remove(discordMembers[i].Id.ToString());
									replyText += "<@" + discordMembers[i].Id.ToString() + ">\n";
								}
							}
							else
							{
								replyText = "Access denied. Only guild members are allowed to add/remove members";
							}
						}
						break;
					}
				case "delete":
					{
						if (squadIndex != -1)
						{
							if (isMember)
							{
								Bot.squads.RemoveAt(squadIndex);
								replyText = "Squad deleted";
							}
							else
							{
								replyText = "Access denied. Only members can delete a guild";
							}
						}
						break;
					}
			}
			string output = Newtonsoft.Json.JsonConvert.SerializeObject(Bot.squads);
			File.WriteAllText(Bot.squadJsonPath, output);

			await ctx.Channel.SendMessageAsync(replyText).ConfigureAwait(false);
		}

		[Command("squadlist")]
		public async Task SquadList(CommandContext ctx)
		{
			string replyText = string.Empty;
			for (int i =0; i < Bot.squads.Count; i++)
			{
				replyText += Bot.squads[i].name +"\n";
			}
			await ctx.Channel.SendMessageAsync(replyText).ConfigureAwait(false);
		}
	}
}