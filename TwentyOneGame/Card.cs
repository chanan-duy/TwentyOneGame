namespace TwentyOneGame;

public class Card
{
    public readonly Suit Suit;
    public readonly Rank Rank;
    public readonly int Value;

    public Card(Suit suit, Rank rank)
    {
        Suit = suit;
        Rank = rank;
        Value = RankToValue(Rank);
    }

    private static int RankToValue(Rank rank)
    {
        return rank switch
        {
            Rank.Six => 6,
            Rank.Seven => 7,
            Rank.Eight => 8,
            Rank.Nine => 9,
            Rank.Ten => 10,
            Rank.Jack => 2,
            Rank.Queen => 3,
            Rank.King => 4,
            Rank.Ace => 11,
            _ => throw new ArgumentOutOfRangeException(nameof(rank), rank, null),
        };
    }
}

public enum Suit
{
    Hearts = 0,
    Diamonds,
    Clubs,
    Spades,
}

public enum Rank
{
    Ace = 0,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
}
