using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardGame.Server.States;

namespace CardGame.Server {

	public class Player : Node
	{
		//public enum States { Idle, Active, Passive, Acting, Passing }

		public State State;
		public List<SetCodes> DeckList;
		public readonly int Id;
		public Player Opponent;
		public int Health = 8000;
		public bool Ready = false;
		public bool Passing = false;
		public List<Card> Deck = new List<Card>();
		public List<Card> Graveyard = new List<Card>();
		public List<Card> Hand = new List<Card>();
		public List<Card> Field = new List<Card>();
		public List<Card> Support = new List<Card>();
		public List<Decorator> Tags = new List<Decorator>();
		public bool Disqualified;
		public bool IsTurnPlayer = false;

		[Signal]
		public delegate void Pause();

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
		
		public Player(int id, List<SetCodes> deckList)
		{
			DeckList = deckList;
			Id = id;
			// Shuffle = shuffle;
		}

		public void LoadDeck(Gamestate game)
		{
			foreach (SetCodes setCode in DeckList)
			{
				var card = Library.Create(setCode);
				foreach (var skill in (IEnumerable<Skill>)card.Skills)
				{
					skill.GameState = game;
				}

				card.SetOwner(this);
				card.SetControllerAndOpponent(this);
				card.Zone = Deck;
				game.RegisterCard(card);
				Deck.Add(card);
			}

			DeclarePlay(new LoadDeck(Deck.ToList()));
		}

		public void DeclarePlay(GameEvent gameEvent)
		{
			EmitSignal(nameof(PlayExecuted), this, gameEvent);
		}

		public void Shuffle()
		{
			// TODO: Implement Shuffle
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
				GD.Print("Player Lost?");
				Opponent.Win();
			}

		}

		public void Discard(Card card)
		{
			Hand.Remove(card);
			Graveyard.Add(card);
			card.Zone = Graveyard;
			card.EmitSignal(nameof(Card.Exit));
			DeclarePlay(new Discard(card));
		}
		
		public void SetPlayableCards()
		{
			foreach(var card in Hand) { card.SetAsPlayable(); }
		}

		public void Legalize()
		{
			foreach (var card in Hand)
			{
				card.Legal = true;
				DeclarePlay(new Legalize(card));
			}
		}

		public void SetTargets(Card selector, List<Card> targets)
		{
			DeclarePlay(new SetTargets(selector, targets));
		}

		public void SetValidAttackTargets()
		{
			foreach (var unit in Field.Select(u =>(Unit)u))
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
			foreach(var unit in Field.Select(u => (Unit)u)) { unit.SetAsAttacker(); }
		}

		public void SetActivatables(string gameEvent = "")
		{
			foreach (var support in Support.Select(s => (Support)s )) { support.SetAsActivatable(gameEvent);}
		}

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
			foreach (var card in Field)
			{
				card.Ready = true;
				DeclarePlay(new ReadyCard(card));
			}

			foreach (var card in Support)
			{
				card.Ready = true;
				DeclarePlay(new ReadyCard(card));
			}
		}

		public void UnreadyCard(Card card)
		{
			card.Ready = false;
			DeclarePlay(new UnreadyCard(card));
		}

		public void SetFaceDown(Card card)
		{
			Hand.Remove(card);
			Support.Add(card);
			card.Zone = Support;
			card.EmitSignal(nameof(Card.Exit));
			DeclarePlay(new Discard(card));
			card.Legal = false;
			DeclarePlay(new SetSupport(card));
		}

		public void AttackUnit(Unit attacker, Unit defender)
		{
			DeclarePlay(new AttackUnit(attacker, defender));
		}

		public void AttackDirectly(Unit attacker)
		{
			DeclarePlay(new AttackDirectly(attacker));
		}

		public void Deploy(Unit card)
		{
			// GD.PushWarning("Deploy Complains Due To Type Mismatch");
			// Move(Hand, card, Field);
			Hand.Remove(card);
			Field.Add(card);
			card.Zone = Field;
			card.EmitSignal(nameof(Card.Exit));
			DeclarePlay(new Deploy(card));
			
		}

		public void Activate(Card card, int skillIndex, List<Card> targets)
		{
			DeclarePlay(new Activate(card, targets));
		}

		public void DestroyUnit(Unit card)
		{
			if (card.HasTag(Tag.CannotTakeDamage))
			{
				// Should Be In Battle.
				return;
			}
			if (card.HasTag(Tag.CannotBeDestroyedByEffect))
			{
				return;
			}
			if (!card.Controller.Field.Contains(card))
			{
				return;
			}
			Field.Remove(card);
			card.Owner.Graveyard.Add(card);
			card.Zone = card.Owner.Graveyard;
			card.EmitSignal(nameof(Card.Exit));
			DeclarePlay(new DestroyUnits(new List<Unit> { card }));
		}

		public void ReturnToDeck(Card card)
		{
			Hand.Remove(card);
			card.Owner.Deck.Add(card);
			card.Zone = card.Owner.Deck;
			card.EmitSignal(nameof(Card.Exit));
			DeclarePlay(new Discard(card));
			Shuffle();
			DeclarePlay(new ReturnedToDeck(card));
		}

		public void LoseLife(int lifeLost)
		{
			if(HasTag(Tag.CannotTakeDamage))
			{
				return;
			}

			Health -= lifeLost;
			
			DeclarePlay(new LoseLife(lifeLost));
			if (Health <= 0)
			{
				Opponent.Win();
			}
		}

		public void Bounce(Card card)
		{
			if(!card.Controller.Field.Contains(card))
			{
				return;
			}
			
			card.Zone.Remove(card);
			card.Owner.Hand.Add(card);
			card.Zone = card.Owner.Hand;
			card.EmitSignal(nameof(Card.Exit));
			DeclarePlay(new Bounce(card));
		}

		public void MillFromDeck()
		{
			if (Deck.Count == 0)
			{
				return;
			}

			var card = Deck[Deck.Count - 1];
			Deck.RemoveAt(Deck.Count - 1);
			Graveyard.Add(card);
			card.Zone = Graveyard;
			DeclarePlay(new Mill(card));
		}

		public void EndTurn()
		{
			//EmitSignal(nameof(TurnEnded), Opponent);
			ReadyCards();
			Illegalize();
			DeclarePlay(new EndTurn());
		}
		
		public void Win() { DeclarePlay(new GameOver(this, Opponent)); }
		
		public void Illegalize()
		{
			foreach (var card in Hand)
			{
				card.Legal = false;
				Forbid(card);
			}
		}

		public void SetLegal(Card card)
		{
			DeclarePlay(new Legalize(card));
		}

		public void Forbid(Card card)
		{
			DeclarePlay(new Forbid(card));
		}

		public void Disqualify()
		{
			throw new NotImplementedException();
		}
	}
	
}
