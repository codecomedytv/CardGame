using System;
using System.Collections.Generic;
using CardGame.Client.Game.Players;
using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game.Cards
{
	public class Card: Spatial
	{
		public int Id;
		public string Title;
		public int Power;
		public CardType CardType;
		public CardStates State;
		private CardFace _face = CardFace.FaceDown;

		public IPlayer Player;
		public IZone Zone;
		public readonly IList<Card> ValidTargets = new List<Card>();
		public readonly IList<Card> ValidAttackTargets = new List<Card>();

		// State Checks
		public bool IsInActive = false;
		public bool CanBeDeployed = false;
		public bool CanBeSet = false;
		public bool CanBePlayed = false;
		
		// Signals
		[Signal]
		public delegate void MouseOvered();


		public override void _Ready()
		{
			GetNode<Area>("Area").Connect("mouse_entered", this, nameof(OnMouseEntered));
		}

		private void OnMouseEntered()
		{
			EmitSignal(nameof(MouseOvered), this);
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
