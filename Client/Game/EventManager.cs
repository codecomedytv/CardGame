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
        public delegate void Animated();

        public Player Player;
        public Opponent Opponent;
        public GameInput GameInput;
        
        public List<(GameEvents Command, Array Arguments)> Events = new List<(GameEvents Command, Array Arguments)>();

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
						Opponent.Draw(arguments);
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
						Player.Deploy(arguments);
						break;
					case GameEvents.OpponentDeploy:
						Opponent.Deploy(arguments);
						break;
					case GameEvents.SetFaceDown:
						Player.SetFaceDown(arguments);
						break;
					case GameEvents.OpponentSetFaceDown:
						Opponent.SetFaceDown(arguments);
						break;
					case GameEvents.LoseLife:
						Player.LoseLife(arguments);
						break;
					case GameEvents.OpponentLoseLife:
						Opponent.LoseLife(arguments);
						break;
					case GameEvents.CardDestroyed:
						Player.DestroyUnit(arguments);
						break;
					case GameEvents.OpponentCardDestroyed:
						Opponent.DestroyUnit(arguments);
						break;
					case GameEvents.AttackedUnit:
						Player.AttackUnit(arguments);
						break;
					case GameEvents.OpponentAttackedUnit:
						Opponent.AttackUnit(arguments);
						break;
					case GameEvents.ReadyCard:
						Player.ReadyCards(arguments);
						break;
					case GameEvents.UnreadyCard:
						Player.UnreadyCards(arguments);
						break;
					case GameEvents.AttackedDirectly:
						Player.attack_directly(arguments);
						break;
					case GameEvents.OpponentAttackedDirectly:
						Opponent.AttackDirectly(arguments);
						break;
					case GameEvents.OpponentActivate:
						Opponent.Activate(arguments);
						break;
					case GameEvents.Resolve:
						Player.Resolve();
						Opponent.Resolve();
						Player.Link.Clear();
						break;
					case GameEvents.SetTargets:
						Player.SetTargets(arguments);
						break;
					case GameEvents.Bounce:
						Player.bounce(arguments);
						break;
					case GameEvents.OpponentBounce:
						Opponent.Bounce(arguments);
						break;
					case GameEvents.AttackDeclared:
						Opponent.ShowAttack(arguments);
						break;
					case GameEvents.AutoTarget:
						Player.autotarget(arguments);
						break;
					case GameEvents.SetState:
						Player.SetState(arguments);
						break;
					case GameEvents.SetDeployable:
						Player.SetDeployable(arguments);
						break;
					case GameEvents.SetAsAttacker:
						Player.SetAttacker(arguments);
						break;
					case GameEvents.NoOp:
						break;
					case GameEvents.SetSettable:
						Player.SetSettable(arguments);
						break;
					case GameEvents.SetActivatable:
						Player.SetActivatable(arguments);
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

        
        
    }
}


