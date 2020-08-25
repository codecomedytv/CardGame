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
		public bool CanBeDeployed => State == CardStates.CanBeDeployed && Player is Player player && player.State == States.Idle;
		public bool CanBeSet = false;
		public bool CanBePlayed = false;
		
		// Signals
		[Signal]
		public delegate void MouseOvered();

		[Signal]
		public delegate void MouseOveredExit();

		public void SetCardArt(Texture art)
		{
			GetNode<Sprite3D>("Face").Texture = art;
		}

		public override void _Ready()
		{
			GetNode<Area>("Area").Connect("mouse_entered", this, nameof(OnMouseEntered));
			GetNode<Area>("Area").Connect("mouse_exited", this, nameof(OnMouseExit));
		}

		private void OnMouseEntered()
		{
			EmitSignal(nameof(MouseOvered), this);
		}
		
		private void OnMouseExit()
		{
			EmitSignal(nameof(MouseOveredExit), this);
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
