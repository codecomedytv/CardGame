using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame.Server {

	public class Player : Node
	{
		public enum States { Idle, Active, Passive, Acting, Passing }

		public States State = States.Passive;
		private List<int> DeckList;
		public readonly int Id;
		public Player Opponent;
		public int Health = 8000;
		public bool Ready = false;
		public bool Passing = false;
		public List<Card> Deck = new List<Card>();
		public List<Card> Discard = new List<Card>();
		public List<Card> Hand = new List<Card>();
		public List<Card> Field = new List<Card>();
		public List<Card> Support = new List<Card>();
		public List<Decorator> Tags = new List<Decorator>();
		public bool Disqualified;

		[Signal]
		public delegate void PlayExecuted();
		
		[Signal]
		public delegate void PriorityPassed();

		[Signal]
		public delegate void TurnEnded();

		[Signal]
		public delegate void Register();

		[Signal]
		public delegate void Deployed();

		[Signal]
		public delegate void Paused();
		
		public Player() {}
		
		public Player(int id, List<int> deckList)
		{
			DeckList = deckList;
			Id = id;
			// Shuffle == shuffle;
		}

		public void LoadDeck(Gamestate game)
		{
			foreach (int setCode in DeckList)
			{
				var card = Library.Create(setCode);
				foreach (var skill in card.Skills)
				{
					skill.GameState = game;
				}

				card.Owner = this;
				card.Controller = this;
				card.Zone = Deck;
				game.RegisterCard(card);
				Deck.Add(card);
			}

			DeclarePlay(new LoadDeck(Deck.ToList()));
		}

		public void DeclarePlay(GameEvent gameEvent)
		{
			throw new NotImplementedException();
		}

		public void Shuffle()
		{
			throw new NotImplementedException();
		}

		public void Draw(int drawCount)
		{
			throw new NotImplementedException();
		}

		public void SetPlayableCards()
		{
			throw new NotImplementedException();
		}

		public void Legalize()
		{
			throw new NotImplementedException();
		}

		public void DeclareState()
		{
			throw new NotImplementedException();
		}

		public void SetAttackers()
		{
			throw new NotImplementedException();
		}

		public void SetActivatables(string gameEvent = "")
		{
			throw new NotImplementedException();
		}

		public void ReadyCards()
		{
			throw new NotImplementedException();
		}

		public void SetFaceDown(Card card)
		{
			throw new NotImplementedException();
		}

		public void ShowAttack(int playerId, int attackerId, int defenderId)
		{
			throw new NotImplementedException();
		}

		public void Deploy(Card card)
		{
			throw new NotImplementedException();
		}

		public void EndTurn()
		{
			throw new NotImplementedException();
		}

		public void SetLegal(Card card)
		{
			throw new NotImplementedException();
		}

		public void Forbid(Card card)
		{
			throw new NotImplementedException();
		}
	}
	
}
