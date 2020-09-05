using System.Collections.Generic;
using CardGame.Server;
using CardGame.Server.Game;

namespace CardGame.Tests
{
    public class GameFixture: WAT.Test
    {
        private List<Player> Players = new List<Player>();
        protected MockMessenger Play;// Replace with test focused
        private Match Match;
        protected Player Player;
        protected Player Opponent;

        protected void StartGame(List<SetCodes> deckList, List<SetCodes> deckList2 = null)
        {
            deckList2 ??= deckList;
            Players.Add(new Player(1, deckList));
            Players.Add(new Player(2, deckList2));
            Play = new MockMessenger();
            Match = new Match(new Players(Players[0], Players[1]), Play);
            AddChild(Match);
            foreach(var player in Players){ Play.SetReady(player.Id); }

            var turnPlayer = Match.TurnPlayer;
            Player = turnPlayer;
            Opponent = turnPlayer.Opponent;
        }

        public override void Post()
        {
            Players.Clear();
            Play.Free();
            Match.Free();
        } 
    }
    
}
