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

	/*
		[Command("squad")]
		[Description("WIP")]
		public async Task Squad(CommandContext ctx, params uint[] IDs){
			await ctx.Channel.SendMessageAsync("Work in Progress. Begone thot!").ConfigureAwait(false);
			

		}
	*/
	}
}
