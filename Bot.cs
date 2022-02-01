using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using JayBot.Commands;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace JayBot
{
	public class Bot
	{
		public DiscordClient Client { get; private set; }
		public CommandsNextExtension Commands { get; private set; }

		static public List<Question> questions = new List<Question>();	
		static public List<Question> serverQuestions = new List<Question>();
		static public List<Squad> squads = new List<Squad>();

		static private readonly string folderName = "/home/chris/repos/JayBot/DataFiles/";
		static public readonly string dataJsonPath = folderName + "game.json";
		static public readonly string serverQuestionJsonPath = folderName+"server.json";
		static public readonly string squadJsonPath = folderName+"squads.json";
		public void LoadJson()
		{
			using (StreamReader reader = new StreamReader(dataJsonPath))
			{
				string json = reader.ReadToEnd();
				questions = JsonConvert.DeserializeObject<List<Question>>(json);
			}
			using (StreamReader reader = new StreamReader(serverQuestionJsonPath)){
				string json = reader.ReadToEnd();
				serverQuestions = JsonConvert.DeserializeObject<List<Question>>(json);
			}
			using (StreamReader reader = new StreamReader(squadJsonPath)){
				string json = reader.ReadToEnd();
				squads = JsonConvert.DeserializeObject<List<Squad>>(json);
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
			//Commands.RegisterCommands<FunCommands>();
			Commands.RegisterCommands<FunCommands>();
			Commands.RegisterCommands<QuestionCommands>();
			Commands.RegisterCommands<SquadCommands>();

			await Client.ConnectAsync();
			await Task.Delay(-1);
		}

		private Task OnClientReady(object semder, ReadyEventArgs e)
		{
			return Task.CompletedTask;
		}
	}
}
