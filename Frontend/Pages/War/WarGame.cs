using Frontend.Common;

namespace Frontend.Pages.War
{
    public enum WarBattleOutcome
    {
        InProgress,
        War,
        OpponentWon,
        PlayerWon
    }

    public class WarGame
    {
        public WarPlayer Player { get; set; } = new WarPlayer();
        public WarPlayer Opponent { get; set; } = new WarPlayer();
        public int? WarExpectedStackCount { get; set; }
        public bool? GameResult { get; set; } = null;
        public int CardsDrawn { get; set; }

        public bool PlayerWon => GameResult == true;
        public bool PlayerLost => GameResult == false;
        public bool GameOver => GameResult.HasValue;

        public void Reset()
        {
            PlayingCardDeck? deck = PlayingCardDeck.CreateStandardDeck();
            deck.Shuffle();

            Player.Reset();
            Opponent.Reset();
            GameResult = null;
            WarExpectedStackCount = null;
            CardsDrawn = 0;

            for (int i = 0; deck.Count > 0; i++)
            {
                var card = deck.Pop();
                if (card == null) break;

                if (i % 2 == 0)
                {
                    Player.Deck.Push(card);
                }
                else
                {
                    Opponent.Deck.Push(card);
                }
            }
        }

        public void DrawCards()
        {
            // game over
            if (GameOver) return;

            // draw card or shuffle discard into deck
            var cardPlayer = Player.Draw();
            if (cardPlayer == null)
            {
                // no cards left -- lost
                OpponentWinStack();
                GameResult = false;
                return;
            }

            // draw card or shuffle discard into deck
            var cardOpp = Opponent.Draw();
            if (cardOpp == null)
            {
                // no cards left -- won
                PlayerWinStack();
                GameResult = true;
                return;
            }

            // face down if in war
            if (WarExpectedStackCount.HasValue && Player.Stack.Count < WarExpectedStackCount)
            {
                cardPlayer.IsFaceUp = false;
                cardOpp.IsFaceUp = false;
            }
            else
            {
                cardPlayer.IsFaceUp = true;
                cardOpp.IsFaceUp = true;
            }

            ++CardsDrawn;
        }

        public WarBattleOutcome GetOutcome()
        {
            // waiting for draws
            if (GameOver) return WarBattleOutcome.InProgress;
            if (Player.Stack.Count != Opponent.Stack.Count) return WarBattleOutcome.InProgress;
            if (WarExpectedStackCount.HasValue && Player.Stack.Count < WarExpectedStackCount.Value) return WarBattleOutcome.War;

            // check
            int rankPlayer = Player.Stack.First().AceHighRank;
            int rankOpp = Opponent.Stack.First().AceHighRank;

            if (rankOpp > rankPlayer)
            {
                // opp won
                return WarBattleOutcome.OpponentWon;
            }
            else if (rankOpp < rankPlayer)
            {
                // player won
                return WarBattleOutcome.PlayerWon;
            }
            else
            {
                // war
                return WarBattleOutcome.War;
            }
        }

        public WarBattleOutcome CheckBattle()
        {
            // waiting for draws
            if (GameOver) return WarBattleOutcome.InProgress;
            if (Player.Stack.Count != Opponent.Stack.Count) return WarBattleOutcome.InProgress;
            if (WarExpectedStackCount.HasValue && Player.Stack.Count < WarExpectedStackCount.Value) return WarBattleOutcome.War;

            // check
            int rankPlayer = Player.Stack.First().AceHighRank;
            int rankOpp = Opponent.Stack.First().AceHighRank;

            if (rankOpp > rankPlayer)
            {
                // opp won
                OpponentWinStack();
                return WarBattleOutcome.InProgress;
            }
            else if (rankOpp < rankPlayer)
            {
                // player won
                PlayerWinStack();
                return WarBattleOutcome.InProgress;
            }
            else
            {
                // war
                WarExpectedStackCount = Player.Stack.Count + 4;
                return WarBattleOutcome.War;
            }
        }

        public void CheckShuffle()
        {
            if (Player.Deck.Count == 0 && Player.Discard.Count > 0)
                Player.ShuffleDiscardIntoDeck();

            if (Opponent.Deck.Count == 0 && Opponent.Discard.Count > 0)
                Opponent.ShuffleDiscardIntoDeck();
        }
        
        private void PlayerWinStack()
        {
            foreach (var pair in Player.Stack.Zip(Opponent.Stack))
            {
                pair.First.IsFaceUp = true;
                pair.Second.IsFaceUp = true;
                Player.Discard.Push(pair.Second);
                Player.Discard.Push(pair.First);
            }
            
            Player.Stack.Clear();
            Opponent.Stack.Clear();
            WarExpectedStackCount = null;
        }
        
        private void OpponentWinStack()
        {
            foreach (var pair in Opponent.Stack.Zip(Player.Stack))
            {
                pair.First.IsFaceUp = true;
                pair.Second.IsFaceUp = true;
                Opponent.Discard.Push(pair.First);
                Opponent.Discard.Push(pair.Second);
            }
        
            Player.Stack.Clear();
            Opponent.Stack.Clear();
            WarExpectedStackCount = null;
        }
    }

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

        public PlayingCard? Draw()
        {
            // draw card or shuffle discard into deck
            var card = Deck.Pop();
            if (card == null)
            {
                ShuffleDiscardIntoDeck();
                card = Deck.Pop();
            }

            // no cards left -- lost
            if (card == null) return null;

            card.IsFaceUp = true;
            Stack.Push(card);
            return card;
        }

        public void ShuffleDiscardIntoDeck()
        {
            var cards = Discard.OrderBy(_ => Guid.NewGuid()).ToArray();
            Discard.Clear();

            foreach (var card in cards)
            {
                card.IsFaceUp = false;
                Deck.Push(card);
            }
        }
    }
}