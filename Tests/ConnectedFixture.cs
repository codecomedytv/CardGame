using CardGame;
using CardGame.Client;
using CardGame.Client.Room;
using CardGame.Server;
using Godot;
using Godot.Collections;

namespace CardGame.Tests.Scripts
{
    public class ConnectedFixture: WAT.Test
    {
	    private readonly CSharpScript MockGame = (CSharpScript) ResourceLoader.Load("res://Tests/MockGame.cs");
 	    protected ServerConn Server;
	    protected readonly Array<ClientConn> Clients = new Array<ClientConn>();
	    private readonly Array<SetCodes> DeckList = new Array<SetCodes>();
	    
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
		    if (DeckList.Count != 0)
		    {
			    Clients[0].DeckList = DeckList;
			    Clients[1].DeckList = DeckList;
		    }
		    Server.Host();
		    Clients[0].Join();
		    Clients[1].Join();
		    await ToSignal(UntilSignal(Server.Server, "connection_succeeded", 1.0), YIELD);
		    await ToSignal(UntilSignal(Clients[0].Multiplayer, "connected_to_server", 1.0), YIELD);
		    await ToSignal(UntilSignal(Clients[1].Multiplayer, "connected_to_server", 1.0), YIELD);
		    
		    // We access the Mock Game Nodes via Tree
		    // Clients[0].GetNode<MockGame>("1");
		    // Clients[1].GetNode<MockGame>("1");

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
		    DeckList.Clear();
		    Server.Free();
	    }
	    
    }
}

