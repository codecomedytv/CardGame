using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Hosting;
using CardGame;
using CardGame.Client.Match;
using Godot;
using Godot.Collections;
using Array = Godot.Collections.Array;

namespace CardGameSharp.Client.Game
{
    public class EventManager: Reference
    {
        [Signal]
        public delegate void CommandRequested();

        [Signal]
        public delegate void Battle();

        [Signal]
        private delegate void Animated();

        private Player Player;
        private Opponent Opponent;
        private GameInput GameInput;

        private List<(GameEvents Command, Array Arguments)> Events = new List<(GameEvents Command, Array Arguments)>();

        public void SetUp(Player player, Opponent opponent, GameInput gameInput)
        {
            Player = player;
            Opponent = opponent;
            GameInput = gameInput;
        }
        
        public void OnAnimationFinished()
        {
            EmitSignal(nameof(CommandRequested));
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
						Player.Draw(arguments);
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
						Player.LoseLife(arguments);
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
						Player.ReadyCards(arguments);
						break;
					case GameEvents.UnreadyCard:
						Player.UnreadyCards(arguments);
						break;
					case GameEvents.AttackedDirectly:
						Player.attack_directly((int)arguments[0]);
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
						Player.SetTargets(arguments);
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
						Player.autotarget((int)arguments[0]);
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
					case GameEvents.NoOp:
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


