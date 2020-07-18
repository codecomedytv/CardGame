using System.Collections.Generic;
using System.Threading.Tasks;
using CardGame.Client;
using CardGame.Client.Cards;
using CardGame.Client.Room;
using CardGame.Server;
using CardGame.Tests;
using Godot;
using Godot.Collections;
using Player = CardGame.Client.Room.Player;

namespace CardGame.ManualTestStates
{
	// Some testing requires physical interaction but also starts off with a similar state which
	// I don't want to have to do manually each time, so instead I'm going to choose from a set of options here.
	// (We just imported most from Tests.ConnectedFixture)
	
	public class ManualStates: Node
	{
		private readonly CSharpScript MockGame = (CSharpScript) ResourceLoader.Load("res://Tests/MockGame.cs");
		protected ServerConn Server;
		protected readonly Array<ClientConn> Clients = new Array<ClientConn>();
		protected readonly DeckList DeckList = new DeckList();
		protected MockGame PlayerMockGame;
		protected MockGame OpponentMockGame;
		protected Player Player;
		protected Player Opponent;
		private Container Container;

		public async Task AddGame(Container container)
		{
			Container = container;
			Server = new ServerConn();
			Clients.Add(new ClientConn(MockGame));
			Clients.Add(new ClientConn(MockGame));
			container.AddChild(Server);
			container.AddChild(Clients[0]);
			container.AddChild(Clients[1]);
			Server.Visible = false;
			Clients[0].RectMinSize = new Vector2(1920, 1080);
			Clients[1].RectMinSize = new Vector2(1920, 1080);
			Clients[0].SizeFlagsHorizontal = (int) Control.SizeFlags.Fill;
			Clients[0].SizeFlagsVertical = (int) Control.SizeFlags.Fill;
			Clients[1].SizeFlagsHorizontal = (int) Control.SizeFlags.Fill;
			Clients[1].SizeFlagsVertical = (int) Control.SizeFlags.Fill;
			Server.Host();
			Clients[0].Join(DeckList);
			Clients[1].Join(DeckList);
			await ToSignal(Clients[0], nameof(ClientConn.GameBegan));
			await ToSignal(Clients[1], nameof(ClientConn.GameBegan));
			PlayerMockGame = Clients[1].GetNode<MockGame>("1");
			OpponentMockGame = Clients[0].GetNode<MockGame>("1");
			Player = PlayerMockGame.GetPlayerView();
			Opponent = OpponentMockGame.GetPlayerView();
		}

		protected Task<object[]> PlayerState => WaitOnState(PlayerMockGame);
		protected Task<object[]> OpponentState => WaitOnState(OpponentMockGame);

		protected Task<object[]> Update => GameState();

		protected async Task<object[]> GameState()
		{
			return await Task.WhenAll(PlayerState, OpponentState);
		}

		private async Task<object[]> WaitOnState(MockGame game)
		{
			return await Container.ToSignal(game, nameof(Game.StateSet));
		}

		public async void BattleState()
		{
			await Update;
			var attacker = Player.Hand[6];
			//attacker.DoubleClick();
			PlayerMockGame.DoubleClick(attacker);
			await Update;
			OpponentMockGame.Pass();
			await Update;
			PlayerMockGame.Pass();
			await Update;
			PlayerMockGame.End();
			await Update;
			var defending = Opponent.Hand[0];
			//defending.DoubleClick();
			OpponentMockGame.DoubleClick(defending);
			await Update;
			PlayerMockGame.Pass();
			await Update;
			OpponentMockGame.Pass();
			await Update;
			OpponentMockGame.End();
			await Update;
			// Our Chance Now
		}

		public async void WonGame(Container container)
		{
			DeckList.Clear();
			DeckList.Add(SetCodes.AlphaDungeonGuide, 40);
			await AddGame(container);
			
			await Update;
			
			foreach (var card in Player.Hand)
			{
				PlayerMockGame.DoubleClick(card);
				await Update;
				OpponentMockGame.Pass();
				await Update;
				PlayerMockGame.Pass();
				await Update;
			}
			
			PlayerMockGame.End();
			await Update;

			foreach (var card in Opponent.Hand)
			{
				OpponentMockGame.DoubleClick(card);
				await Update;
				PlayerMockGame.Pass();
				await Update;
				OpponentMockGame.Pass();
				await Update;
			}

		}

	}
}
