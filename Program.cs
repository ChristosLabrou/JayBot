namespace JayBot
{
	class Program
	{
		static void Main(string[] args)
		{
			//Console.WriteLine("Hello World!");
			Bot bot = new Bot();
			bot.RunAsync().GetAwaiter().GetResult();
		}
	}
}

