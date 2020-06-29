using CardGame.Client.Library.Cards;
using Godot;

namespace CardGame.Client.Player
{
    public class Controller: Godot.Object
    {
        public readonly Model Model;
        public readonly View View;
        public readonly bool IsUser;

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
    }
    
    

    
}