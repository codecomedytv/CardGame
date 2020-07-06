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
        public Zone Deck;
        public Zone Discard;
        public Zone Units;
        public Zone Support;
        public Zone Hand;
        private AnimatedSprite PlayingState;
        public  void Initialize(Control view)
        {
            PlayingState = view.GetNode<AnimatedSprite>("View/PlayingState");
            DeckCount = 40;
            Damage = view.GetNode<Label>("Damage");
            Deck = new Zone(view.GetNode<Container>("Deck"));
            Discard = new Zone(view.GetNode<Container>("Discard"));
            Units = new Zone(view.GetNode<Container>("Units"));
            Support = new Zone(view.GetNode<Container>("Support"));;
            Hand = new Zone(view.GetNode<Container>("Hand"));
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