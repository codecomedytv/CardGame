using System;
using Godot;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CardGame.Server.Game;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Events;
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
		public bool IsDisqualified;
		public Func<bool> IsTurnPlayer;
		public Match Match;
		public int Seat;

		[Signal]
		public delegate void TargetSelected();

		public Player()
		{
			
		}

		public void OnTargetSelected(Card card) => EmitSignal(nameof(TargetSelected), card);
		
		public void SetState(States newState)
		{
			State = newState;
			switch (State)
			{
				case States.Idle:
				{
					foreach(var card in Hand) {card.SetCanBeDeployed();}
					foreach(var card in Hand) {card.SetCanBeSet();}
					foreach(var card in Field) {card.SetCanAttack();}
					foreach(var card in Support) {card.SetCanBeActivated();}
					break;
				}
				case States.Active:
				{
					foreach(var card in Support) {card.SetCanBeActivated();}
					break;
				}
			}
		}
		
		public Player(int id, List<SetCodes> deckList)
		{
			DeckList = deckList;
			Id = id;
			// Shuffle = shuffle;
			Deck = new Zone(this);
			Graveyard = new Zone(this);
			Hand = new Zone(this);
			Support = new Zone(this);
			Field = new Zone(this);
		}

		public void LoadDeck(Match match)
		{
			foreach (var card in DeckList.Select(setCode => Library.Create(setCode, this)))
			{
				card.Skill.Match = match;
				card.Zone = Deck;
				Match.RegisterCard(card);
				Deck.Add(card);
			}
			
			Match.History.Add(new LoadDeck(this, new ReadOnlyCollection<Card>(Deck.ToList())));
		}

		public void Move(Zone origin, Card card, Zone destination)
		{
			origin.Remove(card);
			destination.Add(card);
			card.Zone = destination;
		}

		public void Shuffle() { /* TODO: Implement Shuffle */ }

		public void Draw()
		{
			var card = Deck.Top;
			Move(Deck, card, Hand);
			Match.History.Add(new Move(GameEvents.Draw, this, Hand, card, Hand));
		}

		public void Win() => Match.History.Add(new GameOver(this, Opponent));

		public bool HasTag(TagIds tagId) => Tags.Any(tag => tag.TagId == tagId);
	}
	
}
