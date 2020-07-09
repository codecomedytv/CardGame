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
        public CardStates State;
        public Player Player;
        public string Title;
        public string Art;
        public string Effect;
        public int Attack = 0;
        public int Defense = 0;
        public int ChainIndex;
        public readonly List<Card> ValidTargets = new List<Card>();
        public readonly List<Card> ValidAttackTargets = new List<Card>();
        public Vector2 Position { get => RectGlobalPosition; set => RectGlobalPosition = value; }
        public Zone Zone;

        public bool IsFaceUp => View.IsFaceUp;
        public bool IsInActive => State == CardStates.Passive || State == CardStates.Activated;
        public bool CanBeDeployed => State == CardStates.CanBeDeployed && Player.State == States.Idle;
        public bool CanBeSet => State == CardStates.CanBeSet && Player.State == States.Idle;
        public bool CanBeActivated => State == CardStates.CanBeActivated && !Player.IsInActive;
        public bool CanAttack => State == CardStates.CanAttack && ValidAttackTargets.Count > 0 && Player.State == States.Idle;
        public bool CanTarget => State == CardStates.CanBeActivated && ValidTargets.Count > 0 && !Player.IsInActive;
        public bool CanBePlayed => CanBeDeployed || CanBeSet || CanBeActivated; // can attack?
        public bool HasTarget(Card target) => ValidTargets.Contains(target);
        public bool HasAttackTarget(Card defender) => ValidAttackTargets.Contains(defender);
        
        public Card(int id, CardInfo c)
        {
            Id = id;
            View = (CardView) Scene.Instance();
            (Title, Effect, Art, CardType, Attack, Defense) = (c.Title, c.Text, c.Art, c.Type, c.Attack, c.Defense);
        }
        
        public override void _Ready()
        {
            AddChild(View);
            RectSize = View.RectSize;
            RectMinSize = View.RectMinSize;
            View.Display(this);
        }

        public override void _Process(float delta)
        {
            if (!IsInsideTree() || Player == null || !Player.IsUser)
            {
                return;
            }
            if (!Player.Targeting && !Player.Attacking && !Player.IsInActive && CanBePlayed)
            {
                View.Legal.Visible = true;
            }
            else
            {
                View.Legal.Visible = false;
            }

            if (!Player.Attacking && Player.AttackingCard == this)
            {
                HighlightAttackTargets();
            }
            else if(CanAttack && GetGlobalRect().HasPoint(GetGlobalMousePosition()))
            {
                HighlightAttackTargets();
            }
            else
            {
                StopHighlightingAttackTargets();
            }
            
            // ActivationTargets????? // Resolution Targets?
        }

        public void FlipFaceUp() => View.FlipFaceUp();
        public void FlipFaceDown() => View.FlipFaceDown();
        public void AddToChain() => View.AddToChain(ChainIndex.ToString());
        public void RemoveFromChain() => View.RemoveFromChain();
        public void HighlightAsTarget() => View.ValidTarget.Visible = true;
        public void StopHighlightingAsTarget() => View.ValidTarget.Visible = false;
        public void HighlightTargets() { foreach (var target in ValidTargets) { target.HighlightAsTarget(); } }
        public void HighlightAttackTargets() { foreach (var target in ValidAttackTargets) { target.HighlightAsTarget(); } }
        public void StopHighlightingTargets() { foreach (var target in ValidTargets) { target.StopHighlightingAsTarget(); } }
        public void StopHighlightingAttackTargets() { foreach (var target in ValidAttackTargets) { target.StopHighlightingAsTarget(); } }
        public void Select() => View.SelectedTarget.Visible = true;
        public void Deselect() => View.SelectedTarget.Visible = false;

        public void AttackUnit(Card defending)
        {
            View.SelectedTarget.Visible = true;
            View.AttackIcon.Visible = true;
            defending.View.SelectedTarget.Visible = true;
            defending.View.DefenseIcon.Visible = true;
        }

        public void CancelAttack()
        {
            View.SelectedTarget.Visible = false;
            View.AttackIcon.Visible = false;
        }

        public override string ToString() => $"{Id} : {Title}";
    }
}
