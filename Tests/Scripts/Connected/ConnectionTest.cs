namespace CardGameSharp.Tests.Scripts.Connected
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
	    public void xxx()
	    {
		    Assert.IsTrue(true);
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
