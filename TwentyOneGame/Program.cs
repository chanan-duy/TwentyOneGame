namespace TwentyOneGame;

internal static class Logic
{
    public static void Run()
    {
        var userIo = new UserInputOut();
        var shouldRun = true;
        while (shouldRun)
        {
            var game = new Game(userIo);
            game.Start();

            userIo.PrintNewLine();
            userIo.PrintNewLine();

            Console.WriteLine("Play again? (y/n)");
            var response = userIo.Read().ToLower();
            shouldRun = response == "y";
        }

        Console.WriteLine("Exiting...");
    }
}

internal static class Program
{
    private static void Main(string[] _)
    {
        Logic.Run();
    }
}
