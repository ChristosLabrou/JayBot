using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JayBot.Commands
{
	public class FunCommands : BaseCommandModule
	{
		[Command("say")]
		[Description("Posts input (without the bot command)")]
		public async Task Say(CommandContext ctx, params string[] text)
		{
			string message = ctx.Message.Content;
			await ctx.Channel.SendMessageAsync(message.Remove(0, 5)).ConfigureAwait(false);
			await ctx.Message.DeleteAsync();
		}

		[Command("submit")]
		[Description("Submits question to the list")]
		public async Task Submit(CommandContext ctx, string text)
		{
			var jedi = ctx.Member.Guild.GetRole(777326202309836800);

			if (ctx.Member.Roles.Contains(jedi))
			{
				Bot.questions.Add(new Question(text));
				string output = Newtonsoft.Json.JsonConvert.SerializeObject(Bot.questions);
				File.WriteAllText(Bot.dataJsonPath, output);
				await ctx.Channel.SendMessageAsync("Done").ConfigureAwait(false);
			}
		}

		[Command("read")]
		[Description("Prints the entire list.\nFOR DEBUG PURPOSES")]
		public async Task Read(CommandContext ctx)
		{
			var jedi = ctx.Member.Guild.GetRole(777326202309836800);

			if (ctx.Member.Roles.Contains(jedi))
				for (var i = 0; i < Bot.questions.Count; i++)
				{
					await ctx.Channel.SendMessageAsync($"{i + 1}) " + Bot.questions[i].text).ConfigureAwait(false);
				}
		}

		[Command("size")]
		[Description("Prints the list length.")]
		public async Task Size(CommandContext ctx)
		{
			//var jedi = ctx.Member.Guild.GetRole(777326202309836800);

			//if (ctx.Member.Roles.Contains(jedi))

			await ctx.Channel.SendMessageAsync($"{Bot.questions.Count}").ConfigureAwait(false);

		}

		[Command("question")]
		[Description("Prints <number> randomly chosen questions from the list. Doesn't reroll duplicates")]
		public async Task Question(CommandContext ctx, int number)
		{
			var jedi = ctx.Member.Guild.GetRole(777326202309836800);

			if (ctx.Member.Roles.Contains(jedi))
			{
				var rng = new Random();
				for (var i = 0; i < number; i++)
				{
					int index = rng.Next(0, Bot.questions.Count);
					await ctx.Channel.SendMessageAsync(Bot.questions[index].text).ConfigureAwait(false);
				}
			}
		}
	}
}
