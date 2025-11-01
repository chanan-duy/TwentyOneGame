namespace TwentyOneGame.Test;

public class MockUserIO : IUserIO
{
    private readonly Queue<string> _inputs = new();
    private readonly List<string> _outputs = [];

    public void PrintOut(string message)
    {
        _outputs.Add(message);
    }

    public void PrintNewLine()
    {
        _outputs.Add(Environment.NewLine);
    }

    public string Read()
    {
        return _inputs.Count > 0 ? _inputs.Dequeue() : string.Empty;
    }
}

[TestFixture]
public class GameTest
{
    private Game _game;
    private MockUserIO _mockIo;

    [SetUp]
    public void Setup()
    {
        _mockIo = new MockUserIO();
        _game = new Game(_mockIo);
    }

    [Test]
    public void InitHands_DealsTwoCardsToUserAndDealer()
    {
        _game.InitHands();

        Assert.Multiple(() =>
        {
            Assert.That(_game.UserHand, Has.Count.EqualTo(2));
            Assert.That(_game.DealerHand, Has.Count.EqualTo(2));
            Assert.That(_game.Deck.CurrentIndex, Is.EqualTo(4));
        });
    }

    [TestCaseSource(nameof(ScoreTestData))]
    public void CalculateScore_WithVariousHands_ReturnsCorrectScore(List<Card> hand, int expectedScore)
    {
        var score = Game.CalculateScore(hand);

        Assert.That(score, Is.EqualTo(expectedScore));
    }

    private static IEnumerable<object[]> ScoreTestData()
    {
        yield return [new List<Card> { new(Suit.Clubs, Rank.Six), new(Suit.Hearts, Rank.Ten) }, 16];
        yield return [new List<Card> { new(Suit.Clubs, Rank.Jack), new(Suit.Hearts, Rank.Queen), new(Suit.Diamonds, Rank.King) }, 9];
        yield return [new List<Card> { new(Suit.Spades, Rank.Ace), new(Suit.Hearts, Rank.Ten) }, 21];
        yield return [new List<Card> { new(Suit.Spades, Rank.Ace), new(Suit.Hearts, Rank.Ten), new(Suit.Diamonds, Rank.Seven) }, 18];
        yield return [new List<Card> { new(Suit.Spades, Rank.Ace), new(Suit.Hearts, Rank.Ace), new(Suit.Diamonds, Rank.Nine) }, 21];
        yield return [new List<Card> { new(Suit.Spades, Rank.Ace), new(Suit.Hearts, Rank.Ace), new(Suit.Diamonds, Rank.Ten) }, 12];
    }

    [Test]
    public void RecalculateState_UpdatesUserAndDealerScores()
    {
        _game.UserHand.Add(new Card(Suit.Clubs, Rank.Ten));
        _game.UserHand.Add(new Card(Suit.Clubs, Rank.Eight));
        _game.DealerHand.Add(new Card(Suit.Hearts, Rank.Jack));
        _game.DealerHand.Add(new Card(Suit.Hearts, Rank.Queen));

        _game.RecalculateState();

        Assert.Multiple(() =>
        {
            Assert.That(_game.UserScore, Is.EqualTo(18));
            Assert.That(_game.DealerScore, Is.EqualTo(5));
        });
    }
}
