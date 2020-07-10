using System.Collections.Generic;
using System.Linq;
using CardGame.Client.Room;
using CardGame.Client.Room.View;
using Godot;

namespace CardGame.Client.Cards
{
	// ReSharper disable once ClassNeverInstantiated.Global
	public class CardView : Control
	{
		private Label Id;
		private Node2D Attack;
		private Node2D Defense;
		private Sprite Art;
		private Sprite Back;
		private Sprite Legal;
		private Sprite ValidTarget;
		private Sprite SelectedTarget;
		public Sprite AttackIcon;
		public Sprite DefenseIcon;
		private AnimatedSprite ChainLink;
		private Label ChainIndex;
		public bool IsFaceUp => !Back.Visible;
		
		public override void _Ready()
		{
			Id = GetNode("ID") as Label;
			ChainLink = GetNode("Frame/ChainLink") as AnimatedSprite;
			ChainIndex = GetNode("Frame/ChainIndex") as Label;
			Legal = GetNode("Frame/Legal") as Sprite;
			ValidTarget = GetNode("Frame/ValidTarget") as Sprite;
			SelectedTarget = GetNode("Frame/SelectedTarget") as Sprite;
			AttackIcon = GetNode("Frame/AttackIcon") as Sprite;
			DefenseIcon = GetNode("Frame/DefenseIcon") as Sprite;
			Attack = GetNode("Frame/Attack") as Node2D;
			Defense = GetNode("Frame/Defense") as Node2D;
			Art = GetNode("Frame/Illustration") as Sprite;
			Back = GetNode("Frame/Back") as Sprite;
		}
		
		public void Display(Card card)
		{
			FlipFaceDown();
			if(card.CardType == CardTypes.Null) {return;}
			FlipFaceUp();
			Id.Text = card.Id.ToString();
			Art.Texture = ResourceLoader.Load($"res://Assets/CardArt/{card.Art}.png") as Texture;
			if(card.CardType != CardTypes.Unit) {return;}
			SetAttack(card.Attack);
			SetDefense(card.Defense);
		}

		private void SetAttack(int value)
		{

			ClearBattle(Attack);
			Attack.Visible = true;
			var values = value.ToString().Reverse().ToList();
			for (var i = 0; i < values.Count(); i++)
			{
				Attack.GetNode<Sprite>(i.ToString()).Visible = true;
				Attack.GetNode<Sprite>(i.ToString()).Texture = Assets.Icons.Numbers.IconList[values[i].ToString()];
			}
		}

		private void SetDefense(int value)
		{
			ClearBattle(Defense);
			Defense.Visible = true;
			var values = value.ToString().Reverse().ToList();
			for (var i = 0; i < values.Count(); i++)
			{
				Defense.GetNode<Sprite>(i.ToString()).Visible = true;
				Defense.GetNode<Sprite>(i.ToString()).Texture = Assets.Icons.Numbers.IconList[values[i].ToString()];
			}
		}
		
		private void ClearBattle(Node2D container)
		{
			foreach (Node2D child in container.GetChildren()) { child.Visible = false; }
		}

		public void FlipFaceDown() => Back.Visible = true;
		public void FlipFaceUp() => Back.Visible = false;
		
		public void AddToChain(int chainIndex)
		{
			GD.Print($"Adding To Chain with Chain of {chainIndex}");
			ChainLink.Frame = 0;
			ChainLink.Visible = true;
			ChainIndex.Text = chainIndex.ToString();
			ChainIndex.Visible = true;
			ChainLink.Play();
		}

		public void RemoveFromChain()
		{
			ChainIndex.Visible = false;
			ChainLink.Visible = false;
			ChainLink.Stop();
		}

		public bool IsCurrentlySelected() => SelectedTarget.Visible;
		public void ShowAsLegal() => Legal.Visible = true;
		public void StopShowingAsLegal() => Legal.Visible = false;
		public void HighlightAsTarget() => ValidTarget.Visible = true;
		public void StopHighlightingAsTarget() => ValidTarget.Visible = false;
		public void Select() => SelectedTarget.Visible = true;
		public void Deselect() => SelectedTarget.Visible = false;

		public void StopAttacking() => AttackIcon.Visible = false;
		public void StopDefending() => DefenseIcon.Visible = false;

		[Signal]
		public delegate void DoubleClicked();
		public void OnMouseEnter()
		{
			CardViewer.View(GetParent() as Card);
		}
		
		public override void _Input(InputEvent inputEvent)
		{
			if (inputEvent is InputEventMouseButton mouseButton && mouseButton.Doubleclick)
			{
				// We need to check against player attacking/targeting sub-client states
				// Deselect(); 
				if (GetGlobalRect().HasPoint(mouseButton.Position))
				{
					DoubleClick();
				}
			}
		}
		
		public void DoubleClick() => EmitSignal(nameof(DoubleClicked));
	}
}




