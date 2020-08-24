using Godot;

namespace CardGame.Client.Game.Cards
{
	public class Card: Spatial
	{
		public int Id;
		public string Title;
		public int Power;
		public CardType CardType;
		public CardFace Face = CardFace.FaceDown;

		public override void _Ready()
		{
			GetNode<Area>("Area").Connect("mouse_entered", this, nameof(OnMouseEntered));
			GetNode<Area>("Area").Connect("mouse_exited", this, nameof(OnMouseExited));
		}

		private void OnMouseEntered()
		{
			GD.Print("Entered");
		}

		private void OnMouseExited()
		{
			GD.Print("Exit!");
		}


		public void DisplayPower(int power)
		{
			throw new System.NotImplementedException();
		}

		public void FlipFaceUp()
		{
			throw new System.NotImplementedException();
		}

		public void FlipFaceDown()
		{
			throw new System.NotImplementedException();
		}
	}
}
