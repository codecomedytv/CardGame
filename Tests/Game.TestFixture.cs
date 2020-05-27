﻿using System.Collections.Generic;
 using System.Linq;
 using CardGame.Server;

 namespace CardGame.Tests
{
    public class GameTestFixture: WAT.Test
    {
        public List<Player> Players = new List<Player>();
        public Gamestate GameState = new Gamestate();
        public IMessenger Play = new MockMessenger(); // Replace with test focused
        public Game Game;

        protected void StartGame(List<SetCodes> deckList, List<SetCodes> deckList2 = null)
        {
            deckList2 = deckList2 != null ? deckList2 : deckList.ToList();
            Players.Add(new Player(1, deckList.ToList()));
            Players.Add(new Player(2, deckList2.ToList()));
            Game = new Game(Players, Play);
            foreach(var player in Players){ Play.SetReady(player.Id); }
        }
    }
    
    
}