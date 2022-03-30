using Newtonsoft.Json;
using System.Collections.Generic;

namespace JayBot
{
	public class Squad
	{
		public string name;
		public List<string> memberIDs;
		//public string massPing { get; set; }

		[JsonConstructor]
		public Squad(string name, List<string> memberIDs)
		{
			this.name = name;
			this.memberIDs = memberIDs;
		}

		public string GenerateMassPing()
		{
			string massPing = string.Empty;
			for (int i = 0; i<memberIDs.Count; i++)
			{
				massPing += "<@" + memberIDs[i] + ">\n";
			}
			return massPing;
		}
	}
}