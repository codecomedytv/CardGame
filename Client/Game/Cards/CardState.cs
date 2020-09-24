using System;
using CardGame.Client.Game.Players;

namespace CardGame.Client.Game.Cards
{
    public class CardState
    {
        
        // We require a base state because additional checks are required clientside
        // For example, when a user takes an action (say like choosing an attack target), then the base card state..
        // ..wouldn't change, which means it would still be able to be view as "playable" even though the player is..
        // ..is the wrong state to take that action..
        // ..HOWEVER we can't just modify the baseState directly because we wouldn't be able to get it until..
        // ..the server pushes out another update. So instead we use a property accessor via the public state property..
        // ..to make those additional checks for us on user input without actual modifying the base state.
        
        private readonly Card Card;
        private CardStates BaseState;
        public CardStates State { get; set; }

        public CardState(Card card)
        {
            Card = card;
        }
        
        private CardStates SetState(CardStates newState)
        {
            BaseState = newState;
            return newState;
        }

        private CardStates GetState()
        {
            if (CanBeDeployed())
            {
                return CardStates.CanBeDeployed;
            }
            if (CanBeSet())
            {
                return CardStates.CanBeSet;
            }
            if (CanBeActivated())
            {
                return CardStates.CanBeActivated;
            }
            if (CanAttack())
            {
                return CardStates.CanAttack;
            }
            if (CanAttackDirectly())
            {
                return CardStates.CanAttackDirectly;
            }

            return CardStates.Passive;
        }

        private bool CanBeDeployed()
        {
            return State == CardStates.CanBeDeployed && Card.Controller.PlayerState.State == States.Idle;
        }

        private bool CanBeSet()
        {
            return State == CardStates.CanBeSet && Card.Controller.PlayerState.State == States.Idle;
        }

        private bool CanBeActivated()
        {
            return State == CardStates.CanBeActivated && !Card.Controller.PlayerState.IsInActive;
        }

        private bool CanAttack()
        {
            return State == CardStates.CanAttack && Card.ValidAttackTargets.Count > 0 &&
                   Card.Controller.PlayerState.State == States.Idle;
        }

        private bool CanAttackDirectly()
        {
            return State == CardStates.CanAttackDirectly && Card.Controller.PlayerState.State == States.Idle;
        }

        public bool CanBePlayed()
        {
            return CanBeSet() || CanBeActivated() || CanBeDeployed() || CanAttack() || CanAttackDirectly();
        }
    }
}