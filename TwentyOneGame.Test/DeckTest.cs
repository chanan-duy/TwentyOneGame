namespace TwentyOneGame.Test;

public class DeckTest
{
    [Test]
    public void ConstructorTest()
    {
        var deck = new Deck();

        Assert.That(deck, Is.Not.EqualTo(null));
    }

    [Test]
    public void FillTest()
    {
        var deck = new Deck();
        deck.FillCardsRandomized();

        Assert.That(deck.Cards, Is.All.Not.EqualTo(null));
    }
}
