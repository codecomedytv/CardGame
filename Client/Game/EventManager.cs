using System;
using System.Collections;
using System.Collections.Generic;
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
        
        public List<Godot.Collections.Dictionary<object, object>> Events = new List<Godot.Collections.Dictionary<object, object>>();

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
            var e = new Godot.Collections.Dictionary<object, object>
            {
                {"command", command}, {"args", args}
            };
            Events.Add(e);
        } 
        
        public void Execute()
		{
			foreach(var command in Events)
			{
				var args = command["args"] as Godot.Collections.Array;
				switch((GameEvents)command["command"])
				{
					case GameEvents.Draw:
						Player.Draw(command["args"] as Array);
						break;
					case GameEvents.LoadDeck:
						var decksize = (int)args[0];
						Player.LoadDeck(decksize);
						break;
					case GameEvents.OpponentDraw:
						Opponent.Draw(command["args"] as Array);
						break;
					case GameEvents.OpponentLoadDeck:
						var Odecksize = (int)args[0];
						Opponent.LoadDeck(Odecksize);
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
						Player.Deploy(command["args"] as Array);
						break;
					case GameEvents.OpponentDeploy:
						Opponent.Deploy(command["args"] as Array);
						break;
					case GameEvents.SetFaceDown:
						Player.SetFaceDown(command["args"] as Array);
						break;
					case GameEvents.OpponentSetFaceDown:
						Opponent.SetFaceDown(command["args"] as Array);
						break;
					case GameEvents.LoseLife:
						Player.LoseLife(new Array<int>(args));
						break;
					case GameEvents.OpponentLoseLife:
						Opponent.LoseLife(new Array<int>(args));
						break;
					case GameEvents.CardDestroyed:
						Player.DestroyUnit(command["args"] as Array);
						break;
					case GameEvents.OpponentCardDestroyed:
						Opponent.DestroyUnit(command["args"] as Array);
						break;
					case GameEvents.AttackedUnit:
						Player.AttackUnit(command["args"] as Array);
						break;
					case GameEvents.OpponentAttackedUnit:
						Opponent.AttackUnit(command["args"] as Array);
						break;
					case GameEvents.ReadyCard:
						Player.ReadyCards(new Array<int>(args));
						break;
					case GameEvents.UnreadyCard:
						Player.UnreadyCards(new Array<int>(args));
						break;
					case GameEvents.AttackedDirectly:
						Player.attack_directly(command["args"] as Array);
						break;
					case GameEvents.OpponentAttackedDirectly:
						Opponent.AttackDirectly(command["args"] as Array);
						break;
					case GameEvents.OpponentActivate:
						Opponent.Activate(command["args"] as Array);
						break;
					case GameEvents.Resolve:
						Player.Resolve();
						Opponent.Resolve();
						Player.Link.Clear();
						break;
					case GameEvents.SetTargets:
						Player.SetTargets(command["args"] as Array);
						break;
					case GameEvents.Bounce:
						Player.bounce(command["args"] as Array);
						break;
					case GameEvents.OpponentBounce:
						Opponent.Bounce(command["args"] as Array);
						break;
					case GameEvents.AttackDeclared:
						Opponent.ShowAttack(command["args"] as Array);
						break;
					case GameEvents.AutoTarget:
						Player.autotarget(command["args"] as Array);
						break;
					case GameEvents.SetState:
						Player.SetState(command["args"] as Array);
						break;
					case GameEvents.SetDeployable:
						Player.SetDeployable(command["args"] as Array);
						break;
					case GameEvents.SetAsAttacker:
						Player.SetAttacker(command["args"] as Array);
						break;
					case GameEvents.NoOp:
						break;
					case GameEvents.SetSettable:
						Player.SetSettable(command["args"] as Array);
						break;
					case GameEvents.SetActivatable:
						Player.SetActivatable(command["args"] as Array);
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


