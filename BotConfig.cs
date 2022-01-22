using Newtonsoft.Json;

namespace JayBot
{
	public struct BotConfig
	{
		[JsonProperty("token")]
		public string Token { get; private set; }
		[JsonProperty("prefix")]
		public string Prefix { get; private set; }
	}
}
