using System.Collections.Generic;
using CardGame.Client.Room;
using CardGame.Client.Room.View;
using Godot;
using Player = CardGame.Client.Room.Player;

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
        public int Power = 0;
        public int ChainIndex;
        public readonly List<Card> ValidTargets = new List<Card>();
        public readonly List<Card> ValidAttackTargets = new List<Card>();
        public Vector2 Position { get => RectGlobalPosition; set => RectGlobalPosition = value; }
        public Zone Zone;

        public bool IsFaceUp => View.IsFaceUp;

        public bool IsInActive =>
            State == CardStates.Passive || State == CardStates.Activated && !Player.ActionInProgress;
        public bool CanBeDeployed => State == CardStates.CanBeDeployed && Player.State == States.Idle && !Player.ActionInProgress;
        public bool CanBeSet => State == CardStates.CanBeSet && Player.State == States.Idle && !Player.ActionInProgress;
        public bool CanBeActivated => State == CardStates.CanBeActivated && !Player.IsInActive && !Player.ActionInProgress;
        public bool CanAttack => State == CardStates.CanAttack && ValidAttackTargets.Count > 0 && Player.State == States.Idle && !Player.ActionInProgress;
        public bool CanAttackDirectly => State == CardStates.CanAttackDirectly && Player.State == States.Idle && !Player.ActionInProgress;
        
        public bool CanTarget => State == CardStates.CanBeActivated && ValidTargets.Count > 0 && !Player.IsInActive; // && !Player.ActionInProgress;
        public bool CanBePlayed => CanBeDeployed || CanBeSet || CanBeActivated || CanAttack; // can attack?
        public bool HasTarget(Card target) => ValidTargets.Contains(target);
        public bool HasAttackTarget(Card defender) => ValidAttackTargets.Contains(defender);
        
        public Card(int id, CardInfo c)
        {
            Id = id;
            View = (CardView) Scene.Instance();
            (Title, Effect, Art, CardType, Power) = (c.Title, c.Text, c.Art, c.Type, c.Power);
        }
        
        public override void _Ready()
        {
            if (!GetTree().GetNodesInGroup("cards").Contains(this))
            {
                AddToGroup("cards");
            }
            AddChild(View);
            RectSize = View.RectSize;
            RectMinSize = View.RectMinSize;
            View.Display(this);
        }

        public override void _Process(float delta)
        {
            if (Player.CanPlay(this)) { ShowAsLegal(); } else { StopShowingAsLegal(); }
        }

        public bool IsCurrentlySelected() => View.IsCurrentlySelected();
        public void FlipFaceUp() => View.FlipFaceUp();
        public void FlipFaceDown() => View.FlipFaceDown();
        public void ShowAsLegal() => View.ShowAsLegal();
        public void StopShowingAsLegal() => View.StopShowingAsLegal();
        public void AddToChain() => View.AddToChain(ChainIndex);
        public void RemoveFromChain() => View.RemoveFromChain();
        public void HighlightAsTarget() => View.HighlightAsTarget();
        public void StopHighlightingAsTarget() => View.StopHighlightingAsTarget();
        public void HighlightTargets() => ValidTargets.ForEach(t => t.HighlightAsTarget());
        public void HighlightAttackTargets()
        {
            ValidAttackTargets.ForEach(t => t.HighlightAsTarget());
            if (CanAttackDirectly)
            {
                Player.Opponent.HighlightAsTarget();
            }
        }

        public void StopHighlightingTargets() { ValidTargets.ForEach(t => t.StopHighlightingAsTarget()); }
        public void StopHighlightingAttackTargets()
        {
            ValidAttackTargets.ForEach(t => t.StopHighlightingAsTarget());
            Player.Opponent.StopHighlightingAsTarget();
        }

        public void Select() => View.Select();
        public void Deselect() => View.Deselect();
        public void StopAttacking() => View.StopAttacking();
        public void StopDefending() => View.StopDefending();
        public void ShowAsTargeted() => View.ShowAsTargeted();
        public void StopShowingAsTargeted() => View.StopShowingAsTargeted();

        public void AttackUnit(Card defending)
        {
            Select();
            View.AttackIcon.Visible = true;
            defending.ShowAsTargeted();
            defending.View.DefenseIcon.Visible = true;
        }

        public void CancelAttack()
        {
            Deselect();
            View.AttackIcon.Visible = false;
        }

        public override string ToString() => $"{Id} : {Title}";

        public bool IsFocused(Vector2 mousePosition) => GetGlobalRect().HasPoint(mousePosition);

    }
}
