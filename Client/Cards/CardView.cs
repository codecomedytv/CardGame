using System.Collections.Generic;
using CardGame.Client.Room;
using CardGame.Client.Room.View;
using Godot;

namespace CardGame.Client.Cards
{
	public class CardView : Control
	{
		private Label Id;
		private Label Attack;
		private Label Defense;
		private Sprite Art;
		private TextureRect Back;
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
			ChainIndex = GetNode("Frame/ChainLink/Index") as Label;
			Legal = GetNode("Legal") as Sprite;
			ValidTarget = GetNode("ValidTarget") as Sprite;
			SelectedTarget = GetNode("SelectedTarget") as Sprite;
			AttackIcon = GetNode("AttackIcon") as Sprite;
			DefenseIcon = GetNode("DefenseIcon") as Sprite;
			Attack = GetNode("Battle/Attack") as Label;
			Defense = GetNode("Battle/Defense") as Label;
			Art = GetNode("Frame/Illustration") as Sprite;
			Back = GetNode("Back") as TextureRect;
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
		
		public void AddToChain(string chainIndex)
		{
			ChainLink.Frame = 0;
			ChainLink.Visible = true;
			ChainIndex.Text = chainIndex; 
			ChainLink.Play();
		}

		public void RemoveFromChain()
		{
			ChainLink.Visible = false;
			ChainLink.Stop();
		}

		public void ShowAsLegal() => Legal.Visible = false;
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
			if (inputEvent is InputEventMouseButton mouseButton && mouseButton.Doubleclick && GetGlobalRect().HasPoint(mouseButton.Position))
			{
				DoubleClick();
			}
		}

		public void DoubleClick() => EmitSignal(nameof(DoubleClicked));
	}
}




