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
        public Godot.Collections.Dictionary<int, Card> Cards = new Godot.Collections.Dictionary<int, Card>();
        public Opponent Opponent;
        public List<Card> Link = new List<Card>();
        public GameInput Input;
        public Interact Interact;

        [Signal]
        private delegate void PlayerWon();

        [Signal]
        private delegate void PlayerLost();
        
	    public void SetState(string state) => Visual.SetState(state); 
			
		public void SetDeployable(int id) => Cards[id].CanBeDeployed = true;

	    public void SetSettable(int id) => Cards[id].CanBeSet = true;

		public void SetActivatable(int id) => Cards[id].CanBeActivated = true;

		public void AutoTarget(int id) 
		{
			var targeter = Cards[id];
			// change States
		    //	State = "Targeting"
		    State = "Targeting";
			Visual.AutoTarget(targeter);
		    //	_show_valid_targets(targeter);
		}

		public void SetUp(Godot.Collections.Dictionary<int, Card> cards) 
		{
			Cards = cards;
	    }
			
		public void SetTargets(int id, Array targets) => Cards[id].ValidTargets = targets;
		
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

		public void AttackUnit(int attackerId, int defenderId)
		{
			var attacker = Cards[attackerId];
			var defender = Cards[defenderId];
			Visual.AttackUnit(attacker, defender);
	    }
			
		public void AttackDirectly(int attackerId) => Visual.AttackDirectly(Cards[attackerId]);
		
		public void Bounce(int id)
		{
			var card = Cards[id];
			Field.Remove(card);
			Hand.Add(card);
			card.Zone = Card.Zones.Hand;
			Visual.Bounce(card);
		}
			
		public void Deploy(int id) 
		{
			var card = Cards[id];
			Hand.Remove(card);
			Field.Add(card);
			card.Zone = Card.Zones.Unit;
			Visual.Deploy(card);
	    }
		
		public void SetFaceDown(int id) 
		{
			var card = Cards[id];
			Hand.Remove(card);
			Support.Add(card);
			card.Zone = Card.Zones.Support;
			Visual.SetFaceDown(card);
	    }
		

		public void Draw(Dictionary codes)
		{
			DeckSize -= 1;
			var card = Library.Library.Fetch((int)codes["id"], (SetCodes)codes["setCode"]);
			card.Connect("CardActivated", Input, "Activate");
			card.Player = this;
			card.Interact = Interact;
			card.UnderPlayersControl = true;
			Cards[card.Id] = card;
			Hand.Add(card);
			card.Zone = Card.Zones.Hand;
			Visual.Draw(card, this);
	    }

		public void DestroyUnit(int id) 
		{
			var card = Cards[id];
			Field.Remove(card);
			Graveyard.Add(card);
			card.Zone = Card.Zones.Discard;
			Visual.DestroyUnit(card);
	    }
			
		public void LoseLife(int lostLife)
		{
			Health -= lostLife;
			Visual.LoseLife(lostLife);
	    }
		
		public void ReadyCards(Array cardIds)
		{
			foreach (var id in cardIds)
			{
				if (Cards[(int)id] is Card card) card.IsReady = true;
			}
			
			Visual.ReadyCards(cardIds);
		}

		public void UnreadyCards(Array cardIds) 
		{
			foreach (var id in cardIds)
			{
				if (Cards[(int)id] is Card card) card.IsReady = false;
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


		public void SetAttacker(int id)
		{
			Cards[id].CanAttack = true;
		}
    }
}
