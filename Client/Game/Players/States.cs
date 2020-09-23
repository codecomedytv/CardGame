using System;
using System.Diagnostics.Contracts;
using CardGame.Client.Game.Cards;

namespace CardGame.Client.Game.Players
{
    public enum States
    {
        // States shared by Server & Client
        Acting,
        Active,
        Idle,
        Passing,
        Passive,
        
        // ClientSide-Only States
        Targeting,
        Processing,
        GameOver
        
    }

    public class PlayerState
    {
        public event Action<States> StateChanged;
        
        private States InternalState = States.Passive;
        public States State { 
            get => InternalState;
            set => SetState(value);
        }
        public bool IsInActive => State != States.Active && State != States.Idle;
        public bool IsChoosingAttackTarget => Attacking && State == States.Idle;
        public bool Attacking = false;
        public Card CardInUse = null;

        private States SetState(States state)
        {
            InternalState = state;
            StateChanged?.Invoke(state);
            return state;
        }
    }
}