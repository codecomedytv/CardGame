using CardGame;
using CardGame.Client;
using CardGame.Server;
using Godot;
using Godot.Collections;

namespace CardGame.Tests.Scripts
{
    public class ConnectedFixture: WAT.Test
    {
	    private readonly PackedScene PackedServer = (PackedScene) ResourceLoader.Load("res://Server/Server.tscn");
	    private readonly PackedScene PackedClient = (PackedScene) ResourceLoader.Load("res://Client/Client.tscn");
 	    protected ServerConn Server;
	    protected readonly Array<ClientConn> Clients = new Array<ClientConn>();
	    private readonly Array<SetCodes> DeckList = new Array<SetCodes>();

	    protected async void AddGame()
	    {
		    Server = PackedServer.Instance() as ServerConn;
		    Clients.Add(PackedClient.Instance() as ClientConn);
		    Clients.Add(PackedClient.Instance() as ClientConn);
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

