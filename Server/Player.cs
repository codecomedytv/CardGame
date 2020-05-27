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
		public List<Card> Graveyard = new List<Card>();
		public List<Card> Hand = new List<Card>();
		public List<Card> Field = new List<Card>();
		public List<Support> Support = new List<Support>();
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
			GD.PushWarning("Shuffling Not Implemented");
		}

		public void Draw(int drawCount)
		{
			var lost = false;
			var cards = new List<Card>();
			for (var i = 0; i < drawCount; i++)
			{
				var card = Deck[Deck.Count-1];
				Deck.RemoveAt(Deck.Count-1);
				Hand.Add(card);
				card.Zone = Hand;
				cards.Add(card);
				if(Deck.Count == 0)
				{
					lost = true;
					break;
				}
			}
			DeclarePlay(new Draw(cards));
			if (lost)
			{
				Opponent.Win();
			}

		}

		public void Discard(Card discarding)
		{
			move(Hand, discarding, Graveyard);
			DeclarePlay(new Discard(discarding));
		}

		private void move(List<Card> from, Card card, List<Card> to)
		{
			from.Remove(card);
			to.Add(card);
			card.Zone = to;
		}


		public void SetPlayableCards()
		{
			foreach(var card in Hand) { card.SetAsPlayable(); }
		}

		public void Legalize()
		{
			throw new NotImplementedException();
		}

		public void SetTargets(Card selector, List<Card> targets)
		{
			DeclarePlay(new SetTargets(selector, targets));
		}

		public void SetValidAttackTargets()
		{
			foreach (var unit in Field)
			{
				unit.SetValidAttackTargets();
				DeclarePlay(new SetTargets(unit, unit.ValidAttackTargets.ToList()));
			}
		}

		public void ShowAttack(Player player, Unit attacker, Unit defender)
		{
			DeclarePlay(new ShowAttack(attacker, defender));
		}

		public void BeginTurn(int drawCount)
		{
			DeclarePlay(new BeginTurn());
		}
		
		public void DeclareState()
		{
			DeclarePlay(new SetState(this, State));
		}

		public void SetAttackers()
		{
			// TODO: Research IEnumerable & Collections
			foreach(var unit in Field) { unit.SetAsAttacker() as Unit; }
		}

		public void SetActivatables(string gameEvent = "")
		{
			foreach (var support in Support) { support.SetAsActivatable(gameEvent);}
		}

		public bool Active() { return State == States.Idle || State == States.Active; }

		public bool HasTag(Tag tag)
		{
			foreach (var decorator in Tags)
			{
				if (decorator.Tag == tag)
				{
					return true;
				}
			}

			return false;
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

		public void Deploy(Unit card)
		{
			GD.PushWarning("Deploy Complains Due To Type Mismatch");
			move(Hand, card, Field);
			
		}

		public void EndTurn()
		{
			ReadyCards();
			Illegalize();
			DeclarePlay(new EndTurn());
		}
		
		public void Win() { DeclarePlay(new GameOver(this, Opponent)); }

		public void Illegalize()
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
