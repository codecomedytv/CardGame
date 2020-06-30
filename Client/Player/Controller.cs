using System.Collections.Generic;
using CardGame.Client.Library.Cards;
using Godot;

namespace CardGame.Client.Player
{
    public class Controller: Godot.Object
    
    {
        public States State;
        public int Health = 8000;
        public int DeckCount = 40;
        public IList<Card> Units = new List<Card>();
        public IList<Card> Support = new List<Card>();
        public IList<Card> Hand = new List<Card>();
        public IList<Card> Graveyard = new List<Card>();
        
        //public readonly Model Model;
        public readonly View View;
        public readonly bool IsUser;
        public Controller Opponent;

        [Signal]
        public delegate void Executed();

        public Controller(View view, bool isUser)
        {
            IsUser = isUser;
            View = view;
        }

        public async void Execute()
        {
            View.Execute();
            await ToSignal(View, nameof(View.AnimationFinished));
            EmitSignal(nameof(Executed));
        }

        public void SetState(States state)
        {
            if (!IsUser)
            {
                return;
            }
            // We may need to queue or block this to avoid interrupting animations.
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
    }
    
    

    
}