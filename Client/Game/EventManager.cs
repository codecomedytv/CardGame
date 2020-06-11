using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Array = Godot.Collections.Array;
using CardGame.Client.Match.Model;

namespace CardGame.Client.Match
{
    public class EventManager: Reference
    {
        [Signal]
        private delegate void Animated();

        private Player Player;
        private Opponent Opponent;

        private List<(GameEvents Command, Array Arguments)> Events = new List<(GameEvents Command, Array Arguments)>();

        public void SetUp(Player player, Opponent opponent)
        {
            Player = player;
            Opponent = opponent;
        }

        public void Queue(int command, Array args)
        {
	        var message = (Command: (GameEvents) command, Arguments: args);
	        Events.Add(message);
        } 
        
        public void Execute()
		{
			foreach(var (command, arguments) in Events)
			{
				switch(command)
				{
					case GameEvents.Draw:
						Player.Draw((Dictionary)arguments[0]);
						break;
					case GameEvents.LoadDeck:
						Player.LoadDeck((int)arguments[0]);
						break;
					case GameEvents.OpponentDraw:
						Opponent.Draw();
						break;
					case GameEvents.OpponentLoadDeck:
						Opponent.LoadDeck((int)arguments[0]);
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
						Player.Deploy((int)arguments[0]);
						break;
					case GameEvents.OpponentDeploy:
						Opponent.Deploy((Dictionary)arguments[0]);
						break;
					case GameEvents.SetFaceDown:
						Player.SetFaceDown((int)arguments[0]);
						break;
					case GameEvents.OpponentSetFaceDown:
						Opponent.SetFaceDown();
						break;
					case GameEvents.LoseLife:
						Player.LoseLife((int)arguments[0]);
						break;
					case GameEvents.OpponentLoseLife:
						Opponent.LoseLife((int)arguments[0]);
						break;
					case GameEvents.CardDestroyed:
						Player.DestroyUnit((int)arguments[0]);
						break;
					case GameEvents.OpponentCardDestroyed:
						Opponent.DestroyUnit((int)arguments[0]);
						break;
					case GameEvents.AttackedUnit:
						Player.AttackUnit((int)arguments[0], (int)arguments[1]);
						break;
					case GameEvents.OpponentAttackedUnit:
						Opponent.AttackUnit((int)arguments[0], (int)arguments[1]);
						break;
					case GameEvents.ReadyCard:
						Player.ReadyCard((int)arguments[0]);
						break;
					case GameEvents.UnreadyCard:
						Player.UnreadyCard((int)arguments[0]);
						break;
					case GameEvents.AttackedDirectly:
						Player.AttackDirectly((int)arguments[0]);
						break;
					case GameEvents.OpponentAttackedDirectly:
						Opponent.AttackDirectly((int)arguments[0]);
						break;
					case GameEvents.OpponentActivate:
						Opponent.Activate(arguments);
						break;
					case GameEvents.Resolve:
						Resolve();
						break;
					case GameEvents.SetTargets:
						Player.SetTargets((int)arguments[0], (Array)arguments[1]);
						break;
					case GameEvents.Bounce:
						Player.Bounce((int)arguments[0]);
						break;
					case GameEvents.OpponentBounce:
						Opponent.Bounce((int)arguments[0]);
						break;
					case GameEvents.AttackDeclared:
						Opponent.ShowAttack((int)arguments[0], (int)arguments[1]);
						break;
					case GameEvents.AutoTarget:
						Player.AutoTarget((int)arguments[0]);
						break;
					case GameEvents.SetState:
						Player.SetState((string)arguments[0]);
						break;
					case GameEvents.SetDeployable:
						Player.SetDeployable((int)arguments[0]);
						break;
					case GameEvents.SetAsAttacker:
						Player.SetAttacker((int)arguments[0]);
						break;
					case GameEvents.SetSettable:
						Player.SetSettable((int)arguments[0]);
						break;
					case GameEvents.SetActivatable:
						Player.SetActivatable((int)arguments[0]);
						break;
					case GameEvents.Discard:
						break;
					case GameEvents.OpponentDiscard:
						break;
					case GameEvents.Mill:
						break;
					case GameEvents.OpponentMill:
						break;
					case GameEvents.Activate:
						break;
					case GameEvents.ReturnToDeck:
						break;
					case GameEvents.OpponentReturnedToDeck:
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


