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

    [Test]
    public void TakeCardTest()
    {
        var deck = new Deck();
        deck.FillCardsRandomized();

        var firstCard = deck.Cards[0];
        var card = deck.TakeCard();

        Assert.Multiple(() =>
        {
            Assert.That(firstCard, Is.EqualTo(card));
            Assert.That(deck.CurrentIndex, Is.EqualTo(1));
        });
    }

    [Test]
    public void TakeCardAllAndNullTest()
    {
        var deck = new Deck();
        deck.FillCardsRandomized();

        var deckSize = deck.Cards.Length;
        for (var i = 0; i < deckSize; i++)
        {
            var takenCard = deck.TakeCard();
            Assert.That(deck.Cards[i], Is.EqualTo(takenCard));
        }

        var card = deck.TakeCard();
        Assert.That(card, Is.EqualTo(null));
    }
}
