using System.Collections.Generic;
using System.Threading.Tasks;
using CardGame.Client.Library.Cards;
using CardGame.Client.Room;
using Godot;

namespace CardGame.Client.Players
{
    public class Player: Godot.Object
    
    {
        public States State;
        public int Health = 8000;
        public int DeckCount = 40;
        public IList<Card> Units = new List<Card>();
        public IList<Card> Support = new List<Card>();
        public IList<Card> Hand = new List<Card>();
        public IList<Card> Graveyard = new List<Card>();
        public readonly View View;
        public Player Opponent;

        [Signal]
        public delegate void Executed();

        public Player(View view)
        {
            View = view;
        }
        
        public void SetState(States state)
        {
            State = state;
            View.SetState(state);
        }
        
        public void Draw(Card card)
        {
            card.Player = this;
            DeckCount -= 1;
            Hand.Add(card);
            View.Draw(card, DeckCount.ToString());
        }

        public void Deploy(Card card, bool isOpponent = false)
        {
            Hand.Remove(card);
            Units.Add(card);
            View.Deploy(card, isOpponent);
        }

        public void SetFaceDown(Card card)
        {
            Hand.Remove(card);
            Support.Add(card);
            View.SetFaceDown(card);
        }

        public void SetFaceDown()
        {
            var card = Hand[0];
            Hand.Remove(card);
            Support.Add(card);
            View.SetFaceDown(card);
        }

        public void Activate(Card card)
        {
            View.Activate(card);
        }

        public void Activate(Card card, bool isOpponent)
        {
            // In future versions we may define zones as Dictionaries? Or store card index on the card object
            Support.RemoveAt(0);
            Support.Add(card);
            View.Activate(card, isOpponent);
        }

        public void Trigger(Card card)
        {
            View.Trigger(card);
        }

        public void Destroy(Card card)
        {
            Units.Remove(card);
            Graveyard.Add(card);
            View.Destroy(card);
        }

        public void SendCardToGraveyard(Card card)
        {
            // We're assuming support for now
            Support.Remove(card);
            Graveyard.Add(card);
            View.Destroy(card);
        }

        public void Battle(Card attacker, Card defender, bool isOpponent)
        {
            View.Battle(attacker, defender, isOpponent);
        }

        public void SendCardToZone(Card card, ZoneIds zoneId)
        {
            // Implement A Way To Move From Arrays
            // (Do we even need these this information anyway?)
            View.SendCardToZone(card, zoneId);
        }
    }
    
    

    
}