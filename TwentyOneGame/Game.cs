using System.Text;

namespace TwentyOneGame;

public class Game
{
    private readonly IUserIO _userIo;
    public readonly Deck Deck = new();

    public readonly List<Card> UserHand = [];
    public readonly List<Card> DealerHand = [];

    public int UserScore { get; private set; }
    public int DealerScore { get; private set; }

    public Game(IUserIO userIo)
    {
        _userIo = userIo;
        Deck.FillCardsRandomized();
    }

    public void Start()
    {
        InitHands();
        RecalculateState();

        _userIo.PrintOut("---");
        _userIo.PrintOut($"Dealer is showing: {DealerHand[0].Rank} of {DealerHand[0].Suit}");
        _userIo.PrintNewLine();

        _userIo.PrintOut("Your hand:");
        PrintOutCards(UserHand);

        _userIo.PrintOut($"Your score: {UserScore}");
        _userIo.PrintNewLine();

        PlayerTurn();

        if (UserScore <= 21)
        {
            DealerTurn();
        }

        DetermineWinner();
    }

    private void PlayerTurn()
    {
        while (UserScore < 21)
        {
            _userIo.PrintOut("---");
            _userIo.PrintOut("Hit or Stand? (h/s)");
            var choice = _userIo.Read().ToLower();

            if (choice == "s")
            {
                _userIo.PrintOut("You stand");
                break;
            }

            if (choice == "h")
            {
                _userIo.PrintOut("You hit");
                var newCard = Deck.TakeCard();
                if (newCard != null)
                {
                    UserHand.Add(newCard);
                    RecalculateState();

                    _userIo.PrintNewLine();
                    _userIo.PrintOut("Your new hand:");
                    PrintOutCards(UserHand);

                    _userIo.PrintOut($"Your score: {UserScore}");
                    _userIo.PrintNewLine();
                }
            }
            else
            {
                _userIo.PrintOut("Invalid input. Enter 'h' or 's'");
            }
        }
    }

    private void DealerTurn()
    {
        _userIo.PrintOut("---");
        _userIo.PrintOut("Dealer's hand:");
        PrintOutCards(DealerHand);
        _userIo.PrintOut($"Dealer's score: {DealerScore}");
        _userIo.PrintNewLine();

        while (DealerScore < 17)
        {
            _userIo.PrintOut("Dealer hits");
            var newCard = Deck.TakeCard();
            if (newCard != null)
            {
                DealerHand.Add(newCard);
                RecalculateState();
                _userIo.PrintOut("Dealer's new hand:");
                PrintOutCards(DealerHand);

                _userIo.PrintOut($"Dealer's score: {DealerScore}");
                _userIo.PrintNewLine();
            }
            else
            {
                break;
            }
        }
    }

    private void DetermineWinner()
    {
        _userIo.PrintNewLine();
        _userIo.PrintOut("---");
        _userIo.PrintOut("End");
        _userIo.PrintOut($"Your final score: {UserScore}");
        _userIo.PrintOut($"Dealer's final score: {DealerScore}");

        if (UserScore > 21)
        {
            _userIo.PrintOut("You busted. Dealer wins");
        }
        else if (DealerScore > 21)
        {
            _userIo.PrintOut("Dealer busted. You win");
        }
        else if (UserScore > DealerScore)
        {
            _userIo.PrintOut("You win");
        }
        else if (DealerScore > UserScore)
        {
            _userIo.PrintOut("Dealer wins");
        }
        else
        {
            _userIo.PrintOut("A tie");
        }
    }

    public void RecalculateState()
    {
        UserScore = CalculateScore(UserHand);
        DealerScore = CalculateScore(DealerHand);
    }

    public void InitHands()
    {
        UserHand.Clear();
        DealerHand.Clear();

        UserHand.Add(Deck.TakeCard()!);
        DealerHand.Add(Deck.TakeCard()!);
        UserHand.Add(Deck.TakeCard()!);
        DealerHand.Add(Deck.TakeCard()!);
    }

    public static int CalculateScore(List<Card> cards)
    {
        var score = cards.Sum(c => c.Value);
        var aceCount = cards.Count(c => c.Rank == Rank.Ace);

        while (score > 21 && aceCount > 0)
        {
            score -= 10;
            aceCount--;
        }

        return score;
    }

    private void PrintOutCards(List<Card> cards)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            sb.AppendLine($"{i + 1}.  {card.Rank} of {card.Suit}, {card.Value}{(card.Rank == Rank.Ace ? "/1" : "")}");
        }

        _userIo.PrintOut(sb.ToString());
    }
}
