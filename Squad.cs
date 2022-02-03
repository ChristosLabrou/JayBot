using System.Collections.Generic;
using Newtonsoft.Json;

namespace JayBot{
    public class Squad{
        public string name;
        public List<string> memberIDs;
        public string massPing {get;set;}

        //[JsonConstructor]
        public Squad(string name, List<string> memberIDs){
            this.name = name;
            this.memberIDs = memberIDs;
        }

        [JsonConstructor]
        public Squad(string name, List<string> memberIDs, string massPing){
            this.name = name;
            this.memberIDs = memberIDs;
            this.massPing = massPing;
        }
    }
}