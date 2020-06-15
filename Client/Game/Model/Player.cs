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

	    public void SetDeployable(int id) => Cards[id].CanBeDeployed = true;
	    
	    public void SetSettable(int id) => Cards[id].CanBeSet = true;

		public void SetActivatable(int id) => Cards[id].CanBeActivated = true;
		
		protected Player()
		{
			
		}

		public Player(Dictionary<int, Card> cards)
		{
			Cards = cards;
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

		
		public void LoadDeck(int deckSize)
		{
			DeckSize = deckSize;
			Visual.LoadDeck(deckSize);
		}

		public void Move(int from, int id, int to)
		{
			GD.Print("Moving");
		}
    }
}
