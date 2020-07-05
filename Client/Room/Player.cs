using System;
using System.Threading.Tasks;
using CardGame.Client.Library;
using CardGame.Client.Library.Cards;
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
        public int DeckCount = 40;
        public int Health = 8000;
        public Label Damage;
        public Label Deck;
        public PanelContainer Discard;
        private Label DiscardCount;
        public HBoxContainer Units;
        public HBoxContainer Support;
        public HBoxContainer Hand;
        private readonly Tween Gfx = new Tween();
        private float Delay = 0.0F;
        private AnimatedSprite PlayingState;
        public Player Opponent;

        public override void _Ready()
        {
            AddChild(Gfx);
            PlayingState = GetNode<AnimatedSprite>("View/PlayingState");
            Damage = GetNode<Label>("Damage");
            Deck = GetNode<Label>("Deck");
            Discard = GetNode<PanelContainer>("Discard");
            DiscardCount = GetNode<Label>("Discard/Count");
            Units = GetNode<HBoxContainer>("Units");
            Support = GetNode<HBoxContainer>("Support");
            Hand = GetNode<HBoxContainer>("Hand");
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

    }

        
}