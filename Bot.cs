using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using JayBot.Commands;
using System.Collections.Generic;

namespace JayBot
{
	public class Bot
	{
		public DiscordClient Client { get; private set; }
		public CommandsNextExtension Commands { get; private set; }

		static public List<Question> questions = new List<Question>();
		static public readonly string dataJsonPath = @"C:\Users\Chris\source\repos\DiscordBotSolution\DiscordBotProject\bin\data.json";
		//static public DSharpPlus.Entities.DiscordRole JediRole;

		public void LoadJson() 
		{
			using (StreamReader reader = new StreamReader(dataJsonPath))
			{
				string json = reader.ReadToEnd();
				questions = JsonConvert.DeserializeObject<List<Question>>(json);
			}

		}

		public async Task RunAsync()
		{
			
			var json = string.Empty;

			using (var fs = File.OpenRead("config.json"))
			using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
				json = await sr.ReadToEndAsync().ConfigureAwait(false);

			var configJson = JsonConvert.DeserializeObject<BotConfig>(json);

			var config = new DiscordConfiguration
			{
				Token = configJson.Token,
				TokenType = TokenType.Bot,
				AutoReconnect = true,
				MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug,
			};
			Client = new DiscordClient(config);
			LoadJson();
			Client.Ready += OnClientReady;

			var commandsConfig = new CommandsNextConfiguration
			{
				StringPrefixes = new string[] { configJson.Prefix },
				EnableDms = false,
				EnableMentionPrefix = true
			};

			Commands = Client.UseCommandsNext(commandsConfig);
			Commands.RegisterCommands<FunCommands>();

			await Client.ConnectAsync();
			await Task.Delay(-1);
		}

		private Task OnClientReady(object semder, ReadyEventArgs e)
		{
			return Task.CompletedTask;
		}
	}
}
