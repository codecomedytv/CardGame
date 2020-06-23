using System.Collections.Generic;
using System.Linq;
using CardGame.Server;
using CardGame.Server.Room;

namespace CardGame.Tests.Scripts
{
    public class GameFixture: WAT.Test
    {

    public List<Player> Players = new List<Player>();
    // public Gamestate GameState = new Gamestate();
    public MockMessenger Play;// Replace with test focused
    public Match Match;

    protected void StartGame(List<SetCodes> deckList, List<SetCodes> deckList2 = null)
    {
        deckList2 = deckList2 != null ? deckList2 : deckList.ToList();
        Players.Add(new Player(1, deckList.ToList()));
        Players.Add(new Player(2, deckList2.ToList()));
        Play = new MockMessenger();
        Match = new Match(Players, Play);
        AddChild(Match);
        foreach(var player in Players){ Play.SetReady(player.Id); }
    }

    public override void Post()
    {
        Players.Clear();
        Play.Free();
        Match.Free();
    }
    }
    
}
