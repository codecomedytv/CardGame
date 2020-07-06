using System;
using System.Threading.Tasks;
using CardGame.Client.Library;
using CardGame.Client.Library.Cards;
using Godot;
using Object = Godot.Object;

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
        public Label Damage;
        public Label Deck;
        public PanelContainer Discard;
        public HBoxContainer Units;
        public HBoxContainer Support;
        public HBoxContainer Hand;
        private AnimatedSprite PlayingState;
        public  void Initialize(Control view)
        {
            PlayingState = view.GetNode<AnimatedSprite>("View/PlayingState");
            DeckCount = 40;
            Damage = view.GetNode<Label>("Damage");
            Deck = view.GetNode<Label>("Deck");
            Discard = view.GetNode<PanelContainer>("Discard");
            Units = view.GetNode<HBoxContainer>("Units");
            Support = view.GetNode<HBoxContainer>("Support");
            Hand = view.GetNode<HBoxContainer>("Hand");
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