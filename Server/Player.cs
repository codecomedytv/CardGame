using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardGame.Server.States;
using Godot.Collections;

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
		public List<Card> Deck = new List<Card>();
		public List<Card> Graveyard = new List<Card>();
		public List<Card> Hand = new List<Card>();
		public List<Card> Field = new List<Card>();
		public List<Card> Support = new List<Card>();
		public List<Decorator> Tags = new List<Decorator>();
		public bool IsDisqualified;
		public bool IsTurnPlayer = false;
		public Link Link;
		public Battle Battle;

		[Signal]
		public delegate void PlayExecuted();
		
		[Signal]
		public delegate void Register();
		public Player() {}
		
		public bool OnDeploy(Unit unit) => State.OnDeploy(unit);
		
		public bool OnAttack(Unit unit, object defender, bool isDirectAttack) => State.OnAttack(unit, defender, isDirectAttack);
		
		public bool OnActivation(Support card, Array<int> targets) => State.OnActivation(card, targets);
		
		public bool OnSetFaceDown(Support support) => State.OnSetFaceDown(support);

		public bool OnPriorityPassed() => State.OnPassPlay();

		public bool OnEndTurn() => State.OnEndTurn();

		public void SetState(State newState)
		{
			State = newState;
			State.OnEnter(this);
			DeclarePlay(new SetState(this, State));
		}
		
		public Player(int id, List<SetCodes> deckList)
		{
			DeckList = deckList;
			Id = id;
			// Shuffle = shuffle;
		}

		#region Commands

		public void LoadDeck(Gamestate game)
		{
			foreach (SetCodes setCode in DeckList)
			{
				var card = Library.Create(setCode);
				card.Skill.GameState = game;
				card.Owner = this;
				card.Controller = this;
				card.Zone = Deck;
				game.RegisterCard(card);
				Deck.Add(card);
			}

			DeclarePlay(new LoadDeck(Deck.ToList()));
		}

		public void DeclarePlay(GameEvent gameEvent) => EmitSignal(nameof(PlayExecuted), this, gameEvent);
		

		public void Shuffle()
		{
			// TODO: Implement Shuffle
		}

		public void DrawCards(int drawCount)
		{
			for (var i = 0; i < drawCount; i++)
			{
				if (Deck.Count == 0)
				{
					Opponent.Win();
				}
				Draw(1);
			}
		}

		public void Draw(int drawCount)
		{
			var cards = new List<Card>();
			var card = Deck[Deck.Count-1];
			Deck.RemoveAt(Deck.Count-1);
			Hand.Add(card);
			card.Zone = Hand;
			cards.Add(card);
			DeclarePlay(new Draw(cards));
		}

		public void Discard(Card card)
		{
			Hand.Remove(card);
			Graveyard.Add(card);
			card.Zone = Graveyard;
			card.EmitSignal(nameof(Card.Exit));
			DeclarePlay(new Discard(card));
		}
		
		
		public void SetTargets(Card selector, List<Card> targets)
		{
			DeclarePlay(new SetTargets(selector, targets));
		}

		public void ShowAttack(Player player, Unit attacker, Unit defender)
		{
			DeclarePlay(new ShowAttack(attacker, defender));
		}

		public bool HasTag(Tag tag) => Tags.Exists(decorator => decorator.Tag == tag);
		

		public void ReadyCard(Card card)
		{
			card.Ready = true;
			DeclarePlay(new ReadyCard(card));
		}

		public void UnreadyCard(Card card)
		{
			card.Ready = false;
			DeclarePlay(new UnreadyCard(card));
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
			Hand.Remove(card);
			Field.Add(card);
			card.Zone = Field;
			card.EmitSignal(nameof(Card.Exit));
			DeclarePlay(new Deploy(card));
		}

		public void DestroyUnit(Unit card)
		{
			// This might be causing problems elsewhere?
			if (card.HasTag(Tag.CannotBeDestroyedByEffect))
			{
				return;
			}
			if (!card.Controller.Field.Contains(card))
			{
				return;
			}
			card.Controller.Field.Remove(card);
			card.Owner.Graveyard.Add(card);
			card.Zone = card.Owner.Graveyard;
			card.EmitSignal(nameof(Card.Exit));
			
			// This is (currently) required to make sure the animations sync
			if(card.Zone == Graveyard)
				DeclarePlay(new DestroyUnits(new List<Unit> { card }));
			else
			{
				Opponent.DeclarePlay(new DestroyUnits(new List<Unit> { card }));
			}
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

		public void EndTurn() { DeclarePlay(new EndTurn()); }
		
		public void Win() { DeclarePlay(new GameOver(this, Opponent)); }

		#endregion
		
		
	}
	
}
