using System.Collections.Generic;
using CardGame;
using CardGame.Client.Match;
using Godot;
using Godot.Collections;

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
        
        //func _on_animation_finished() -> void:
        //emit_signal("COMMAND_REQUESTED")
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
			GD.Print("Executing Events");
			foreach(var command in Events)
			{
				var args = command["args"] as Godot.Collections.Array;
				switch((GameEvents)command["command"])
				{
					case GameEvents.Draw:
						Player.Draw(args);
						break;
					case GameEvents.LoadDeck:
						Player.LoadDeck(args);
						break;
					case GameEvents.OpponentDraw:
						Opponent.Draw(args);
						break;
					case GameEvents.OpponentLoadDeck:
						Opponent.LoadDeck(args);
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
						Player.Deploy(args);
						break;
					case GameEvents.OpponentDeploy:
						Opponent.Deploy(args);
						break;
					case GameEvents.SetFaceDown:
						Player.SetFaceDown(args);
						break;
					case GameEvents.OpponentSetFaceDown:
						Opponent.SetFaceDown(args);
						break;
					case GameEvents.LoseLife:
						Player.LoseLife(new Array<int>(args));
						break;
					case GameEvents.OpponentLoseLife:
						Opponent.LoseLife(new Array<int>(args));
						break;
					case GameEvents.CardDestroyed:
						Player.destroy_unit(args);
						break;
					case GameEvents.OpponentCardDestroyed:
						Opponent.DestroyUnit(args);
						break;
					case GameEvents.AttackedUnit:
						Player.AttackUnit(args);
						break;
					case GameEvents.OpponentAttackedUnit:
						Opponent.AttackUnit(args);
						break;
					case GameEvents.ReadyCard:
						Player.ReadyCards(new Array<int>(args));
						break;
					case GameEvents.UnreadyCard:
						Player.UnreadyCards(new Array<int>(args));
						break;
					case GameEvents.Legalize:
						Player.Legalize(args);
						break;
					case GameEvents.Forbid:
						Player.Forbid(args);
						break;
					case GameEvents.AttackedDirectly:
						Player.attack_directly(args);
						break;
					case GameEvents.OpponentAttackedDirectly:
						Opponent.AttackDirectly(args);
						break;
					case GameEvents.OpponentActivate:
						Opponent.Activate(args);
						break;
					case GameEvents.Resolve:
						Player.Resolve();
						Opponent.Resolve();
						Player.Link.Clear();
						break;
					case GameEvents.SetTargets:
						Player.SetTargets(args);
						break;
					case GameEvents.Bounce:
						Player.bounce(args);
						break;
					case GameEvents.OpponentBounce:
						Opponent.Bounce(args);
						break;
					case GameEvents.AttackDeclared:
						Opponent.ShowAttack(args);
						break;
					case GameEvents.AutoTarget:
						Player.autotarget(args);
						break;
					case GameEvents.SetState:
						Player.SetState(args);
						break;
					case GameEvents.SetDeployable:
						Player.SetDeployable(args);
						break;
					case GameEvents.NoOp:
						break;
					case GameEvents.SetSettable:
						Player.SetSettable(args);
						break;
					case GameEvents.SetActivatable:
						Player.set_activatable(args);
						break;
				}
			}
			EmitSignal(nameof(Animated));
			Events.Clear();
		}
    }
}
