using System.Collections.Generic;
using CardGame.Client.Room;
using Godot;

namespace CardGame.Client.Cards
{
	public class CardView : Control
	{
		public Label Id;
		public Label Attack;
		public Label Defense;
		public Sprite Art;
		private TextureRect Back;
		public Sprite Frame;
		public Sprite Legal;
		public Sprite ValidTarget;
		public Sprite SelectedTarget;
		public Sprite AttackIcon;
		public Sprite DefenseIcon;
		public AnimatedSprite ChainLink;
		public Label ChainIndexDisplay;
		public bool IsFaceUp => !Back.Visible;
		
		public override void _Ready()
		{
			Id = GetNode("ID") as Label;
			ChainLink = GetNode("Frame/ChainLink") as AnimatedSprite;
			ChainIndexDisplay = GetNode("Frame/ChainLink/Index") as Label;
			Legal = GetNode("Legal") as Sprite;
			ValidTarget = GetNode("ValidTarget") as Sprite;
			SelectedTarget = GetNode("SelectedTarget") as Sprite;
			AttackIcon = GetNode("AttackIcon") as Sprite;
			DefenseIcon = GetNode("DefenseIcon") as Sprite;
			Attack = GetNode("Battle/Attack") as Label;
			Defense = GetNode("Battle/Defense") as Label;
			Frame = GetNode("Frame") as Sprite;
			Art = GetNode("Frame/Illustration") as Sprite;
			Back = GetNode("Back") as TextureRect;
		}

		public void FlipFaceDown() => Back.Visible = true;
		public void FlipFaceUp() => Back.Visible = false;
		
		[Signal]
		public delegate void MouseEnteredCard();
		[Signal]
		public delegate void MouseExitedCard();
		[Signal]
		public delegate void DoubleClicked();
		public void OnMouseEnter() => EmitSignal(nameof(MouseEnteredCard));
		public void OnMouseExit() => EmitSignal(nameof(MouseExitedCard));
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




