using CardGame.Client.Library.Cards;
using Godot;

namespace CardGame.Client.Player
{
    public class Controller: Godot.Object
    {
        public readonly Model Model;
        public readonly View View;
        public readonly bool IsUser;
        public Controller Opponent;

        [Signal]
        public delegate void Executed();

        public Controller(View view, bool isUser)
        {
            IsUser = isUser;
            Model = new Model();
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
            Model.State = state;
            View.SetState(state);
        }
        
        public void Draw(Card card)
        {
            card.Player = this;
            Model.DeckCount -= 1;
            Model.Hand.Add(card);
            View.Draw(card, Model.DeckCount.ToString());
        }

        public void Deploy(Card card, bool isOpponent = false)
        {
            Model.Hand.Remove(card);
            Model.Units.Add(card);
            View.Deploy(card, isOpponent);
        }

        public void SetFaceDown(Card card)
        {
            Model.Hand.Remove(card);
            Model.Support.Add(card);
            View.SetFaceDown(card);
        }

        public void SetFaceDown()
        {
            var card = Model.Hand[0];
            Model.Hand.Remove(card);
            Model.Support.Add(card);
            View.SetFaceDown(card);
        }

        public void Activate(Card card)
        {
            View.Activate(card);
        }

        public void Activate(Card card, bool isOpponent)
        {
            // In future versions we may define zones as Dictionaries? Or store card index on the card object
            Model.Support.RemoveAt(0);
            Model.Support.Add(card);
            View.Activate(card, isOpponent);
        }

        public void Trigger(Card card)
        {
            View.Trigger(card);
        }

        public void Destroy(Card card)
        {
            Model.Units.Remove(card);
            Model.Graveyard.Add(card);
            View.Destroy(card);
        }
    }
    
    

    
}