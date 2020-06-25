using System.Collections.Generic;
using System.Linq;
using CardGame.Server;
using CardGame.Server.Game;
using CardGame.Server.Game.Network;

namespace CardGame.Tests.Scripts
{
    public class GameFixture: WAT.Test
    {

    public List<Player> Players = new List<Player>();
    // public Gamestate GameState = new Gamestate();
    public MockMessenger Play;// Replace with test focused
    public Match Match;
    protected Player TurnPlayer;
    protected Player Player;
    protected Player Opponent;

    protected void StartGame(List<SetCodes> deckList, List<SetCodes> deckList2 = null)
    {
        deckList2 = deckList2 != null ? deckList2 : deckList.ToList();
        Players.Add(new Player(1, deckList.ToList()));
        Players.Add(new Player(2, deckList2.ToList()));
        //Play = new MockMessenger();
        Play = new MockMessenger();
        Match = new Match(new Players(Players[0], Players[1]), Play);
        AddChild(Match);
        foreach(var player in Players){ Play.SetReady(player.Id); }

        TurnPlayer = Match.TurnPlayer;
        Player = TurnPlayer;
        Opponent = TurnPlayer.Opponent;
    }

    public override void Post()
    {
        TurnPlayer = null;
        Players.Clear();
        Play.Free();
        Match.Free();
    }
    }
    
}
