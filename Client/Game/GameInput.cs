using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public class GameInput: Object
    {
        [Signal]
        public delegate void Deploy();
        
        private Card MousedOverCard;
        public Player User;

        public void SubscribeTo(Card card)
        {
            card.Connect(nameof(Card.MouseOvered), this, nameof(OnMousedOverCard));
        }

        private void OnMousedOverCard(Card card)
        {
            // We probably don't need to connect an exit condition because
            // hovering over a new card replace this one
            MousedOverCard = card;
        }

        private void OnDoubleClicked(Card card)
        {
            if (!User.IsInActive)
            {
                TakeAction(card);
            }
        }

        private void TakeAction(Card card)
        {
            if (card.CanBeDeployed)
            {
                User.State = States.Processing;
                EmitSignal(nameof(Deploy), card.Id);
            }
        }
    }
}