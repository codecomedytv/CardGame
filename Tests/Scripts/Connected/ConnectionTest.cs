using Godot;

namespace CardGame.Tests.Scripts.Connected
{
    public class ConnectionTest: ConnectedFixture
    {
	    public override string Title()
	    {
		    return "Given A Server and Two Clients";
	    }

	    public override void Pre()
	    {
		    AddGame();
	    }
		
		[Test]
		public void AllAreConnected()
		{
			Assert.IsEqual(Server.Server.GetConnectionStatus(),
				NetworkedMultiplayerPeer.ConnectionStatus.Connected);
				
			Assert.IsEqual(Clients[0].client.GetConnectionStatus(),
				NetworkedMultiplayerPeer.ConnectionStatus.Connected);
				
			Assert.IsEqual(Clients[1].client.GetConnectionStatus(),
				NetworkedMultiplayerPeer.ConnectionStatus.Connected);
		}
		
		[Test]
		public async void GameRoomIsSetup()
		{
			await ToSignal(UntilTimeout(0.2), YIELD);
			Assert.IsEqual(Server.GetChildCount(), 1);
			Assert.IsEqual(Clients[0].GetChildCount(), 1);
			Assert.IsEqual(Clients[1].GetChildCount(), 1);
		}
		
		[Test]
		public async void RoomSharesNameAcrossNetwork()
		{
			await ToSignal(UntilTimeout(0.2), YIELD);
			Assert.IsEqual(Server.GetChild(0).Name, "1");
			Assert.IsEqual(Clients[0].GetChild(0).Name, "1");
			Assert.IsEqual(Clients[1].GetChild(0).Name, "1");
		}
		
	
		
	    public override void Post()
	    {
		    RemoveGame();
	    }
    }
}
/*
 * func title() -> String:
	return "Given a Server and two Clients"

func pre() -> void:
	add_game()
	
func post() -> void:
	remove_game()
	
func test_when_setup() -> void:
	describe("When setup")

	asserts.is_equal(server._server.get_connection_status(), \
			  NetworkedMultiplayerPeer.CONNECTION_CONNECTED, \
			 "Then the Server is connected to itself")

	asserts.is_equal(clients[0]._client.get_connection_status(), \
			 NetworkedMultiplayerENet.CONNECTION_CONNECTED, \
			"Then Client 0 is connected to the server")

	asserts.is_equal(clients[1]._client.get_connection_status(), \
			 NetworkedMultiplayerENet.CONNECTION_CONNECTED, \
			"Then Client 1 is connected to the Server")
*/
