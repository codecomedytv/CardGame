using System.Collections.Generic;
using CardGame.Client.Library.Alpha;
using CardGame.Client.Room;
using Godot;

namespace CardGame.Client.Library.Cards
{
	public class Card : Control
	{
		public int Id = 0;
		public string Title;
		public string Illustration = "res://Assets/UI/CardBack_Wooden.png";
		public int Attack = 0;
		public int Defense = 0;
		public string Effect;
		public CardTypes CardType;
		public CardStates State = CardStates.CanBeDeployed;
		public Player Player;
		private Label Identifier;
		private Label AttackLabel;
		private Label DefenseLabel;
		private Sprite Art;
		private TextureRect Back;
		private Sprite Frame;
		public Sprite Legal;
		public Sprite ValidTarget;
		public Sprite SelectedTarget;
		public Sprite AttackIcon;
		public Sprite DefenseIcon;
		private AnimatedSprite ChainLink;
		public Label ChainIndexDisplay;
		public int ChainIndex;
		public readonly List<int> ValidTargets = new List<int>();
		public readonly List<int> ValidAttackTargets = new List<int>();
		public bool IsTargeting = false;

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
			ChainLink = GetNode("Frame/ChainLink") as AnimatedSprite;
			ChainIndexDisplay = GetNode("Frame/ChainLink/Index") as Label;
			Legal = GetNode("Legal") as Sprite;
			ValidTarget = GetNode("ValidTarget") as Sprite;
			SelectedTarget = GetNode("SelectedTarget") as Sprite;
			AttackIcon = GetNode("AttackIcon") as Sprite;
			DefenseIcon = GetNode("DefenseIcon") as Sprite;
			Identifier = GetNode("ID") as Label;
			AttackLabel = GetNode("Battle/Attack") as Label;
			DefenseLabel = GetNode("Battle/Defense") as Label;
			Frame = GetNode("Frame") as Sprite;
			Art = GetNode("Frame/Illustration") as Sprite;
			Back = GetNode("Back") as TextureRect;
			Visualize();
		}
		
		public override string ToString() => $"{Id} : {Title}";

		public void FlipFaceDown() => Back.Visible = true;

		public void FlipFaceUp() => Back.Visible = false;

		public void ShowAboveParent() => ShowBehindParent = false;
		public void ShowBelowParent() => ShowBehindParent = true;

		public void AddToChain()
		{
			ChainLink.Frame = 0;
			ChainLink.Visible = true;
			ChainIndexDisplay.Text = ChainIndex.ToString();
			ChainLink.Play();
		}
		
		public void RemoveFromChain()
		{
			ChainLink.Stop();
			ChainLink.Visible = false;
		}
		
		public void SetData(BaseCard card)
		{
			Title = card.Title;
			Effect = card.Text;
			Illustration = card.Illustration;
			switch (card)
			{
				case Unit unit:
					CardType = unit.CardType;
					Attack = unit.Attack;
					Defense = unit.Defense;
					break;
				case Support support:
					CardType = support.CardType;
					break;
				case NullCard nullCard:
					CardType = CardTypes.Null;
					break;
			}
		}

		private void Visualize()
		{
			if (CardType == CardTypes.Null)
			{
				Back.Visible = true;
				return;
			}
			Identifier.Text = Id.ToString();
			Art.Texture = ResourceLoader.Load(Illustration) as Texture;
			if (CardType != 0) return;
			AttackLabel.Text = Attack.ToString();
			DefenseLabel.Text = Defense.ToString();
		}


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




