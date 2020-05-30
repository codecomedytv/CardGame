using System;
using System.Collections.Generic;
using CardGame.Client.Library.Card;
using Godot;
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

        public States State = States.Passive;
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

        [Signal]
        public delegate void PlayerWon();

        [Signal]
        public delegate void PlayerLost();
        
        // This is more of a singleton of cards the player knows about
        // We got to make sure this is passed-by-ref only (before fixing it entirely)
        public Directory Cards;

        // This is probably not right?
        public Player Opponent;

        // This has a setget attached to it in GDScript
        public bool Active = false;

        public List<Card> Link = new List<Card>();
        public object Input;
        

    }
}
