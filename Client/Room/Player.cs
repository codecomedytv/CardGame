using System.Collections.Generic;
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
        Targeting,
        Processing,
        GameOver
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
        private readonly Label Health;
        private readonly AnimatedSprite PlayingState;
        public bool IsInActive => State != States.Active && State != States.Idle;
        public bool Targeting;
        public bool Attacking;
        public Card CardInUse;
        public Player Opponent;
        public Sprite SelectedTarget;
        public Sprite ValidTarget;
        public readonly List<Card> Targets = new List<Card>();
        
        public void LoseLife(int lifeLost)
        {
            Health.Text = (Health.Text.ToInt() - lifeLost).ToString();
        }

        public void HighlightAsTarget()
        {
            ValidTarget.Visible = true;
        }

        public void StopHighlightingAsTarget()
        {
            ValidTarget.Visible = false;
        }

        public void ShowAsTargeted()
        {
            SelectedTarget.Visible = true;
        }

        public void StopShowingAsTargeted()
        {
            SelectedTarget.Visible = false;
        }

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
            Health = view.Health;
            ValidTarget = view.ValidTarget;
            SelectedTarget = view.SelectedTarget;
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