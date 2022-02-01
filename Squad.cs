using System.Collections.Generic;
using Newtonsoft.Json;

namespace JayBot{
    public class Squad{
        public string name;
        public List<string> memberIDs;

        [JsonConstructor]
        public Squad(string name, List<string> memberIDs){
            this.name = name;
            this.memberIDs = memberIDs;
        }
    }
}