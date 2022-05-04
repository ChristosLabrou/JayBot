using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using DSharpPlus.Entities;
using System.Collections.Generic;
using System;

namespace JayBot.Commands
{
	public class GeneralCommands : BaseCommandModule
	{
		/*
		[Command("debug")]
		[Description("Debug")]
		public async Task Debug(CommandContext ctx, params string[] text)
		{
			string message = ctx.Message.Content;
			//message = Regex.Replace(message, @"(!|<@934181567931236393>)\s*say", " ");
			//string message = string.Join(" ", text);
			await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);
			//await ctx.Message.DeleteAsync();
		}
		*/

		[Command("say")]
		[Description("Posts input (without the bot command)")]
		public async Task Say(CommandContext ctx, params string[] text)
		{
			string message = ctx.Message.Content;
			message = Regex.Replace(message, @"(!|<@934181567931236393>)\s*say", " ");
			//string message = string.Join(" ", text);
			await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);
			await ctx.Message.DeleteAsync();
		}
		[Command("source")]
		[Description("Posts link to source code.")]
		public async Task Source(CommandContext ctx)
		{
			await ctx.Channel.SendMessageAsync("GitHub link: https://github.com/ChristosLabrou/JayBot").ConfigureAwait(false);
		}
	
		[Command("dm")]
		[Description("Sends DM to target. Only Chris can use it")]
		public async Task DM(CommandContext ctx, DiscordMember target, params string[] text){
			if (ctx.Member.Id == 259995873923039233){
				string message = string.Join(" ", text);
				await target.SendMessageAsync(message).ConfigureAwait(false);
				await ctx.Message.DeleteAsync();
			}			
		}
	
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


		[Command("activity")]
		[Description("Sets bot activity. Only Chris can use it.")]
		public async Task Activity(CommandContext ctx, params string[] status)
		{
			if (ctx.User.Id == 259995873923039233)
			{
				DiscordActivity activity = new DiscordActivity();
				var client = ctx.Client;
				string merged = string.Join(" ", status);
				activity.Name = merged;
				await client.UpdateStatusAsync(activity).ConfigureAwait(false);
			}

		}

		[Command("pingspam")]
		public async Task PingSpam(CommandContext ctx, DiscordMember target, params string[] spam)
		{
			if (ctx.User.Id == 259995873923039233)
			{
				string message = string.Join(" ", spam);
				await ctx.Message.DeleteAsync();
				while (true)
				{
					await target.SendMessageAsync(message).ConfigureAwait(false);
				}
			}
		}

		[Command("roll")]
		public async Task Roll(CommandContext ctx, string die)
		{
			var rng = new Random();
			int sum = 0;
			int extra = 0;
			string[] words = die.Split('d', '+');
			int diceAmount = Int32.Parse(words[0]);
			int diceSize = Int32.Parse(words[1]);
			int[] rolledDice = new int[diceAmount];


			if (words.Length == 3)
			{
				extra = Int32.Parse(words[2]);
			}
			string reply = "**Result**: (";
			for (int i = 0; i<diceAmount; i++)
			{
				rolledDice[i] = rng.Next(0, diceSize);
				sum += rolledDice[i];
				reply += rolledDice[i].ToString();
				if (i != diceAmount - 1)
				{
					reply += ", ";
				}
			}
			if (extra == 0)
			{
				reply += ")\n**Total**: " + sum.ToString();
			}
			else
			{
				reply += ") + " + extra.ToString() + "\n**Total**: " + (sum + extra).ToString();
			}
			
			await ctx.Channel.SendMessageAsync(reply).ConfigureAwait(false);

			//int number = rng.Next(0, Helpers.GetDiceSize(die));
			//await ctx.Channel.SendMessageAsync(number.ToString()).ConfigureAwait(false);
		}
	}
}
