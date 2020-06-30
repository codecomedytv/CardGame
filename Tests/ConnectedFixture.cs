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
	    private PackedScene MainGame = (PackedScene) ResourceLoader.Load("res://Client/Room/Game.tscn");
 	    protected ServerConn Server;
	    protected readonly Array<ClientConn> Clients = new Array<ClientConn>();
	    private readonly Array<SetCodes> DeckList = new Array<SetCodes>();

	    

	    protected async void AddGame()
	    {
		    /* Solution 1
				We can store a Read Only Variable in the Client Class that set with a default but
				we can also override it from the constructor (or we load the default in via the constructor) and then
				we can pass it in from here (why is ClientConn a scene anyway? It should be fine as a Node? I guess it
				is an issue with the exports
				
				Now that we can replace the gametype, we can create an extended type that contains test-only accessors.
				This isn't perfect but it is better than nothing.
		    */
		    
		    // We need direct access to cards
		    // We also need to be able to reason about card ids?
		    // Considering we are in control of the card order we could just work via IDs
		    // BUT! This is likely flaky (what if we wrapped it in a method?)
		    // We also need access to the EndTurn & PassPlay buttons (we can always access these through the tree)
		    // On top of that we can't just click on a card, we need to be able to click on the card in the correct game
		    // which essentially means we have two different card catalogs we need to track
		    
		    // Most of our actions are coming via card-catalog so we may only need those
		    // but what if we wanted to check the state of the game?
		    // We could always
		    Server = new ServerConn();
		    Clients.Add(new ClientConn());
		    Clients.Add(new ClientConn());
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

