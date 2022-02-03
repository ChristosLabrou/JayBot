using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JayBot.Commands
{
	public class QuestionCommands : BaseCommandModule
	{
		[Command("submit")]
		[Description("Submits question to the list")]
		public async Task Submit(CommandContext ctx, string type, string text)
		{
			var jedi = ctx.Member.Guild.GetRole(777326202309836800);

			if (ctx.Member.Roles.Contains(jedi))
			{
				if (type.ToLower() == "game")
				{
					Bot.questions.Add(new Question(text, QuestionTypeEnum.Game));
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(Bot.questions);
					File.WriteAllText(Bot.dataJsonPath, output);
					await ctx.Channel.SendMessageAsync("Done").ConfigureAwait(false);
				}
				else if (type.ToLower() == "server")
				{
					Bot.serverQuestions.Add(new Question(text, QuestionTypeEnum.Server));
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(Bot.serverQuestions);
					File.WriteAllText(Bot.serverQuestionJsonPath, output);
					await ctx.Channel.SendMessageAsync("Done").ConfigureAwait(false);
				}
				else
				{
					await ctx.Channel.SendMessageAsync("Invalid question type. Valid types are: game, server").ConfigureAwait(false);
				}

			}
		}

		[Command("read")]
		[Description("Prints the entire list.\nFor debug purposes, only Chris can use it")]
		public async Task Read(CommandContext ctx)
		{
			var jedi = ctx.Member.Guild.GetRole(777326202309836800);
			if (ctx.Member.Id == 259995873923039233)
				//if (ctx.Member.Roles.Contains(jedi))
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
			string gameplay = "__Gameplay questions__\n";
			string server = "__Server questions__\n";

			if (ctx.Member.Roles.Contains(jedi))
			{
				var rng = new Random();
				for (var i = 0; i < number; i++)
				{
					int index = rng.Next(0, Bot.questions.Count);
					gameplay += $"{i + 1}) " + Bot.questions[index].text + "\n";
					index = rng.Next(0, Bot.serverQuestions.Count);
					server += $"{i + 1}) " + Bot.serverQuestions[index].text + "\n";

				}
				await ctx.Channel.SendMessageAsync(gameplay).ConfigureAwait(false);
				await ctx.Channel.SendMessageAsync(server).ConfigureAwait(false);
			}
		}


	}

}