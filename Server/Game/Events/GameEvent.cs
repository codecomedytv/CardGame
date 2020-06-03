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
            GD.Print(State.ToString());
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

    public class Bounce : GameEvent
    {
        private Card Bounced;

        public Bounce(Card bounced)
        {
            Bounced = bounced;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.Bounce;
            message.Player["args"] = new Array {Bounced.Id};
            message.Opponent["command"] = GameEvents.OpponentBounce;
            message.Opponent["args"] = new Array {Bounced.Id};
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
    
    public class Mill: GameEvent
    {
        private Card Milled;

        public Mill(Card milled)
        {
            Milled = milled;
        }


        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.Mill;
            message.Player["args"] = new Array{Milled.Id}; // Might need to be serialized
            message.Opponent["command"] = GameEvents.OpponentMill;
            message.Opponent["args"] = new Array{Milled.Serialize()};
            return message;
        }
    }

    public class Deploy : GameEvent
    {
        private Unit Deployed;

        public Deploy(Unit deployed)
        {
            Deployed = deployed;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.Deploy;
            message.Player["args"] = new Array {Deployed.Id};
            message.Opponent["command"] = GameEvents.OpponentDeploy;
            message.Opponent["args"] = new Array{Deployed.Serialize()};
            return message;
        }
    }

    public class DestroyUnits : GameEvent
    {
        private List<Unit> DestroyedUnits;

        public DestroyUnits(List<Unit> destroyedUnits)
        {
            DestroyedUnits = destroyedUnits;
        }

        public override Message GetMessage()
        {
            var destroyed = new Array();
            foreach (var unit in DestroyedUnits)
            {
                destroyed.Add(unit.Id);
            }
            var message = new Message();
            message.Player["command"] = GameEvents.CardDestroyed;
            message.Player["args"] = destroyed;
            message.Opponent["command"] = GameEvents.OpponentCardDestroyed;
            message.Opponent["args"] = destroyed;
            return message;
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
       
    }

    public class  Discard : GameEvent
    {
        private Card Discarded;

        public Discard(Card discarded)
        {
            Discarded = discarded;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.Discard;
            message.Player["args"] = new Array{Discarded.Id};
            message.Opponent["command"] = GameEvents.OpponentDiscard;
            message.Opponent["args"] = new Array{Discarded.Serialize()};
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
        private Player Winner;
        private Player Loser;

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

    public class ReadyCard : GameEvent
    {
        private Card ReadiedCard;

        public ReadyCard(Card readiedCard)
        {
            ReadiedCard = readiedCard;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.ReadyCard;
            message.Player["args"] = new Array {ReadiedCard.Id};
            message.Opponent["command"] = GameEvents.NoOp;
            message.Opponent["args"] = new Array();
            return message;
        }
    }

    public class SetSupport : GameEvent
    {
        private Card Facedown;

        public SetSupport(Card faceDown)
        {
            Facedown = faceDown;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.SetFaceDown;
            message.Player["args"] = new Array{Facedown.Id};
            message.Opponent["command"] = GameEvents.OpponentSetFaceDown;
            message.Opponent["args"] = new Array();
            return message;
        }
    }

    public class UnreadyCard : GameEvent
    {
        private Card UnreadiedCard;

        public UnreadyCard(Card unreadiedCard)
        {
            UnreadiedCard = unreadiedCard;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.UnreadyCard;
            message.Player["args"] = new Array{UnreadiedCard.Id};
            message.Opponent["command"] = GameEvents.NoOp;
            message.Opponent["args"] = new Array();
            return message;
        }
    }

    public class ReturnedToDeck : GameEvent
    {
        private Card Returned;

        public ReturnedToDeck(Card returned)
        {
            Returned = returned;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.ReturnToDeck;
            message.Player["args"] = new Array{Returned.Id};
            message.Opponent["command"] = GameEvents.OpponentReturnedToDeck;
            message.Opponent["args"] = new Array(Returned.Serialize());
            return message;
        }
    }

    public class Legalize : GameEvent
    {
        private Card Legalized;

        public Legalize(Card legalized)
        {
            Legalized = legalized;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.Legalize;
            message.Player["args"] = new Array{Legalized.Id};
            message.Opponent["command"] = GameEvents.NoOp;
            message.Opponent["args"] = new Array();
            return message;
        }
    }

    public class Forbid : GameEvent
    {
        private Card Forbidden;

        public Forbid(Card forbidden)
        {
            Forbidden = forbidden;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.Forbid;
            message.Player["args"] = new Array {Forbidden.Id};
            message.Opponent["command"] = GameEvents.NoOp;
            message.Opponent["args"] = new Array();
            return message;
        }
    }

    public class SetAsDeployable : GameEvent
    {
        private Card Deployable;

        public SetAsDeployable(Card deployable)
        {
            Deployable = deployable;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.SetDeployable;
            message.Player["args"] = new Array {Deployable.Id};
            message.Opponent["command"] = GameEvents.NoOp;
            message.Opponent["args"] = new Array();
            return message;
        }
    }

    public class SetAsSettable : GameEvent
    {
        private Card Support;

        public SetAsSettable(Card support)
        {
            Support = support;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.SetSettable;
            message.Player["args"] = new Array{Support.Id};
            message.Opponent["command"] = GameEvents.NoOp;
            message.Opponent["args"] = new Array();
            return message;
        }
    }

    public class SetAsActivatable : GameEvent
    {
        private Card Support;

        public SetAsActivatable(Card support)
        {
            Support = support;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.SetActivatable;
            message.Player["args"] = new Array {Support.Id};
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
        private Card Selector;
        private List<Card> Targets;

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