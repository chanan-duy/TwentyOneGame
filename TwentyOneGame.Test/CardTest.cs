namespace TwentyOneGame.Test;

public class CardTest
{
    [Test]
    public void ConstructorTest()
    {
        const Suit suit = Suit.Diamonds;
        const Rank rank = Rank.Queen;

        var card = new Card(suit, rank);

        Assert.Multiple(() =>
        {
            Assert.That(card, Is.Not.EqualTo(null));
            Assert.That(card.Suit, Is.EqualTo(suit));
            Assert.That(card.Rank, Is.EqualTo(rank));
        });
    }
}
