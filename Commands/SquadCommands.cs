using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JayBot.Commands
{
    public class SquadCommands : BaseCommandModule{
        [Command("guild")]
        [Description("wip")]
        public async Task SquadSubmit(CommandContext ctx, string mode, params string[] parameters){
            bool found = false;
            string replyText = "";
            if (mode.ToLower()=="submit" || mode.ToLower()=="add"){
                //We assume that guild name is parameters[0]
                List<string> tempList = new List<string>();
                //replyText += $"Squad {parameters[0]} added with members:\n";
                for(int i=1;i<parameters.Length;i++){
                    tempList.Add(parameters[i]);
                    replyText+="<@"+parameters[i]+">\n";
                }
                Bot.squads.Add(new Squad(parameters[0], tempList, replyText));

                string output = Newtonsoft.Json.JsonConvert.SerializeObject(Bot.squads);
				File.WriteAllText(Bot.squadJsonPath, output);
                await ctx.Channel.SendMessageAsync($"Guild {parameters[0]} added with members:\n"+replyText).ConfigureAwait(false);
            }
            else if (mode.ToLower()=="summon" || mode.ToLower()=="call" || mode.ToLower()=="ping"){
                for (int i=0;i<Bot.squads.Count;i++){
                    if (Bot.squads[i].name==parameters[0]){
                        found = true;
                        foreach(var member in Bot.squads[i].memberIDs){
                            if (ctx.Member.Id.ToString()==member){
                                await ctx.Channel.SendMessageAsync($"Guild {Bot.squads[i].name} you're summoned!\n"+Bot.squads[i].massPing).ConfigureAwait(false);
                                break;
                            }
                        }
                        break;
                    }
                }

                if (!found){
                    await ctx.Channel.SendMessageAsync("Guild not found. Either you're dumdum and fucked up the name or the guild doesn't exist").ConfigureAwait(false);
                }
            }
            else if (mode.ToLower()=="list"){
                for(int i =0; i<Bot.squads.Count;i++){
                    replyText += $"{i+1}) "+Bot.squads[i].name+"\n";
                }
                await ctx.Channel.SendMessageAsync(replyText).ConfigureAwait(false);
            }
            else if (mode.ToLower()=="delete"){
                bool isMember = false;
                foreach(var guild in Bot.squads){
                    if (guild.name == parameters[0]){
                        foreach(var member in guild.memberIDs){
                            if (ctx.Member.Id.ToString() == member){
                                isMember = true;
                                Bot.squads.Remove(guild);
                                break;
                            }
                        }
                        break;
                    }
                }
                if(!isMember){
                    replyText = "Access denied. Only members can delete a guild";
                }
                else{
                    replyText = "Guild deleted";
                }
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(Bot.squads);
				File.WriteAllText(Bot.squadJsonPath, output);
                await ctx.Channel.SendMessageAsync(replyText).ConfigureAwait(false);
            }
        }

    }
}