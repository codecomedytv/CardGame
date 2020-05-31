using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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

        public override Serialized Serialize()
        {
            throw new System.NotImplementedException();
        }
        
    }

    public class SetState : GameEvent
    {
        private Player Player;
        private Player.States State;

        public SetState(Player player, Player.States state)
        {
            Player = player;
            State = state;
        }

        public override Serialized Serialize()
        {
            throw new System.NotImplementedException();
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

        public override Serialized Serialize()
        {
            throw new System.NotImplementedException();
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

        public override Serialized Serialize()
        {
            throw new System.NotImplementedException();
        }
    }

    public class AttackDirectly : GameEvent
    {
        private Unit Attacker;

        public AttackDirectly(Unit attacker)
        {
            Attacker = attacker;
        }

        public override Serialized Serialize()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Bounce : GameEvent
    {
        private Card Bounced;

        public Bounce(Card bounced)
        {
            Bounced = bounced;
        }

        public override Serialized Serialize()
        {
            throw new System.NotImplementedException();
        }
    }

    public class BeginTurn : GameEvent
    {
        public override Serialized Serialize()
        {
            throw new System.NotImplementedException();
        }
    }
    
    public class Mill: GameEvent
    {
        private Card Milled;

        public Mill(Card milled)
        {
            Milled = milled;
        }


        public override Serialized Serialize()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Deploy : GameEvent
    {
        private Unit Deployed;

        public Deploy(Unit deployed)
        {
            Deployed = deployed;
        }

        public override Serialized Serialize()
        {
            throw new System.NotImplementedException();
        }
    }

    public class DestroyUnits : GameEvent
    {
        private List<Unit> DestroyedUnits;

        public DestroyUnits(List<Unit> destroyedUnits)
        {
            DestroyedUnits = destroyedUnits;
        }

        public override Serialized Serialize()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Draw : GameEvent
    {
        private List<Card> DrawnCards;

        public Draw(List<Card> drawnCards)
        {
            DrawnCards = drawnCards;
        }

       public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.Draw;
            var data = new Array();
            foreach (var card in DrawnCards)
            {
                data.Add(card.Serialize());
            }
            message.Player["args"] = data;
            message.Opponent["command"] = GameEvents.OpponentDraw;
            message.Opponent["args"] = new Array {DrawnCards.Count};
            return message;
        }

       public class Instruction
        {
            public GameEvents GameEvent = GameEvents.Draw;
            public Array args;
        }

        public override Serialized Serialize()
        {
            throw new NotImplementedException();
        }
    }

    public class  Discard : GameEvent
    {
        private Card Discarded;

        public Discard(Card discarded)
        {
            Discarded = discarded;
        }

        public override Serialized Serialize()
        {
            throw new System.NotImplementedException();
        }
    }

    public class EndTurn : GameEvent
    {
        public override Serialized Serialize()
        {
            throw new System.NotImplementedException();
        }
    }

    public class GameOver : GameEvent
    {
        private Player Winner;
        private Player Loser;

        public GameOver(Player winner, Player loser)
        {
            Winner = winner;
            Loser = loser;
        }

        public override Serialized Serialize()
        {
            throw new System.NotImplementedException();
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

        public override Serialized Serialize()
        {
            throw new System.NotImplementedException();
        }
    }

    public class ReadyCard : GameEvent
    {
        private Card ReadiedCard;

        public ReadyCard(Card readiedCard)
        {
            ReadiedCard = readiedCard;
        }

        public override Serialized Serialize()
        {
            throw new System.NotImplementedException();
        }
    }

    public class SetSupport : GameEvent
    {
        private Card Facedown;

        public SetSupport(Card faceDown)
        {
            Facedown = faceDown;
        }

        public override Serialized Serialize()
        {
            throw new System.NotImplementedException();
        }
    }

    public class UnreadyCard : GameEvent
    {
        private Card UnreadiedCard;

        public UnreadyCard(Card unreadiedCard)
        {
            UnreadiedCard = unreadiedCard;
        }

        public override Serialized Serialize()
        {
            throw new System.NotImplementedException();
        }
    }

    public class ReturnedToDeck : GameEvent
    {
        private Card Returned;

        public ReturnedToDeck(Card returned)
        {
            Returned = returned;
        }

        public override Serialized Serialize()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Legalize : GameEvent
    {
        private Card Legalized;

        public Legalize(Card legalized)
        {
            Legalized = legalized;
        }

        public override Serialized Serialize()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Forbid : GameEvent
    {
        private Card Forbidden;

        public Forbid(Card forbidden)
        {
            Forbidden = forbidden;
        }

        public override Serialized Serialize()
        {
            throw new System.NotImplementedException();
        }
    }

    public class SetAsDeployable : GameEvent
    {
        private Card Deployable;

        public SetAsDeployable(Card deployable)
        {
            Deployable = deployable;
        }

        public override Serialized Serialize()
        {
            throw new System.NotImplementedException();
        }
    }

    public class SetAsSettable : GameEvent
    {
        private Card Support;

        public SetAsSettable(Card support)
        {
            Support = support;
        }

        public override Serialized Serialize()
        {
            throw new System.NotImplementedException();
        }
    }

    public class SetAsActivatable : GameEvent
    {
        private Card Support;

        public SetAsActivatable(Card support)
        {
            Support = support;
        }

        public override Serialized Serialize()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Resolve : GameEvent
    {
        public override Serialized Serialize()
        {
            throw new System.NotImplementedException();
        }
    }

    public class SetTargets : GameEvent
    {
        private Card Selector;
        private List<Card> Targets;

        public SetTargets(Card selector, List<Card> targets)
        {
            Selector = selector;
            Targets = targets;
        }

        public override Serialized Serialize()
        {
            throw new System.NotImplementedException();
        }
    }

    public class AutoTarget : GameEvent
    {
        private Card Selector;

        public AutoTarget(Card selector)
        {
            Selector = selector;
        }

        public override Serialized Serialize()
        {
            throw new System.NotImplementedException();
        }
    }


}