namespace TwentyOneGame.Test;

public class MockUserIO : IUserIO
{
    public readonly Queue<string> Inputs = new();
    public readonly List<string> Outputs = [];

    public void PrintOut(string message)
    {
        Outputs.Add(message);
    }

    public void PrintNewLine()
    {
        Outputs.Add(Environment.NewLine);
    }

    public string Read()
    {
        return Inputs.Count > 0 ? Inputs.Dequeue() : string.Empty;
    }

    public void EnqueueInput(string s)
    {
        Inputs.Enqueue(s);
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

    private void SetupTestDeck(IEnumerable<Card> cards)
    {
        var cardArray = cards.ToArray();
        for (var i = 0; i < cardArray.Length; i++)
        {
            _game.Deck.Cards[i] = cardArray[i];
        }
    }

    [Test]
    public void InitGameTest()
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
    public void ScoreTest(List<Card> hand, int expectedScore)
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
    public void RecalculateStateTest()
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

    [Test]
    public void BustedDealerWinsTest()
    {
        SetupTestDeck([
            new Card(Suit.Hearts, Rank.Ten),
            new Card(Suit.Hearts, Rank.Six),
            new Card(Suit.Clubs, Rank.Eight),
            new Card(Suit.Clubs, Rank.Six),
            new Card(Suit.Spades, Rank.Seven),
        ]);
        _mockIo.EnqueueInput("h");

        _game.Start();

        Assert.Multiple(() =>
        {
            Assert.That(_game.UserScore, Is.EqualTo(25));
            Assert.That(_game.DealerScore, Is.EqualTo(12));
            Assert.That(_game.DealerHand, Has.Count.EqualTo(2));

            var output = string.Join(" ", _mockIo.Outputs);
            Assert.That(output, Does.Contain("You busted. Dealer wins"));
        });
    }

    [Test]
    public void PlayerStandsDealerBustsTest()
    {
        SetupTestDeck([
            new Card(Suit.Hearts, Rank.Ten),
            new Card(Suit.Diamonds, Rank.Ten),
            new Card(Suit.Clubs, Rank.Nine),
            new Card(Suit.Clubs, Rank.Six),
            new Card(Suit.Spades, Rank.Eight),
        ]);
        _mockIo.EnqueueInput("s");

        _game.Start();

        Assert.Multiple(() =>
        {
            Assert.That(_game.UserScore, Is.EqualTo(19));
            Assert.That(_game.DealerScore, Is.EqualTo(24));
            Assert.That(_game.DealerHand, Has.Count.EqualTo(3));

            var output = string.Join(" ", _mockIo.Outputs);
            Assert.That(output, Does.Contain("Dealer busted. You win"));
        });
    }

    [Test]
    public void PlayerWinsTest()
    {
        SetupTestDeck([
            new Card(Suit.Hearts, Rank.Ace),
            new Card(Suit.Diamonds, Rank.Ten),
            new Card(Suit.Clubs, Rank.Ten),
            new Card(Suit.Clubs, Rank.Nine),
        ]);

        _game.Start();

        Assert.Multiple(() =>
        {
            Assert.That(_game.UserScore, Is.EqualTo(21));
            Assert.That(_game.DealerScore, Is.EqualTo(19));

            var output = string.Join(" ", _mockIo.Outputs);
            Assert.That(output, Does.Contain("You win"));
        });
    }

    [Test]
    public void TieTest()
    {
        SetupTestDeck([
            new Card(Suit.Hearts, Rank.Ten),
            new Card(Suit.Diamonds, Rank.Ten),
            new Card(Suit.Clubs, Rank.Nine),
            new Card(Suit.Diamonds, Rank.Nine),
        ]);
        _mockIo.EnqueueInput("s");

        _game.Start();

        Assert.Multiple(() =>
        {
            Assert.That(_game.UserScore, Is.EqualTo(19));
            Assert.That(_game.DealerScore, Is.EqualTo(19));

            var output = string.Join(" ", _mockIo.Outputs);
            Assert.That(output, Does.Contain("A tie"));
        });
    }
}
