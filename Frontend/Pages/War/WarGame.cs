using Frontend.Common;

namespace Frontend.Pages.War
{
    public class WarPlayer
    {
        public PlayingCardDeck Deck = new PlayingCardDeck();
        public Stack<PlayingCard> Stack = new Stack<PlayingCard>();
        public Stack<PlayingCard> Discard = new Stack<PlayingCard>();

        public void Reset()
        {
            Deck.Clear();
            Stack.Clear();
            Discard.Clear();
        }
    }
}