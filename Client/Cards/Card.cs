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
        public bool IsInActive => State == CardStates.Passive || State == CardStates.Activated;
        public bool CanAttack => State == CardStates.CanAttack && Player.State == States.Idle && ValidAttackTargets.Count > 0;
        public bool CanTarget => State == CardStates.CanBeActivated && ValidTargets.Count > 0 && !Player.IsInActive;
        public bool HasTarget(Card target) => ValidTargets.Contains(target);
        public bool HasAttackTarget(Card defender) => ValidAttackTargets.Contains(defender);
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
        public void HighlightAsTarget() => View.ValidTarget.Visible = true;
        public void StopHighlightingAsTarget() => View.ValidTarget.Visible = false;

        public void HighlightTargets()
        {
            foreach (var target in ValidTargets) { target.HighlightAsTarget(); }
        }

        public void HighlightAttackTargets()
        {
            foreach (var target in ValidAttackTargets) { target.HighlightAsTarget(); }
        }

        public void StopHighlightingTargets()
        {
            foreach (var target in ValidTargets) { target.StopHighlightingAsTarget(); }
        }

        public void StopHighlightingAttackTargets()
        {
            foreach (var target in ValidTargets) { target.StopHighlightingAsTarget(); }
        }

        public void AttackUnit(Card defending)
        {
            View.SelectedTarget.Visible = true;
            View.AttackIcon.Visible = true;
            defending.View.SelectedTarget.Visible = true;
            defending.View.DefenseIcon.Visible = true;
        }

        public override string ToString() => $"{Id} : {Title}";

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
