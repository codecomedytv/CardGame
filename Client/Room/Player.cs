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
        public Label Damage;
        public Label Deck;
        public PanelContainer Discard;
        public HBoxContainer Units;
        public HBoxContainer Support;
        public HBoxContainer Hand;
        private AnimatedSprite PlayingState;

        public override void _Ready()
        {
            PlayingState = GetNode<AnimatedSprite>("View/PlayingState");
            Damage = GetNode<Label>("Damage");
            Deck = GetNode<Label>("Deck");
            Discard = GetNode<PanelContainer>("Discard");
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