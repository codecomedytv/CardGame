using CardGame.Client.Cards;
using Godot;

namespace CardGame.Client.Room
{
    public enum States
    {
        Acting,
        Active,
        Idle,
        Passing,
        Passive,
        Processing
    }
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Player: Control
    
    {
        public States State;
        public int DeckCount { get; set; }
        public readonly Label Damage;
        public readonly Zone Deck;
        public readonly Zone Discard;
        public readonly Zone Units;
        public readonly Zone Support;
        public readonly Zone Hand;
        private AnimatedSprite PlayingState;
        public bool IsInActive => State != States.Active && State != States.Idle;
        public bool Targeting;
        public bool Attacking;
        public Card CardInUse;

        public Player(View.Player view)
        {
            DeckCount = 40;
            Damage = view.Damage;
            Deck = new Zone(view.Deck);
            Discard = new Zone(view.Discard);
            Units = new Zone(view.Units);
            Support = new Zone(view.Support);
            Hand = new Zone(view.Hand);
            PlayingState = view.PlayingState;
        }
        
        public void SetState(States state)
        {
            State = state;
            if (state == States.Active || state == States.Idle)
            {
                PlayingState.Visible = true;
                PlayingState.Play();
            }

            else
            {
                PlayingState.Frame = 0;
                PlayingState.Visible = false;
                PlayingState.Stop();
            }
        }

        public bool CanPlay(Card card) => !Targeting && !Attacking && !IsInActive && card.CanBePlayed;
        public bool ActionInProgress => Targeting || Attacking;
        public bool IsChoosingTargets => Targeting && !IsInActive;
        public bool IsChoosingAttackTarget => Attacking && State == States.Idle;
    }

        
}