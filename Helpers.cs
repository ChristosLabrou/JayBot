namespace JayBot
{
	static class Helpers
	{
		public static bool IsGuildMember(string guildName, string memberID)
		{
			var result = false;
			for (int i = 0; i < Bot.squads.Count; i++)
			{
				if (guildName == Bot.squads[i].name)
				{
					foreach (var member in Bot.squads[i].memberIDs)
					{
						if (member.ToString() == memberID)
						{
							result = true;
							break;
						}
					}
					break;
				}

			}
			return result;
		}

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


		public static bool GuildExists(string guildName)
		{
			bool result = false;
			foreach (var guild in Bot.squads)
			{
				if (guild.name == guildName)
				{
					result = true;
					break;
				}
			}

			return result;
		}

		//Returns guild index if the guild exists, otherwise returns -1
		public static int GuildIndex(string guildName)
		{
			int result = -1;
			for (int i = 0; i < Bot.squads.Count; i++)
			{
				if (guildName == Bot.squads[i].name)
				{
					result = i;
					break;
				}
			}
			return result;
		}


		public static bool GuildExistsAndUserIsGuildMember(string guildName, string memberID)
		{
			bool result = false;

			foreach (var guild in Bot.squads)
			{
				if (guild.name == guildName)
				{
					result = IsGuildMember(guildName, memberID);
				}
			}


			return result;
		}

	}
}
