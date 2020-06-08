using CardGame;
using CardGame.Client;
using CardGame.Server;
using Godot;
using Godot.Collections;

namespace CardGame.Tests.Scripts
{
    public class ConnectedFixture: WAT.Test
    {
	    protected ServerConn Server;
	    protected Array<ClientConn> Clients = new Array<ClientConn>();
	    protected Array<SetCodes> DeckList = new Array<SetCodes>();

	    public async void AddGame()
	    {
		    var server = ResourceLoader.Load("res://Server/Server.tscn") as PackedScene;
			Server = server.Instance() as ServerConn;
		    // In the old tests we added a fakeshuffle here but all shuffles are fake for now
		    var client = ResourceLoader.Load("res://Client/Client.tscn") as PackedScene;
		    Clients.Add(client.Instance() as ClientConn);
		    Clients.Add(client.Instance() as ClientConn);
		    // We muted clients sounds here but it is muted by default;
		    AddChild(Server);
		    await ToSignal(UntilSignal(Server.Server, "connection_succeeded", 1.0), YIELD);
		    AddChild(Clients[0]);
		    AddChild(Clients[1]);
		    if (DeckList.Count != 0)
		    {
			    Clients[0].DeckList = DeckList;
			    Clients[1].DeckList = DeckList;
		    }
		    Clients[0].Join();
		    await ToSignal(UntilSignal(Clients[0].Multiplayer, "connected_to_server", 1.0), YIELD);
		    Clients[1].Join();
		    await ToSignal(UntilSignal(Clients[1].Multiplayer, "connected_to_server", 1.0), YIELD);
		    await ToSignal(UntilTimeout(0.2), YIELD);


	    }

	    public void RemoveGame()
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

