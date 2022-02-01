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
        [Command("squad")]
        [Description("wip")]
        public async Task SquadSubmit(CommandContext ctx, string mode, string name, params string[] members){
            bool found = false;
            if (mode.ToLower()=="submit"){
                List<string> tempList = new List<string>();
                for (int i=0;i<members.Length;i++){
                    tempList.Add(members[i]);
                }
                Bot.squads.Add(new Squad(name, tempList));
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(Bot.squads);
				File.WriteAllText(Bot.squadJsonPath, output);
                await ctx.Channel.SendMessageAsync("Squad added").ConfigureAwait(false);
            }
            else if (mode.ToLower()=="summon" || mode.ToLower()=="call"){
                string replyText = "";
                for(int i =0; i<Bot.squads.Count;i++){
                    if (Bot.squads[i].name == name){
                        found = true;
                        for (int j=0;j<Bot.squads[i].memberIDs.Count;j++){
                            replyText += "<@" + Bot.squads[i].memberIDs[j] + "> ";
                        }
                        replyText += "\nYou are summoned to wengaem";
                        break;
                    }
                }
                if(!found){
                    replyText = "Squad not found. Check for typos and if the squad exists. If it doesn't you can submit one";
                }
                await ctx.Channel.SendMessageAsync(replyText).ConfigureAwait(false);
            }
            else if (mode.ToLower()=="list"){
                string replyText = "";
                for(int i =0; i<Bot.squads.Count;i++){
                    replyText += Bot.squads[i].name;
                }
                await ctx.Channel.SendMessageAsync(replyText).ConfigureAwait(false);
            }

        }

    }
}