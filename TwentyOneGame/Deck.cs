using System.Security.Cryptography;

namespace TwentyOneGame;

public class Deck
{
    private const int DeckSize = 4 * 9;
    public readonly Card[] Cards = new Card[DeckSize];
    public int CurrentIndex { get; private set; }

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

    public Card? TakeCard()
    {
        if (CurrentIndex >= Cards.Length)
        {
            return null;
        }

        var card = Cards[CurrentIndex];

        CurrentIndex += 1;

        return card;
    }
}
