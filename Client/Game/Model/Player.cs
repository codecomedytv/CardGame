using System;
//using System.Collections.Generic;
using CardGame.Client.Library.Card;
using Zone = System.Collections.Generic.List<CardGame.Client.Library.Card.Card>;
using CardGame.Client.Match;
using Godot;
using Godot.Collections;
using Array = Godot.Collections.Array;

//using Godot.Collections;
// using Array = Godot.Collections.Array;


namespace CardGame.Client.Match.Model
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
        public Zone Hand = new Zone();
        public Zone Field = new Zone();
        public Zone Graveyard = new Zone();
        public Zone Support = new Zone();
        public bool Won = false;
        public bool Lost = false;
        public View.Player Visual;
        public Dictionary<int, Card> Cards;
        public Opponent Opponent;
        public Zone Link = new Zone();
        public GameInput Input;
        public Interact Interact;

        [Signal]
        private delegate void PlayerWon();

        [Signal]
        private delegate void PlayerLost();
        
	    public void SetState(int state)
	    {
		    //Visual.SetState(state);
	    }

	    public void SetDeployable(int id)
	    {	
		    GD.Print(id); // We're bouncing the card back so it isn't in our hand yet?
		    GD.Print(Cards.Keys.Contains(id));
		    Cards[id].CanBeDeployed = true;
	    }

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
		
		protected Player()
		{
			
		}

		public Player(Dictionary<int, Card> cards)
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
		

		public void Draw(int id, SetCodes setcode)
		{
			DeckSize -= 1;
			var card = Library.Library.Fetch(id, setcode);
			card.Connect("CardActivated", Input, "Activate");
			card.Player = this;
			card.Interact = Interact;
			card.UnderPlayersControl = true;
			Cards[card.Id] = card;
			Hand.Add(card);
			card.Zone = Card.Zones.Hand;
			Visual.Draw(card, DeckSize);
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
		
		public void ReadyCard(int id)
		{
			var card = Cards[id];
			card.IsReady = true;
			Visual.ReadyCard(card);
		}

		public void UnreadyCard(int id)
		{
			var card = Cards[id];
			card.IsReady = false;
			Visual.UnreadyCard(card);
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

		public void Move(int from, int id, int to)
		{
			GD.Print("Moving");
		}
    }
}
