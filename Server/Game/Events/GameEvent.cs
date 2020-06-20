using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Network.Messages;
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
        protected GameEvent()
        {
            
        }
        
        public Game.Network.Messages.Message Message = new Game.Network.Messages.Message();
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
    }

    public class SetState : GameEvent
    {
        private Player Player;
        private State State;

        public SetState(Player player, State state)
        {
            Player = player;
            State = state;
            Message = new Game.Network.Messages.SetState(state.ToString());
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
        
    }

    public class AttackDirectly : GameEvent
    {
        private Unit Attacker;

        public AttackDirectly(Unit attacker)
        {
            Attacker = attacker;
        }
        
    }
    
    public class BeginTurn : GameEvent
    {
    }
    
    public class DestroyUnits : GameEvent
    {
        public readonly Card Card;

        public DestroyUnits(Card card)
        {
            Card = card;
        }

    }
    
    public class EndTurn : GameEvent
    {

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
    }

    public class LoadDeck : GameEvent
    {
        private List<Card> CardsLoaded;

        public LoadDeck(List<Card> cardsLoaded)
        {
            CardsLoaded = cardsLoaded;
            Message = new Game.Network.Messages.LoadDeck(cardsLoaded.Count);
        }
    }

    public class Resolve : GameEvent
    {
    }

    public class SetTargets : GameEvent
    {
        // Possibly best to only add a target at a time?
        public readonly Card Selector;
        public readonly List<Card> Targets;

        public SetTargets(Card selector, List<Card> targets)
        {
            Selector = selector;
            Targets = targets;
        }

        /*public override Message GetMessage()
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
        }*/
    }

    public class AutoTarget : GameEvent
    {
        // This could be a state setter
        private Card Selector;

        public AutoTarget(Card selector)
        {
            Selector = selector;
        }
    }
}