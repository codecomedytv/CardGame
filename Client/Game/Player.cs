using System;
using System.Collections.Generic;
using CardGame.Client.Library.Card;
using CardGameSharp.Client.Game;
using Godot;
using Godot.Collections;
using Array = Godot.Collections.Array;

//using Godot.Collections;
// using Array = Godot.Collections.Array;


namespace CardGame.Client.Match
{
    public class Player: Reference
    {
        public enum States
        {
            Idle,
            Active,
            Passive,
            Acting,
            Passing,
            Targeting
        }

        public string State = "NotSet";
        public bool IsTurnPlayer = false;
        public int Health = 8000;
        public int DeckSize = 0;
        public List<Card> Hand = new List<Card>();
        public List<Card> Field = new List<Card>();
        public List<Card> Graveyard = new List<Card>();
        public List<Card> Support = new List<Card>();
        public bool Won = false;
        public bool Lost = false;
        public Visual Visual;
        public Dictionary Cards = new Dictionary();
        public Opponent Opponent;
        public List<Card> Link = new List<Card>();
        public GameInput Input;
        public Interact Interact;

        // This has a setget attached to it in GDScript
        public bool Active = false;




        [Signal]
        public delegate void PlayerWon();

        [Signal]
        public delegate void PlayerLost();
        

        public bool _get_active()
		{
			return State == "Idle" || State == "Active";
		}

		public void SetState(Array args) 
		{
			State = (string)args[0];
			Visual.SetState(State);
	    }
			
		public void SetDeployable(Array args)
		{
			if (Cards[args[0]] is Card card)
			{
				card.CanBeDeployed = true;
			}
	    }

		public void SetSettable(Array args)
		{
			if (Cards[args[0]] is Card card)
			{
				card.CanBeSet = true;
			}
		}

		public void SetActivatable(Array args) 
		{
			if (Cards[args[0]] is Card card)
			{
				card.CanBeActivated = true;
			}
			
		}

		public void autotarget(Array args) 
		{
			var targeter = Cards[args[0]];
			// change States
		    //	State = "Targeting"
		    State = "Targeting";
			Visual.AutoTarget(targeter as Card);
		    //	_show_valid_targets(targeter);
		}

		public void SetUp(Dictionary cards) 
		{
			Cards = cards;
	    }
			
		public void SetTargets(Array args) 
		{
			if (Cards[args[0]] is Card card)
			{
				// Might be not a array?
				card.ValidTargets = args[1] as Array;
			}
	    }

		public void Resolve()
		{
			var linked = new Array<Card>();
			foreach (var card in Link)
			{
				card.Activated = false;
				if (card.UnderPlayersControl)
				{
					linked.Add(card);
				}
			}

			Visual.Resolve(linked);
	    }

		public void AttackUnit(Array args)
		{
			var attacker = Cards[args[0]] as Card;
			var defender = Cards[args[1]] as Card;
			Visual.AttackUnit(attacker, defender);
	    }
			
		public void attack_directly(Array args) 
		{
			var attacker = Cards[args[0]] as Card;
			Visual.AttackDirectly(attacker);
	    }
			
		public void bounce(Array args) 
		{
			var card= Cards[args[0]] as Card;
			Field.Remove(card);
			Hand.Add(card);
			if (card == null) return;
			card.Zone = Card.Zones.Hand;
			Visual.Bounce(card);
		}
			
		public void Deploy(Array args) 
		{
			var card = Cards[args[0]] as Card;
			Hand.Remove(card);
			Field.Add(card);
			if (card != null) card.Zone = Card.Zones.Unit;
			Visual.Deploy(card);
	    }
			
		public void Activate(Array args) {
			var card = Cards[args[0]] as Card;
			Support.Remove(card);
			Graveyard.Add(card);
			if (card == null) return;
			card.Zone = Card.Zones.Discard;
			Visual.Activate(card, Link, new Array<Card>());
		}

		public void SetFaceDown(Array args) {
			var card = Cards[args[0]] as Card;
			Hand.Remove(card);
			Support.Add(card);
			if (card != null) card.Zone = Card.Zones.Support;
			Visual.SetFaceDown(card);
			}
		

		public void Draw(Array args)
		{
			DeckSize -= args.Count;
			foreach (var c in args)
			{
				if (c is Dictionary codes)
				{
					var id = codes["Id"];
					var setCode = codes["setCode"];
					var card = Library.Library.Fetch((int) id, (SetCodes) setCode) as Card;
					if (card == null) continue;
					card.Connect("CardActivated", Input, "Activate");
					card.Player = this;
					card.Interact = Interact;
					card.UnderPlayersControl = true;
					Cards[card.Id] = card;
					Hand.Add(card);
					card.Zone = Card.Zones.Hand;
				}
			}

			Visual.Draw(args, this);
	    }

		public void DestroyUnit(Array args) {
			var card = Cards[args[0]] as Card;
			Field.Remove(card);
			Graveyard.Add(card);
			card.Zone = Card.Zones.Discard;
			Visual.DestroyUnit(card);
			}
			
		public void LoseLife(Array args)
		{
			var lost = (int) args[0];
			Health -= lost;
			Visual.LoseLife(lost);
			}
		
		public void ReadyCards(Array cardIds)
		{
			foreach (var id in cardIds)
			{
				if (Cards[id] is Card card) card.IsReady = true;
			}
			
			Visual.ReadyCards(cardIds);
		}

		public void UnreadyCards(Array cardIds) 
		{
			foreach (var id in cardIds)
			{
				if (Cards[id] is Card card) card.IsReady = false;
			}

			Visual.UnreadyCards(cardIds);
		}

		public void LoadDeck(int deckSize)
		{
			DeckSize = deckSize;
			Visual.LoadDeck(deckSize);
		}

		public void BeginTurn() 
		{
			IsTurnPlayer = true;
			Visual.BeginTurn();
		}

		public void EndTurn() 
		{
			IsTurnPlayer = false;
			Visual.EndTurn();
		}

		public void Win() 
		{
			Won = true;
			EmitSignal(nameof(PlayerWon));
		}

		public void Lose() 
		{
			Lost = true;
			EmitSignal(nameof(PlayerLost));
		}


		public void SetAttacker(Array args)
		{
			if (Cards[args[0]] is Card card)
			{
				card.CanAttack = true;
			}
		}
    }
}
