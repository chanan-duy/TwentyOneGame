namespace TwentyOneGame;

public class UserInputOut : IUserIO
{
    public void PrintOut(string message)
    {
        Console.WriteLine(message);
    }

    public void PrintNewLine()
    {
        Console.Write(Environment.NewLine);
    }

    public string Read()
    {
        return Console.ReadLine() ?? string.Empty;
    }
}
