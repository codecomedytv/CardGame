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
					case GameEvents.SetState:
						Player.SetState(message["state"]);
						break;
					case GameEvents.SetDeployable:
						Player.SetDeployable(message["id"]);
						break;
					case GameEvents.SetSettable:
						Player.SetSettable(message["id"]);
						break;
					case GameEvents.SetActivatable:
						Player.SetActivatable(message["id"]);
						break;
					case GameEvents.MoveCard:
						Player.Move(message["from"], message["id"], message["to"]);
						break;
					case GameEvents.SetProperty:
						var x =(message["id"], message["property"], message["value"]);
						break;
					case GameEvents.CreateCard:
						var c = (message["id"], message["setCode"]);
						break;
					case GameEvents.DeleteCard:
						var g = message["id"];
						break;
					case GameEvents.NoOp:
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			EmitSignal(nameof(Animated));
			Events.Clear();
		}


	}
}


