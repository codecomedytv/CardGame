using Godot;
using System;

namespace CardGame.Client.Cards
{
	public class Card3D : Spatial
	{
		public int Id;
		public bool IsFaceUp = false;
		public Vector3 GlobalPosition => this.GlobalTransform.origin; // Might be translation?
		public Card3D(int id, CardInfo card)
		{
			
		}

		public Card3D()
		{
			
		}
		

		public void Display(Card card)
		{
			/*FlipFaceDown();
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
	
			Power.Visible = true;
			Power.Text = card.Power.ToString();*/
		}

		public void FlipFaceDown()
		{
			//Back.Visible = true;
		}

		public void FlipFaceUp()
		{
		}

		public void AddToChain(int chainIndex)
		{
			// GD.Print($"Adding To Chain with Chain of {chainIndex}");
			// ChainLink.Frame = 0;
			// ChainLink.Visible = true;
			// ChainIndex.Text = chainIndex.ToString();
			// ChainIndex.Visible = true;
			// ChainLink.Play();
		}

		public void RemoveFromChain()
		{
			// ChainIndex.Visible = false;
			// ChainLink.Visible = false;
			// ChainLink.Stop();
		}

		public bool IsCurrentlySelected()
		{
			return false;
		}

		public void ShowAsLegal()
		{
			//Legal.Visible = true;
		}

		public void StopShowingAsLegal()
		{
			//Legal.Visible = false;
		}

		public void HighlightAsTarget()
		{
			//ValidTarget.Visible = true;
		}

		public void StopHighlightingAsTarget()
		{
			//ValidTarget.Visible = false;
		}

		public void Select()
		{
			//Selected.Visible = true;
		}

		public void Deselect()
		{
			//Selected.Visible = false;
		}

		public void StopAttacking()
		{
			//AttackIcon.Visible = false;
		}

		public void StopDefending()
		{
			//DefenseIcon.Visible = false;
		}

		public void ShowAsTargeted()
		{
			//SelectedTarget.Visible = true;
		}

		public void StopShowingAsTargeted()
		{
			//SelectedTarget.Visible = false;
		}
	}
}