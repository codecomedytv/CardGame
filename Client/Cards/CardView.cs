using System.Collections.Generic;
using CardGame.Client.Room;
using CardGame.Client.Room.View;
using Godot;

namespace CardGame.Client.Cards
{
	// ReSharper disable once ClassNeverInstantiated.Global
	public class CardView : Control
	{
		private Label Id;
		private Label Attack;
		private Label Defense;
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
			Attack = GetNode("Frame/Battle/Attack") as Label;
			Defense = GetNode("Frame/Battle/Defense") as Label;
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
			Attack.Text = card.Attack.ToString();
			Defense.Text = card.Defense.ToString();
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

		public void ShowAsLegal() => Legal.Visible = true;
		public void StopShowingAsLegal() => Legal.Visible = false;
		public void HighlightAsTarget() => ValidTarget.Visible = true;
		public void StopHighlightingAsTarget() => ValidTarget.Visible = false;
		public void Select() => SelectedTarget.Visible = true;
		public void Deselect() => SelectedTarget.Visible = false;

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




