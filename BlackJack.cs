using System;
using System.Collections.Generic;

public enum Suit { Hearts, Diamonds, Clubs, Spades }
public enum Rank { Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace }

namespace BlackJack
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.StartGame();
        }
    }

    // 카드 한 장을 표현하는 클래스
    public class Card
    {
        public Suit Suit { get; private set; }
        public Rank Rank { get; private set; }

        public Card(Suit s, Rank r)
        {
            Suit = s;
            Rank = r;
        }

        public int GetValue()
        {
            if ((int)Rank <= 10)
            {
                return (int)Rank;
            }
            else if ((int)Rank <= 13)
            {
                return 10;
            }
            else
            {
                return 11;
            }
        }

        public override string ToString()
        {
            return $"{Suit} {Rank}";
        }
    }

    // 덱을 표현하는 클래스
    public class Deck
    {
        private List<Card> cards;

        public Deck()
        {
            cards = new List<Card>();

            foreach (Suit s in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank r in Enum.GetValues(typeof(Rank)))
                {
                    cards.Add(new Card(s, r));
                }
            }

            Shuffle();
        }

        public void Shuffle()
        {
            Random rand = new Random();

            for (int i = 0; i < cards.Count; i++)
            {
                int j = rand.Next(i, cards.Count);
                Card temp = cards[i];
                cards[i] = cards[j];
                cards[j] = temp;
            }
        }

        public Card DrawCard()
        {
            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }
    }

    // 패를 표현하는 클래스
    public class Hand
    {
        private List<Card> cards;

        public Hand()
        {
            cards = new List<Card>();
        }

        public void AddCard(Card card)
        {
            cards.Add(card);
        }

        public int GetTotalValue()
        {
            int total = 0;
            int aceCount = 0;

            foreach (Card card in cards)
            {
                if (card.Rank == Rank.Ace)
                {
                    aceCount++;
                }
                total += card.GetValue();
            }

            while (total > 21 && aceCount > 0)
            {
                total -= 10;
                aceCount--;
            }

            return total;
        }
    }

    // 플레이어를 표현하는 클래스
    public class Player
    {
        public Hand hand { get; private set; }

        public Player()
        {
            hand = new Hand();
        }

        public Card DrawCardFromDeck(Deck deck)
        {
            Card drawnCard = deck.DrawCard();
            hand.AddCard(drawnCard);
            return drawnCard;
        }
    }

    // 여기부터는 학습자가 작성
    // 딜러 클래스를 작성하고, 딜러의 행동 로직을 구현하세요.
    public class Dealer : Player
    {
        
    }

    // 블랙잭 게임을 구현하세요. 
    public class Game
    {
        public int turn;
        public Deck deck;
        public Player player;
        public Dealer dealer;

        public void StartGame()
        {
            Console.Clear();
            turn = 0;
            deck = new Deck();
            player = new Player();
            dealer = new Dealer();

            dealer.DrawCardFromDeck(deck);
            Console.SetCursorPosition(0, turn);
            Console.Write("플레이어의 드로우: " + player.DrawCardFromDeck(deck).ToString());
            Console.SetCursorPosition(35, turn++);
            Console.Write("딜러의 첫 카드는 미공개입니다.");
            Console.SetCursorPosition(0, turn);
            Console.Write("플레이어의 드로우: " + player.DrawCardFromDeck(deck).ToString());
            Console.SetCursorPosition(35, turn++);
            Console.Write("딜러의 드로우: " + dealer.DrawCardFromDeck(deck).ToString());

            Select();
        }

        public void Select()
        {
            Console.SetCursorPosition(0, turn);
            Console.Write("Hit? or Stay?: ");
            string selection = Console.ReadLine();
            switch(selection)
            {
                case "HIT":
                case "Hit":
                case "hit":
                    Hit();
                    break;
                case "STAY":
                case "Stay":
                case "stay":
                    Stay();
                    break;
                default:
                    Console.SetCursorPosition(0, turn);
                    Console.Write("Error. Re-Input             ");
                    Thread.Sleep(500);
                    Select();
                    break;
            }
        }

        public void Hit()
        {
            Console.SetCursorPosition(0, turn);
            Console.Write("플레이어의 드로우: " + player.DrawCardFromDeck(deck).ToString());
            Console.SetCursorPosition(35, turn++);
            Console.Write("딜러의 드로우: " + dealer.DrawCardFromDeck(deck).ToString());

            if (player.hand.GetTotalValue() < 21)
            {
                if (dealer.hand.GetTotalValue() < 17) Select();
                else EndGame();
            }
            else if (dealer.hand.GetTotalValue() < 17) Stay();
            else EndGame();
        }

        public void Stay()
        {
            Console.SetCursorPosition(35, turn++);
            Console.Write("딜러의 드로우: " + dealer.DrawCardFromDeck(deck).ToString());
            EndGame();
        }

        public void EndGame()
        {
            int pScore = player.hand.GetTotalValue();
            int dScore = dealer.hand.GetTotalValue();

            Console.SetCursorPosition(0, ++turn);
            Console.Write("플레이어의 점수: " + pScore);
            Console.SetCursorPosition(35, turn++);
            Console.Write("딜러의 점수: " + dScore);

            Console.SetCursorPosition(0, ++turn);
            if(pScore > 21) Console.WriteLine("플레이어 패배!");
            else if (dScore > 21) Console.WriteLine("플레이어 승리!");
            else if (pScore > dScore) Console.WriteLine("플레이어 승리!");
            else if (pScore < dScore) Console.WriteLine("플레이어 패배!");
            else Console.WriteLine("무승부!");

            Console.WriteLine("\n재시작을 원할 경우 Enter.");
            Console.ReadLine();
            StartGame();

            return;
        }
    }
}
