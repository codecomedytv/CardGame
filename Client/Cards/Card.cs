using System.Collections.Generic;
using CardGame.Client.Room;
using Godot;

namespace CardGame.Client.Cards
{
    public class Card: Control
    {
        private static readonly PackedScene Scene = (PackedScene) GD.Load("res://Client/Cards/CardView.tscn");
        public readonly CardView View;
        public readonly int Id;
        public readonly CardTypes CardType;
        private CardStates _State = CardStates.Passive;
        public CardStates State
        {
            get => _State;
            set => _State = SetState(value);
        }
        public Player Player;
        public string Title;
        public string Art;
        public string Effect;
        public int Attack = 0;
        public int Defense = 0;
        public Vector2 Position 
        { 
            get => RectGlobalPosition;
            set => RectGlobalPosition = value;
        }

        public bool IsFaceUp => View.IsFaceUp;
        public int ChainIndex;
        public readonly List<Card> ValidTargets = new List<Card>();
        public readonly List<Card> ValidAttackTargets = new List<Card>();
        public bool IsTargeting = false;
        public Zone Zone;

        public Card(int id, CardInfo c)
        {
            Id = id;
            View = (CardView) Scene.Instance();
            (Title, Effect, Art, CardType, Attack, Defense) = (c.Title, c.Text, c.Art, c.Type, c.Attack, c.Defense);
        }

        private CardStates SetState(CardStates state)
        {
            DisplayState(state);
            return state;
        }

        private void DisplayState(CardStates state)
        { 
            if (IsInsideTree()) { View.Legal.Visible = (state != CardStates.Activated && state != CardStates.Passive);}
        }
        
        public void FlipFaceUp() => View.FlipFaceUp();
        public void FlipFaceDown() => View.FlipFaceDown();
        public void AddToChain() => View.AddToChain(ChainIndex.ToString());
        public void RemoveFromChain() => View.RemoveFromChain();
        
        public override string ToString() => $"{Id} : {Title}";
        
        public bool Targets()
        {
            return ValidTargets.Count > 0;
        }

        public bool Attacks()
        {
            return ValidAttackTargets.Count > 0;
        }


        public override void _Ready()
        {
            AddChild(View);
            DisplayState(State);
            RectSize = View.RectSize;
            RectMinSize = View.RectMinSize;
            View.Display(this);
        }
    }
}
