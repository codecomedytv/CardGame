using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CardGame.Server.States;
using Godot;
using Godot.Collections;
using Array = Godot.Collections.Array;
using Serialized = System.Collections.Generic.Dictionary<object, object>;

namespace CardGame.Server
{
    public class Message
    {
        public Dictionary Player = new Dictionary {{"command", null}, {"args", null}};
        public Dictionary Opponent = new Dictionary {{"command", null}, {"args", null}};
    }

    public interface ICommand
    {
        void Execute();
        void Undo();
    }
    
    public abstract class GameEvent: Reference
    {
        public virtual Serialized Serialize()
        {
            return new Serialized();
        }

        public virtual Message GetMessage()
        {
            var message = new Message();
            return message;
        }
    }

    public class ShowAttack : GameEvent
    {
        private Unit Attacker;
        private Unit Defender;

        public ShowAttack(Unit attacker, Unit defender)
        {
            Attacker = attacker;
            Defender = defender;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.AttackDeclared;
            message.Player["args"] = new Array{Attacker.Id, Defender.Id};
            message.Opponent["command"] = GameEvents.NoOp;
            return message;
        }
    }

    public class SetState : GameEvent
    {
        private Player Player;
        private State State;

        public SetState(Player player, State state)
        {
            Player = player;
            State = state;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.SetState;
            message.Player["args"] = new Array{State.ToString()};
            message.Opponent["command"] = GameEvents.NoOp;
            message.Opponent["args"] = new Array();
            return message;
        }
    }

    public class Activate : GameEvent
    {
        private Card Activated;
        private List<Card> Targets;

        public Activate(Card activated, List<Card> targets)
        {
            Activated = activated;
            Targets = targets;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.NoOp;
            message.Player["args"] = new Array{Activated.Id};
            message.Opponent["command"] = GameEvents.OpponentActivate;
            message.Opponent["args"] = new Array{Activated.Serialize(), Targets};
            return message;
        }
    }

    public class AttackUnit : GameEvent
    {
        private Unit Attacker;
        private Unit Defender;

        public AttackUnit(Unit attacker, Unit defender)
        {
            Attacker = attacker;
            Defender = defender;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.AttackedUnit;
            message.Player["args"] = new Array {Attacker.Id, Defender.Id};
            message.Opponent["command"] = GameEvents.OpponentAttackedUnit;
            message.Opponent["args"] = new Array{Attacker.Id, Defender.Id};
            return message;
        }
    }

    public class AttackDirectly : GameEvent
    {
        private Unit Attacker;

        public AttackDirectly(Unit attacker)
        {
            Attacker = attacker;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.AttackedDirectly;
            message.Player["args"] = new Array {Attacker.Id};
            message.Opponent["command"] = GameEvents.OpponentAttackedDirectly;
            message.Opponent["args"] = new Array{Attacker.Id};
            return message;
        }
    }

    

    public class BeginTurn : GameEvent
    {
        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.BeginTurn;
            message.Player["args"] = new Array();
            message.Opponent["command"] = GameEvents.NoOp;
            message.Opponent["args"] = new Array();
            return message;
        }
    }
    
    

    

    public class DestroyUnits : GameEvent
    {
        public readonly Card Card;

        public DestroyUnits(Card card)
        {
            Card = card;
        }

        public override Message GetMessage()
        {
            var destroyed = new Array {Card.Id};
            var message = new Message();
            message.Player["command"] = GameEvents.CardDestroyed;
            message.Player["args"] = destroyed;
            message.Opponent["command"] = GameEvents.OpponentCardDestroyed;
            message.Opponent["args"] = destroyed;
            return message;
        }
    }

    

    

    public class EndTurn : GameEvent
    {
        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.EndTurn;
            message.Player["args"] = new Array();
            message.Opponent["command"] = GameEvents.NoOp;
            message.Opponent["args"] = new Array();
            return message;
        }
    }

    public class GameOver : GameEvent
    {
        public readonly Player Winner;
        public readonly Player Loser;

        public GameOver(Player winner, Player loser)
        {
            Winner = winner;
            Loser = loser;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.Win;
            message.Player["args"] = new Array();
            message.Opponent["command"] = GameEvents.Lose;
            message.Opponent["args"] = new Array();
            return message;
        }
    }

    public class LoadDeck : GameEvent
    {
        private List<Card> CardsLoaded;

        public LoadDeck(List<Card> cardsLoaded)
        {
            CardsLoaded = cardsLoaded;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.LoadDeck;
            message.Player["args"] = new Array {CardsLoaded.Count};
            message.Opponent["command"] = GameEvents.OpponentLoadDeck;
            message.Opponent["args"] = new Array {CardsLoaded.Count};
            return message;
        }
    }

    public class LoseLife : GameEvent
    {
        private int LifeLost;

        public LoseLife(int lifeLost)
        {
            LifeLost = lifeLost;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.LoseLife;
            message.Player["args"] = new Array{LifeLost};
            message.Opponent["command"] = GameEvents.NoOp;
            message.Opponent["args"] = new Array();
            return message;
        }
    }

    

    public class SetSupport : GameEvent
    {
        public readonly Card Card;

        public SetSupport(Card card)
        {
            Card = card;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.SetFaceDown;
            message.Player["args"] = new Array{Card.Id};
            message.Opponent["command"] = GameEvents.OpponentSetFaceDown;
            message.Opponent["args"] = new Array();
            return message;
        }
    }

    
    
    
    public class SetAsDeployable : GameEvent
    {
        public readonly Card Card;

        public SetAsDeployable(Card card)
        {
            Card = card;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.SetDeployable;
            message.Player["args"] = new Array {Card.Id};
            message.Opponent["command"] = GameEvents.NoOp;
            message.Opponent["args"] = new Array();
            return message;
        }
    }

    public class SetAsSettable : GameEvent
    {
        public readonly Card Card;

        public SetAsSettable(Card card)
        {
            Card = card;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.SetSettable;
            message.Player["args"] = new Array{Card.Id};
            message.Opponent["command"] = GameEvents.NoOp;
            message.Opponent["args"] = new Array();
            return message;
        }
    }

    public class SetAsActivatable : GameEvent
    {
        public readonly Card Card;

        public SetAsActivatable(Card card)
        {
            Card = card;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.SetActivatable;
            message.Player["args"] = new Array {Card.Id};
            message.Opponent["command"] = GameEvents.NoOp;
            message.Opponent["args"] = new Array();
            return message;
        }
    }

    public class SetAsAttacker : GameEvent
    {
        public readonly Card Card;

        public SetAsAttacker(Card card)
        {
            Card = card;
        }
        
        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.SetAsAttacker;
            message.Player["args"] = new Array {Card.Id};
            message.Opponent["command"] = GameEvents.NoOp;
            message.Opponent["args"] = new Array();
            return message;
        }
    }

    public class Resolve : GameEvent
    {
        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.Resolve;
            message.Player["args"] = new Array();
            message.Opponent["command"] = GameEvents.Resolve;
            message.Opponent["args"] = new Array();
            return message;
        }
    }

    public class SetTargets : GameEvent
    {
        public readonly Card Selector;
        public readonly List<Card> Targets;

        public SetTargets(Card selector, List<Card> targets)
        {
            Selector = selector;
            Targets = targets;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            var targets = new Array();
            foreach (var u in Targets)
            {
                targets.Add(u.Id);
            }
            message.Player["command"] = GameEvents.SetTargets;
            message.Player["args"] = new Array{Selector.Id, targets};
            message.Opponent["command"] = GameEvents.NoOp;
            message.Opponent["args"] = new Array();
            return message;
        }
    }

    public class AutoTarget : GameEvent
    {
        private Card Selector;

        public AutoTarget(Card selector)
        {
            Selector = selector;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.AutoTarget;
            message.Player["args"] = new Array{Selector.Id};
            message.Opponent["command"] = GameEvents.NoOp;
            message.Opponent["args"] = new Array();
            return message;
        }
    }


}