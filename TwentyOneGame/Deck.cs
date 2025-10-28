using System.Security.Cryptography;

namespace TwentyOneGame;

public class Deck
{
    public const int DeckSize = 4 * 9;
    public readonly Card[] Cards;

    public Deck()
    {
        Cards = new Card[DeckSize];
    }

    public void FillCardsRandomized()
    {
        var index = 0;
        foreach (var suit in Enum.GetValues<Suit>())
        {
            foreach (var rank in Enum.GetValues<Rank>())
            {
                Cards[index++] = new Card(suit, rank);
            }
        }

        RandomNumberGenerator.Shuffle(Cards.AsSpan());
    }
}
