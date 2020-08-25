using System.Collections.Generic;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game.Players
{
	public class Player: Spatial, IPlayer
	{
		private Declaration Declare;
		private Units Units;
		private Support Support;
		private Hand Hand;
		private Spatial Graveyard;
		private Deck Deck;
		private Tween Gfx;
		private AudioStreamPlayer Sfx;
		public States State;
		
		// Begin Business Logic
		public bool IsInActive => State != States.Active && State != States.Idle;

		public void SetState(States state)
		{
			State = state;
			GD.Print("Set State to " + state.ToString());
		}
		
		// End Business Logic
		
		// Begin Animation Logic
		public override void _Ready()
		{
			Units = (Units) GetNode("Units");
			Support = (Support) GetNode("Support");
			Hand = (Hand) GetNode("Hand");
			Graveyard = (Spatial) GetNode("Discard");
			Deck = (Deck) GetNode("Deck");
			Gfx = (Tween) GetNode("GFX");
			Sfx = (AudioStreamPlayer) GetNode("SFX");
		}
		
		public void LoadDeck(IEnumerable<Card> deck)
		{
			foreach (var card in deck)
			{
				card.Player = this;
				AddCardToDeck(card);
			}
		}
		
		public void AddCardToDeck(Card card)
		{
			Deck.Add(card);
		}

		public void Connect(Declaration declaration)
		{
			Declare = declaration;
		}

		public void DisplayName(string name)
		{
			throw new System.NotImplementedException();
		}

		public void DisplayHealth(int health)
		{
			throw new System.NotImplementedException();
		}

		public void Draw(Card card)
		{
			Tween Command()
			{
				card.Visible = false;
				Deck.Add(card);
				var globalPosition = card.Translation;
				Deck.Remove(card);
				Hand.Add(card);
				var globalDestination = card.Translation;
				var rotation = new Vector3(-25, 180, 0);
				
				// Wrap In GFX Class
				Gfx.InterpolateProperty(card, nameof(Visible), false, true, 0.1F);
				Gfx.InterpolateProperty(card, nameof(Translation), globalPosition, globalDestination, 0.1F);
				Gfx.InterpolateProperty(card, nameof(RotationDegrees), card.Rotation, rotation, 0.1F);
				return Gfx;
			}

			Declare(Command);
		}
		
		public void Discard(Card card)
		{
			throw new System.NotImplementedException();
		}

		public void Deploy(Card card)
		{
			Tween Command()
			{
				var origin = card.Translation;
				var destination = Units.NextSlot();

				Hand.Remove(card);
				Units.Add(card);

				Gfx.InterpolateProperty(card, nameof(Translation), origin, destination, 0.3F);
				Gfx.InterpolateProperty(card, nameof(RotationDegrees), new Vector3(-25, 180, 0), new Vector3(0, 180, 0), 0.1F);
				Gfx.InterpolateCallback(Hand, 0.2F, nameof(Hand.Sort));
				return Gfx;
			}

			Declare(Command);
		}

		public void SetFaceDown(Card card)
		{
			Tween Command()
			{
				var origin = card.Translation;
				var destination = Support.NextSlot() + new Vector3(0, 0, 0.1F);

				Hand.Remove(card);
				Units.Add(card);

				Gfx.InterpolateProperty(card, nameof(Translation), origin, destination, 0.3F);
				Gfx.InterpolateProperty(card, nameof(RotationDegrees), new Vector3(-25, 180, 0), new Vector3(0, 0, 0), 0.1F);
				Gfx.InterpolateCallback(Hand, 0.2F, nameof(Hand.Sort));
				return Gfx;
			}
			
			Declare(Command);
		}

		public void Attack(Card attacker, Card defender)
		{
			throw new System.NotImplementedException();
		}

		public void AttackDirectly(Card attacker)
		{
			throw new System.NotImplementedException();
		}
	}
}
