using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;

namespace JayBot.Commands
{
	public class GeneralCommands : BaseCommandModule
	{
		[Command("say")]
		[Description("Posts input (without the bot command)")]
		public async Task Say(CommandContext ctx, params string[] text)
		{
			string message = ctx.Message.Content;
			await ctx.Channel.SendMessageAsync(message.Remove(0, 5)).ConfigureAwait(false);
			await ctx.Message.DeleteAsync();
		}
		[Command("source")]
		[Description("Posts link to source code.")]
		public async Task Source(CommandContext ctx)
		{
			await ctx.Channel.SendMessageAsync("GitHub link: https://github.com/ChristosLabrou/JayBot").ConfigureAwait(false);
		}

	}
}
