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
        public CardStates State = CardStates.CanBeDeployed;
        public Player Player;
        public string Title;
        public string Art;
        public string Effect;
        public int Attack = 0;
        public int Defense = 0;

        public bool IsFaceUp => View.IsFaceUp;
        public int ChainIndex;
        public readonly List<int> ValidTargets = new List<int>();
        public readonly List<int> ValidAttackTargets = new List<int>();
        public bool IsTargeting = false;
        public Zone Zone;

        public Card(int id, CardInfo c)
        {
            Id = id;
            View = (CardView) Scene.Instance();
            (Title, Effect, Art, CardType, Attack, Defense) = (c.Title, c.Text, c.Art, c.CardType, c.Attack, c.Defense);
        }
        
        public void FlipFaceUp() => View.FlipFaceUp();
        public void FlipFaceDown() => View.FlipFaceDown();

        public void MoveZone(Zone oldZone, Zone newZone)
        {
            View.SelectedTarget.Visible = false;
            View.DefenseIcon.Visible = false;
            View.AttackIcon.Visible = false;
            View.ChainLink.Stop();
            View.ChainLink.Visible = false;
            oldZone.Remove(this);
            ShowBehindParent = true;
            oldZone.Sort();
            newZone.Add(this);
        }
        
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
            RectSize = View.RectSize;
            RectMinSize = View.RectMinSize;
            Visualize();
        }
        
        public void AddToChain()
        {
            View.ChainLink.Frame = 0;
            View.ChainLink.Visible = true;
            View.ChainIndexDisplay.Text = ChainIndex.ToString();
            View.ChainLink.Play();
        }

        private void Visualize()
        {
            View.FlipFaceDown();
            if (CardType == CardTypes.Null) { return; }
            View.FlipFaceUp();
            View.Id.Text = Id.ToString();
            View.Art.Texture = ResourceLoader.Load(Art) as Texture;
            if (CardType != CardTypes.Unit) return;
            View.Attack.Text = Attack.ToString();
            View.Defense.Text = Defense.ToString();
        }
    }
    
    
}
