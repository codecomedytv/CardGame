using System.Collections.Generic;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game.Players
{
	public class Player: Spatial, IPlayer
	{
		private Declaration Declare;
		private Spatial Units;
		private Spatial Support;
		private Hand Hand;
		private Spatial Graveyard;
		private Deck Deck;
		private Tween Gfx;
		private AudioStreamPlayer Sfx;

		public override void _Ready()
		{
			Units = (Spatial) GetNode("Units");
			Support = (Spatial) GetNode("Support");
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
			throw new System.NotImplementedException();
		}

		public void Set(Card card)
		{
			throw new System.NotImplementedException();
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
