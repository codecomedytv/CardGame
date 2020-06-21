using CardGame.Client.Match;
using Godot;
using Godot.Collections;
using Array = Godot.Collections.Array;

namespace CardGame.Client.Library.Card
{
	public class Card : Control
	{
		public enum Zones
		{
			Deck,
			Discard,
			Hand,
			Unit,
			Support
		};

		public int Id = 0;
		public string Title;
		public string Illustration;
		public int Attack = 0;
		public int Defense = 0;
		public string Effect;
		public CardTypes CardType;
		public bool Blank = false;
		
		// Visual Onready Vars
		public AnimatedSprite LegalPlay;
		public Label Identifier;
		public Label TitleLabel;
		public Label AttackLabel;
		public Label DefenseLabel;
		public Sprite Art;
		public TextureRect Back;
		public TextureRect Combat;

		public override void _Ready()
		{
			LegalPlay = GetNode("Frame/LegalPlay") as AnimatedSprite;
			Identifier = GetNode("ID") as Label;
			TitleLabel = GetNode("Title") as Label;
			AttackLabel = GetNode("Battle/Attack") as Label;
			DefenseLabel = GetNode("Battle/Defense") as Label;
			Art = GetNode("Frame/Illustration") as Sprite;
			Back = GetNode("Back") as TextureRect;
			Combat = GetNode("Combat") as TextureRect;
			Visualize();
		}
		
		public override string ToString() => $"{Id} : {Title}";
		
		public void TurnInvisible() => Modulate = new Color(Modulate.r, Modulate.g, Modulate.b, 0);

		public void TurnVisible() => Modulate = new Color(Modulate.r, Modulate.g, Modulate.b, 1);
		
		public void FlipFaceUp() => Back.Visible = false;

		public void FlipFaceDown() => Back.Visible = true;
		

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
			}
		}

		public void Visualize()
		{
			if (Blank)
			{
				// We can handle this by just creating a null object
				FlipFaceDown();
				return;
			}

			Identifier.Text = Id.ToString();
			TitleLabel.Text = Title;
			Art.Texture = ResourceLoader.Load(Illustration) as Texture;
			if (CardType == 0)
			{
				// 0 is Unit?
				AttackLabel.Text = Attack.ToString();
				DefenseLabel.Text = Defense.ToString();
			}
			else
			{
				Combat.Hide();
			}

		}
		
	}
}




