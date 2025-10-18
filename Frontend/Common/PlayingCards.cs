
namespace Frontend.Common
{
    public enum Suit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }

    public enum Rank
    {
        Ace = 1,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King
    }

    public class PlayingCard
    {
        public Suit Suit { get; set; }
        public Rank Rank { get; set; }
        public bool IsFaceUp { get; set; } = true;

        public int AceLowRank => (int)Rank;
        public int AceHighRank => Rank == Rank.Ace ? 14 : (int)Rank;

        public PlayingCard(Suit suit, Rank rank, bool isFaceUp = true)
        {
            Suit = suit;
            Rank = rank;
            IsFaceUp = isFaceUp;
        }

        public static PlayingCard Create(Suit suit, Rank rank, bool isFaceUp = true) => new PlayingCard(suit, rank, isFaceUp);
        public static PlayingCard CreateRandom(bool isFaceUp = true)
        {
            var rand = new Random();
            var suit = (Suit)rand.Next(0, 4);
            var rank = (Rank)rand.Next(1, 14);
            return new PlayingCard(suit, rank, isFaceUp);
        }

        public string GetImagePath()
        {
            var suitStr = Suit.ToString().ToLower();
            var rankStr = Rank switch
            {
                Rank.Ace => "ace",
                Rank.Jack => "jack",
                Rank.Queen => "queen",
                Rank.King => "king",
                _ => ((int)Rank).ToString()
            };

            return $"images/cards/{rankStr}_of_{suitStr}.png".ToLower();
        }

        public override string ToString()
        {
            return $"{Rank} of {Suit}";
        }

        public bool Equals(PlayingCard? other)
        {
            if (other is null) return false;
            return Suit == other.Suit && Rank == other.Rank;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not PlayingCard other) return false;
            return Suit == other.Suit && Rank == other.Rank;
        }

        public static bool operator ==(PlayingCard? left, PlayingCard? right) => Equals(left, right);
        public static bool operator !=(PlayingCard? left, PlayingCard? right) => !Equals(left, right);
    }

    public class PlayingCardDeck
    {
        private Stack<PlayingCard> _cards = [];

        public int Count => _cards.Count;

        public PlayingCardDeck() {}

        public static PlayingCardDeck CreateStandardDeck()
        {
            var deck = new PlayingCardDeck();
            deck.Reset();
            return deck;
        }

        public IEnumerable<PlayingCard> Cards => _cards;
        public PlayingCard? Pop() => _cards.Count > 0 ? _cards.Pop() : null;
        public void Push(PlayingCard card) => _cards.Push(card);
        public void Clear() => _cards.Clear();

        public void Reset()
        {
            _cards.Clear();
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    _cards.Push(new PlayingCard(suit, rank, isFaceUp: false));
                }
            }
        }

        public void Shuffle()
        {
            _cards = new Stack<PlayingCard>(_cards.OrderBy(_ => Guid.NewGuid()));
        }
    }
}
