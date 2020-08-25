using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game.Players
{
	public class Opponent: Spatial, IPlayer
	{
		
		private Declaration Declare;
		private Units Units;
		private Support Support;
		private Hand Hand;
		private Spatial Graveyard;
		private Deck Deck;
		private Tween Gfx;
		private AudioStreamPlayer Sfx;

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
		
		public void Connect(Declaration declaration)
		{
			Declare = declaration;
		}

		public void AddCardToDeck(Card card)
		{
			Deck.Add(card);
		}
		
		public void DisplayName(string name)
		{
			throw new System.NotImplementedException();
		}

		public void DisplayHealth(int health)
		{
			throw new System.NotImplementedException();
		}
		
		public void LoadDeck(IEnumerable<Card> deck)
		{
			foreach (var card in deck)
			{
				AddCardToDeck(card);
			}
		}
		public void Draw(Card ignoredCard)
		{
			Tween Command()
			{
				var card = Deck.ElementAt(Deck.Count - 1);
				Deck.Add(card);
				var globalPosition = card.Translation;
				Deck.Remove(card);
				Hand.Add(card);
				var globalDestination = card.Translation;
				var rotation = new Vector3(60, 0, 0);
				
				// Wrap In GFX Class
				Gfx.InterpolateProperty(card, nameof(Visible), false, true, 0.1F);
				Gfx.InterpolateProperty(card, nameof(Translation), globalPosition, globalDestination, 0.1F);
				Gfx.InterpolateProperty(card, nameof(RotationDegrees), card.Rotation, rotation, 0.1F);
				return Gfx;
			};

		   Declare(Command);
		}
		
		public void Discard(Card card)
		{
			throw new System.NotImplementedException();
		}

		public void Deploy(Card card)
		{
			GD.Print("Opponent Deploying Card");
			Tween Command()
			{
				//card.GetNode<Sprite3D>("Face").FlipV = !card.GetNode<Sprite3D>("Face").FlipV;
				var fakeCard = Hand[0];
				Hand.Remove(fakeCard);
				Hand.Add(card);
				fakeCard.Free();
				
				var origin = card.Translation;
				var destination = Units.NextSlot()  + new Vector3(0, 0, 0.05F);

				Hand.Remove(card);
				Units.Add(card);
				
				GD.Print(origin);
				GD.Print(destination);
				Gfx.InterpolateProperty(card, nameof(Translation), origin, destination, 0.3F);
				Gfx.InterpolateProperty(card, nameof(RotationDegrees), new Vector3(-25, 0, 0), new Vector3(0, 180, 0), 0.1F);
				Gfx.InterpolateCallback(Hand, 0.2F, nameof(Hand.Sort));
				return Gfx;
			}
			
			Declare(Command);
		}

		public void SetFaceDown(Card ignoredCard)
		{
			Tween Command()
			{
				var card = Hand[0];
				var origin = card.Translation;
				var destination = Support.NextSlot() + new Vector3(0, 0, 0.05F);

				Hand.Remove(card);
				Units.Add(card);

				Gfx.InterpolateProperty(card, nameof(Translation), origin, destination, 0.3F);
				Gfx.InterpolateProperty(card, nameof(RotationDegrees), new Vector3(-25, 0, 0), new Vector3(0, 0, 0), 0.1F);
				Gfx.InterpolateCallback(Hand, 0.2F, nameof(Hand.Sort));
				return Gfx;
			}
			
			Declare(Command);
		}

		public void Activate(Card card)
		{
			GD.Print("Opponent Activate");
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
