using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using DSharpPlus.Entities;
using System.Collections.Generic;

namespace JayBot.Commands
{
	public class GeneralCommands : BaseCommandModule
	{
		[Command("say")]
		[Description("Posts input (without the bot command)")]
		public async Task Say(CommandContext ctx, params string[] text)
		{
			string message = ctx.Message.Content;
			message = Regex.Replace(message, @"!say", "");
			await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);
			await ctx.Message.DeleteAsync();
		}
		[Command("source")]
		[Description("Posts link to source code.")]
		public async Task Source(CommandContext ctx)
		{
			await ctx.Channel.SendMessageAsync("GitHub link: https://github.com/ChristosLabrou/JayBot").ConfigureAwait(false);
		}
	/*
		[Command("dm")]
		public async Task DM(CommandContext ctx, DiscordMember target, params string[] text){
			string message = ctx.Message.Content;
			int length = 1+2+3+1+ctx.Member.Id.ToString().Length+1; //magic numbers yay!
			message = message.Remove(0, length);
			await target.SendMessageAsync(message).ConfigureAwait(false);
			await ctx.Message.DeleteAsync();
		}
	*/
		[Command("servericon")]
		[Description("Posts server icon")]
		public async Task Banner(CommandContext ctx){
			await ctx.Channel.SendMessageAsync(ctx.Guild.IconUrl).ConfigureAwait(false);
		}


		[Command("title")]
		[Description("Create your own unique title for yourself or your character!")]
		public async Task Title(CommandContext ctx, 
		[Description("Determines output\n------------------\nValid choices:\n`add`: Adds text to your title\n`show`: Shows your current title\n`delete`: Deletes your existing title\n------------------")]string mode,
		[Description("Your title text (only relevant for the add mode)")]params string[] title){
			string path = "DataFiles/Titles/" + ctx.Member.Id.ToString() +".txt";
			string reply = string.Empty;

			switch (mode.ToLower()){
				case "add":
				{
					using (StreamWriter sw = File.AppendText(path)){
						foreach(string word in title){
							sw.Write(word+" ");
						}
						sw.Write(" ");
					}
					reply = "Title added/expanded";
					break;
				}
				case "show":
				{
					if(File.Exists(path)){
						reply = File.ReadAllText(path);
					}
					else{
						reply = "You don't have a title";
					}
					break;
				}
				case "delete":
				{
					File.Delete(path);
					reply = "Title deleted";
					break;
				}
			}
			await ctx.Channel.SendMessageAsync(reply).ConfigureAwait(false);
		}
	}
}
