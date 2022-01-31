using Newtonsoft.Json;

namespace JayBot
{
	public class Question
	{
		public string text;
		public QuestionTypeEnum type;

		//This attribute is required by Newtonsoft deserialization
		[JsonConstructor]
		public Question(string text, QuestionTypeEnum type){
			this.text = text;
			this.type = type;
		}
	}
}
