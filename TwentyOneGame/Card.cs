namespace TwentyOneGame;

public class Card
{
    public readonly Suit Suit;
    public readonly Rank Rank;

    public Card(Suit suit, Rank rank)
    {
        Suit = suit;
        Rank = rank;
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
