using System.Threading.Tasks;
using CardGame;
using CardGame.Client;
using CardGame.Client.Room;
using CardGame.Server;
using Godot;
using Godot.Collections;
using Player = CardGame.Client.Room.Player;

namespace CardGame.Tests.Scripts
{
	// All tests using the connected fixture must await playerState before taking action otherwise they will be
	// moving too quickly for the server

	public class ConnectedFixture: WAT.Test
    {
	    private readonly CSharpScript MockGame = (CSharpScript) ResourceLoader.Load("res://Tests/MockGame.cs");
 	    protected ServerConn Server;
	    protected readonly Array<ClientConn> Clients = new Array<ClientConn>();
	    protected readonly DeckList DeckList = new DeckList();
	    protected MockGame PlayerMockGame;
	    protected MockGame OpponentMockGame;
	    protected Player Player;
	    protected Player Opponent;
	    protected Player OppPlayerFromPlayer;
	    protected Player PlayerPlayerFromOpp;
	    
	    protected async void AddGame()
	    {
		    // The Game Script handles loading its own Scene. This allows us to extend the top level script for
		    // additionally functionality when it comes to test (exposing protected members etc)
		    
		    // Our main goal here is the ability to access cards, interact with them and control the position of the mouse
		    // to do so.
		    
		    // We also need to be able to access PassPlay/EndTurn Buttons
		    Server = new ServerConn();
		    Clients.Add(new ClientConn(MockGame));
		    Clients.Add(new ClientConn(MockGame));
		    AddChild(Server);
		    AddChild(Clients[0]);
		    AddChild(Clients[1]);
		    Server.Host();
		    Clients[0].Join(DeckList);
		    Clients[1].Join(DeckList);
		    await ToSignal(UntilSignal(Server.Server, "connection_succeeded", 1.0), YIELD);
		    await ToSignal(UntilSignal(Clients[0].Multiplayer, "connected_to_server", 1.0), YIELD);
		    await ToSignal(UntilSignal(Clients[1].Multiplayer, "connected_to_server", 1.0), YIELD);
		    PlayerMockGame = Clients[1].GetNode<MockGame>("1");
		    OpponentMockGame = Clients[0].GetNode<MockGame>("1");
		    Player = PlayerMockGame.GetPlayerView();
		    Opponent = OpponentMockGame.GetPlayerView();
		    OppPlayerFromPlayer = PlayerMockGame.GetOpponentView();
		    PlayerPlayerFromOpp = OpponentMockGame.GetOpponentView();
	    }

	    protected Task<object[]> PlayerState => WaitOnState(PlayerMockGame);
	    protected Task<object[]> OpponentState => WaitOnState(OpponentMockGame);
	    private async Task<object[]> WaitOnState(MockGame game)
	    {
		    return await ToSignal(UntilSignal(game, nameof(Game.StateSet), 5), YIELD);
	    }
	    
	    protected void RemoveGame()
	    {
		    RemoveChild(Server);
		    while (Clients.Count > 0)
		    {
			    var client = Clients[0];
			    Clients.RemoveAt(0);
			    client.Free();
		    }

		    Clients.Clear();
		    //DeckList.Clear();
		    Server.Free();
	    }
	    
    }
}

