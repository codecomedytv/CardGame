using System;
using CardGame.Client;
using CardGame.ManualTestStates;
using CardGame.Server;
using Godot;

namespace CardGame
{
	public class Main : Control
	{
		public ManualStates ManualTestStates;
		public override void _Ready()
		{
			ManualTestStates = new ManualStates();
			GetNode<Button>("Options/Play").Connect("pressed", this, "Start");
			GetNode<Button>("Options/HostJoin").Connect("pressed", this, "HostJoin");
			GetNode<Button>("Options/Join").Connect("pressed", this, "Join");
			GetNode<Button>("Options/BattleState").Connect("pressed", this, "BattleState");
			GetNode<Button>("Options/WinLose").Connect("pressed", this, "WinLose");

		}

		private void AddClients(int count = 1)
		{
	
			var server = new ServerConn();
			var client = new ClientConn();
			var client2 = new ClientConn();
			server.Name = "Server";
			client.Name = "Client";
			client2.Name = "Client2";
			client.RectMinSize = new Vector2(1920, 1080);
			client2.RectMinSize = new Vector2(1920, 1080);
			client.SizeFlagsHorizontal = (int) SizeFlags.Fill;
			client.SizeFlagsVertical = (int) SizeFlags.Fill;
			client2.SizeFlagsHorizontal = (int) SizeFlags.Fill;
			client2.SizeFlagsVertical = (int) SizeFlags.Fill;
			server.Visible = false;
			GetNode<VBoxContainer>("ScrollContainer/VBoxContainer").AddChild(server, true);
			GetNode<VBoxContainer>("ScrollContainer/VBoxContainer").AddChild(client, true);
			GetNode<VBoxContainer>("ScrollContainer/VBoxContainer").AddChild(client2, true);
		}

		public void Start()
		{
			AddClients();
			OS.WindowFullscreen = true;
			GetNode<VBoxContainer>("Options").Hide();
			GetNode<ServerConn>("ScrollContainer/VBoxContainer/Server").Host();
			GetNode<ClientConn>("ScrollContainer/VBoxContainer/Client").Join();
			GetNode<ClientConn>("ScrollContainer/VBoxContainer/Client2").Join();
		}

		public void HostJoin()
		{
			AddClients();
			GetNode<ServerConn>("ScrollContainer/VBoxContainer/Server").Host();
			GetNode<ClientConn>("ScrollContainer/VBoxContainer/Client").Join();
		}

		public void Join()
		{
			AddClients();
			GetNode<ClientConn>("ScrollContainer/VBoxContainer/Client").Join();
		}

		public async void BattleState()
		{
			OS.WindowFullscreen = true;
			GetNode<VBoxContainer>("Options").Visible = false;
			await ManualTestStates.AddGame(GetNode<VBoxContainer>("ScrollContainer/VBoxContainer"));
			ManualTestStates.BattleState();
		}

		public void WinLose()
		{
			OS.WindowFullscreen = true;
			GetNode<VBoxContainer>("Options").Visible = false;
			ManualTestStates.WonGame(GetNode<VBoxContainer>("ScrollContainer/VBoxContainer"));
		}
		
		public override void _Input(InputEvent inputEvent)
		{
			if (!(inputEvent is InputEventKey key) || !key.Pressed) return;
			switch (key.Scancode)
			{
				case (uint) KeyList.Q:
				{
					GetTree().Quit();
					break;
				}
				case (uint) KeyList.F:
				{
					OS.WindowFullscreen = !OS.WindowFullscreen;
					break;
				}
				case (uint) KeyList.Key7:
				{
					OS.WindowSize = new Vector2(1280, 720);
					break;
				}
				case (uint) KeyList.M:
					{
						OS.WindowSize = new Vector2(640, 1136);
						break;
					}
			}
		}
	}
}
