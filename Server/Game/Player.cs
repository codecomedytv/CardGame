using System;
using Godot;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CardGame.Server.Game;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Events;
using CardGame.Server.Game.Skills;
using CardGame.Server.Game.Tags;
using CardGame.Server.Game.Zones;

namespace CardGame.Server {

	public class Player : Node, ISource
	{

		public readonly List<SetCodes> DeckList;
		public readonly int Id;

		public States State;
		public Player Opponent;
		public int Health = 8000;
		public bool Ready = false;
		public readonly List<Tag> Tags = new List<Tag>();
		public readonly Zone Deck;
		public readonly Zone Graveyard;
		public readonly Zone Hand;
		public readonly Zone Support;
		public readonly Zone Field;
		public History History;
		public bool IsDisqualified;
		public Func<bool> IsTurnPlayer;
		public Unit AttackingWith;
		public int Seat;
		public Skill TargetingSkill;

		[Signal]
		public delegate void TargetSelected();

		public Player()
		{
			
		}
		
		public Player(int id, List<SetCodes> deckList)
		{
			DeckList = deckList;
			Id = id;
			// Shuffle = shuffle;
			Deck = new Zone(this, ZoneIds.Deck);
			Graveyard = new Zone(this, ZoneIds.Graveyard);
			Hand = new Zone(this, ZoneIds.Hand);
			Support = new Zone(this, ZoneIds.Support);
			Field = new Zone(this, ZoneIds.Field);
		}

		public void OnTargetSelected(Card card) => EmitSignal(nameof(TargetSelected), card);
		
		public void LoadDeck(CardCatalog cards)
		{
			foreach (var card in DeckList.Select(setCode => Library.Create(setCode, this)))
			{
				card.History = History;
				card.Zone = Deck;
				cards.RegisterCard(card);
				Deck.Add(card);
			}
			
			History.Add(new LoadDeck(this, new ReadOnlyCollection<Card>(Deck.ToList())));
		}

		public void Move(Zone origin, Card card, Zone destination)
		{
			origin.Remove(card);
			destination.Add(card);
			card.Zone = destination;
		}

		public void Shuffle() { /* TODO: Implement Shuffle (Probably via an Action) */ }

		public void Draw()
		{
			var card = Deck.Top;
			Move(Deck, card, Hand);
			History.Add(new Draw(this, this, card));
		}

		public void Deploy(Unit unit)
		{
			Move(Hand, unit, Field);
			History.Add(new Deploy(this, this, unit));
		}

		public void SetFaceDown(Support support)
		{
			Move(Hand, support, Support);
			History.Add(new SetFaceDown(this, this, support));
		}

		public void Win() => History.Add(new GameOver(this, Opponent));

		public bool HasTag(TagIds tagId) => Tags.Any(tag => tag.TagId == tagId);
	}
	
}
