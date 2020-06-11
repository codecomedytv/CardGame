using System.Collections.Generic;
using System.Linq;
using CardGame.Server;

namespace CardGame.Tests.Scripts
{
    public class GameFixture: WAT.Test
    {

    public List<Player> Players = new List<Player>();
    // public Gamestate GameState = new Gamestate();
    public MockMessenger Play;// Replace with test focused
    public Room Game;

    protected void StartGame(List<SetCodes> deckList, List<SetCodes> deckList2 = null)
    {
        deckList2 = deckList2 != null ? deckList2 : deckList.ToList();
        Players.Add(new Player(1, deckList.ToList()));
        Players.Add(new Player(2, deckList2.ToList()));
        Play = new MockMessenger();
        Game = new Room(Players, Play);
        AddChild(Game);
        foreach(var player in Players){ Play.SetReady(player.Id); }
    }

    public override void Post()
    {
        Players.Clear();
        Play.Free();
        Game.Free();
    }
    }
    
}
