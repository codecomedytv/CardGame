using Godot;

namespace CardGame.Client.Room.View
{
	public class Player: Spatial
	{
		public Label Damage { get; private set; }
		public Sprite3D Deck { get; private set; }
		public Sprite3D Discard { get; private set; }
		public Spatial Hand { get; private set; }
		public Spatial Units { get; private set; }
		public Spatial Support { get; private set; }
		public Label Health { get; private set; }
		

		public override void _Ready()
		{
			Damage = GetNode<Label>("Damage");
			Deck = GetNode<Sprite3D>("Deck");
			Discard = GetNode<Sprite3D>("Discard");
			Hand = GetNode<Spatial>("Hand");
			Units = GetNode<Spatial>("Units");
			Support = GetNode<Spatial>("Support");
			Health = GetNode<Label>("Health/Label");
		}
	}
}
