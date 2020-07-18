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
		private Label Attack;
		private Label Defense;
		private Sprite Art;
		private Sprite Back;
		private Sprite Highlight;
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
			GetNode<AnimationPlayer>("AnimationPlayer").Play("BounceGlow");
			Legal = GetNode<Sprite>("Frame/Highlight");
			Id = GetNode("ID") as Label;
			ChainLink = GetNode("Frame/ChainLink") as AnimatedSprite;
			ChainIndex = GetNode("Frame/ChainIndex") as Label;
			//Legal = GetNode("Legal") as Sprite;
			ValidTarget = GetNode("Frame/ValidTarget") as Sprite;
			SelectedTarget = GetNode("Frame/SelectedTarget") as Sprite;
			AttackIcon = GetNode("Frame/AttackIcon") as Sprite;
			DefenseIcon = GetNode("Frame/DefenseIcon") as Sprite;
			Attack = GetNode("Frame/Attack") as Label;
			Defense = GetNode("Frame/Defense") as Label;
			Art = GetNode("Frame/Art") as Sprite;
			Back = GetNode("Frame/Back") as Sprite;
		}

		public void Display(Card card)
		{
			FlipFaceDown();
			if (card.CardType == CardTypes.Null)
			{
				return;
			}

			FlipFaceUp();
			Id.Text = card.Id.ToString();
			Art.Texture = ResourceLoader.Load($"res://Assets/CardArt/{card.Art}.jpg") as Texture;
			if (card.CardType != CardTypes.Unit)
			{
				return;
			}

			Attack.Visible = true;
			Defense.Visible = true;
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

		public bool IsCurrentlySelected() => SelectedTarget.Visible;
		public void ShowAsLegal() => Legal.Visible = true;
		public void StopShowingAsLegal() => Legal.Visible = false;
		public void HighlightAsTarget() => ValidTarget.Visible = true;
		public void StopHighlightingAsTarget() => ValidTarget.Visible = false;
		public void Select() => SelectedTarget.Visible = true;
		public void Deselect() => SelectedTarget.Visible = false;
		public void StopAttacking() => AttackIcon.Visible = false;
		public void StopDefending() => DefenseIcon.Visible = false;
		public void ShowAsTargeted() => SelectedTarget.Visible = true;
		public void StopShowingAsTargeted() => SelectedTarget.Visible = false;
		
	}
}




