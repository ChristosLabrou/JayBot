using System.IO;

namespace JayBot
{
	static class Helpers
	{
		public static bool IsGuildMember(int guildIndex, string memberID)
		{
			bool result = false;
			if (guildIndex != -1)
			{
				foreach (var member in Bot.squads[guildIndex].memberIDs)
				{
					if (memberID == member)
					{
						result = true;
						break;
					}

				}
			}
			return result;
		}

		public static void SortSquadList()
		{
			Bot.squads.Sort(delegate (Squad squad1, Squad squad2) { return squad1.name.CompareTo(squad2.name); });
			string output = Newtonsoft.Json.JsonConvert.SerializeObject(Bot.squads);
			File.WriteAllText(Bot.squadJsonPath, output);
		}
	}
}
