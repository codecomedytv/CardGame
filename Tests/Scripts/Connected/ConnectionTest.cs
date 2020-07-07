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
				
			Assert.IsEqual(Clients[0].Client.GetConnectionStatus(),
				NetworkedMultiplayerPeer.ConnectionStatus.Connected);
				
			Assert.IsEqual(Clients[1].Client.GetConnectionStatus(),
				NetworkedMultiplayerPeer.ConnectionStatus.Connected);
		}
		
		[Test]
		public async void GameRoomIsSetup()
		{
			await ToSignal(UntilTimeout(0.2), YIELD);
			Assert.IsEqual(Server.GetChildCount(), 1);
			Assert.IsEqualOrGreaterThan(Clients[0].GetChildCount(), 1);
			Assert.IsEqualOrGreaterThan(Clients[1].GetChildCount(), 1);
		}
		
		[Test]
		public async void RoomSharesNameAcrossNetwork()
		{
			await ToSignal(UntilTimeout(0.2), YIELD);
			Assert.IsEqual(Server.GetChild(0).Name, "1");
			Assert.IsNotNull(Clients[0].GetNodeOrNull("1"));
			Assert.IsNotNull(Clients[1].GetNodeOrNull("1"));
		}
		
	    public override void Post()
	    {
		    RemoveGame();
	    }
    }
}

