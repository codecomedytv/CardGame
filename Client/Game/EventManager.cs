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
					case GameEvents.EndTurn:
						Player.EndTurn();
						break;
					case GameEvents.Win:
						Player.Win();
						break;
					case GameEvents.Lose:
						Player.Lose();
						break;
					case GameEvents.Deploy:
						Player.Deploy(message["id"]);
						break;
					case GameEvents.OpponentDeploy:
						Opponent.Deploy(message["id"], (SetCodes)message["setCode"]);
						break;
					case GameEvents.SetFaceDown:
						Player.SetFaceDown(message["id"]);
						break;
					case GameEvents.OpponentSetFaceDown:
						Opponent.SetFaceDown();
						break;
					case GameEvents.LoseLife:
						Player.LoseLife(message["lifeLost"]);
						break;
					case GameEvents.OpponentLoseLife:
						Opponent.LoseLife(message["lifeLost"]);
						break;
					case GameEvents.CardDestroyed:
						Player.DestroyUnit(message["id"]);
						break;
					case GameEvents.OpponentCardDestroyed:
						Opponent.DestroyUnit(message["id"]);
						break;
					case GameEvents.AttackedUnit:
						Player.AttackUnit(message["attackerId"], message["defenderId"]);
						break;
					case GameEvents.OpponentAttackedUnit:
						Opponent.AttackUnit(message["attackerId"], message["defenderId"]);
						break;
					case GameEvents.ReadyCard:
						Player.ReadyCard(message["id"]);
						break;
					case GameEvents.UnreadyCard:
						Player.UnreadyCard(message["id"]);
						break;
					case GameEvents.AttackedDirectly:
						Player.AttackDirectly(message["attackerId"]);
						break;
					case GameEvents.OpponentAttackedDirectly:
						Opponent.AttackDirectly(message["attackerId"]);
						break;
					case GameEvents.OpponentActivate:
						//Opponent.Activate(arguments);
						break;
					case GameEvents.Resolve:
						Resolve();
						break;
					case GameEvents.SetTargets:
						//Player.SetTargets((int)arguments[0], (Array)arguments[1]);
						break;
					case GameEvents.Bounce:
						Player.Bounce(message["id"]);
						break;
					case GameEvents.OpponentBounce:
						Opponent.Bounce(message["id"]);
						break;
					case GameEvents.AttackDeclared:
						Opponent.ShowAttack(message["attackerId"], message["defenderId"]);
						break;
					case GameEvents.AutoTarget:
						Player.AutoTarget(message["id"]);
						break;
					case GameEvents.SetState:
						Player.SetState(message["state"]);
						break;
					case GameEvents.SetDeployable:
						Player.SetDeployable(message["id"]);
						break;
					case GameEvents.SetAsAttacker:
						Player.SetAttacker(message["id"]);
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

		private void Resolve()
		{
			Player.Resolve();
			Opponent.Resolve();
			Player.Link.Clear();
		}

		
	}
}


