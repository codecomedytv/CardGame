﻿using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public class GameInput: Node
    {
        [Signal]
        public delegate void Deploy();
        
        private Card MousedOverCard;
        public Player User;

        public override void _Input(InputEvent gameEvent)
        {
            if (gameEvent is InputEventMouseButton mb && mb.Doubleclick && MousedOverCard != null)
            {
                GD.Print($"Double Clicked {MousedOverCard.Id}");
            }
        }

        public void SubscribeTo(Card card)
        {
            card.Connect(nameof(Card.MouseOvered), this, nameof(OnMousedOverCard));
            card.Connect(nameof(Card.MouseOveredExit), this, nameof(OnMousedOverExitCard));
        }

        private void OnMousedOverCard(Card card)
        {
            MousedOverCard = card;
        }

        private void OnMousedOverExitCard(Card card)
        {
            // Make sure a different card hasn't already overriden it
            if (MousedOverCard == card)
            {
                MousedOverCard = null;
            }
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