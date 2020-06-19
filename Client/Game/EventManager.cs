using System;
using Godot;
using Godot.Collections;
using CardGame.Client.Match.Model;

namespace CardGame.Client.Match
{
	public class EventManager: Reference
	{
		[Signal]
		private delegate void Animated();

		private Player Player;
		private Opponent Opponent;

		private Array<Dictionary<string, int>> Events = new Array<Dictionary<string, int>>();

		public void SetUp(Player player, Opponent opponent)
		{
			Player = player;
			Opponent = opponent;
		}

		public void Queue(Dictionary<string,int> message)
		{
			Events.Add(message);
		} 
		
		public void Execute()
		{
			foreach(var message in Events)
			{
				var e = (GameEvents)message["command"];
				switch((GameEvents)message["command"])
				{
					case GameEvents.Draw:
						Player.Draw(message["id"], (SetCodes)message["setCode"]);
						break;
					case GameEvents.LoadDeck:
						Player.LoadDeck(message["count"]);
						break;
					case GameEvents.OpponentDraw:
						Opponent.Draw();
						break;
					case GameEvents.OpponentLoadDeck:
						Opponent.LoadDeck(message["count"]);
						break;
					case GameEvents.BeginTurn:
						Player.BeginTurn();
						break;
					case GameEvents.SetState:
						Player.SetState(message["state"]);
						break;
					case GameEvents.SetDeployable:
						Player.SetDeployable(message["id"]);
						break;
					case GameEvents.SetSettable:
						Player.SetSettable(message["id"]);
						break;
					case GameEvents.NoOp:
						break;
					default:
						GD.PushWarning($"Ignore Game Command: {message["Command"]}");
						break;
				}
			}
			EmitSignal(nameof(Animated));
			Events.Clear();
		}
		
	}
}


