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

    public class Bounce : GameEvent, ICommand
    {
        public readonly ISource Source;
        public readonly Player Player;
        public readonly Card Card;
        public readonly List<Card> PreviousZone;
        

        public Bounce(ISource source, Player player, Card card)
        {
            Source = source;
            Player = player;
            Card = card;
            PreviousZone = card.Zone;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.Bounce;
            message.Player["args"] = new Array {Card.Id};
            message.Opponent["command"] = GameEvents.OpponentBounce;
            message.Opponent["args"] = new Array {Card.Id};
            return message;
        }

        public void Execute()
        {
            Player.Move(PreviousZone, Card, Card.Owner.Hand);
        }

        public void Undo()
        {
            Player.Move(Card.Owner.Hand, Card, PreviousZone);
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
    
    public class Mill: GameEvent, ICommand
    {
        public readonly ISource Source;
        public readonly Player Player;
        public readonly Card Card;

        public Mill(ISource source, Player player, Card card)
        {
            Source = source;
            Player = player;
            Card = card;
        }


        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.Mill;
            message.Player["args"] = new Array{Card.Id}; // Might need to be serialized
            message.Opponent["command"] = GameEvents.OpponentMill;
            message.Opponent["args"] = new Array{Card.Serialize()};
            return message;
        }

        public void Execute()
        {
            Player.Move(Card.Owner.Deck, Card, Card.Owner.Graveyard);
        }

        public void Undo()
        {
            Player.Move(Card.Owner.Graveyard, Card, Card.Owner.Deck);
        }
    }

    public class Deploy : GameEvent, ICommand
    {
        public readonly ISource Source;
        public readonly Player Player;
        public readonly Card Card;
        public readonly List<Card> PreviousZone;

        public Deploy(ISource source, Player player, Card card)
        {
            Source = source;
            Player = player;
            Card = card;
            PreviousZone = Card.Zone;
        }

        public void Execute()
        {
            Player.Move(PreviousZone, Card, Player.Field);
        }

        public void Undo()
        {
            Player.Move(Player.Field, Card, PreviousZone);
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.Deploy;
            message.Player["args"] = new Array {Card.Id};
            message.Opponent["command"] = GameEvents.OpponentDeploy;
            message.Opponent["args"] = new Array{Card.Serialize()};
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

    public class Draw : GameEvent, ICommand
    {
        public readonly ISource Source;
        public readonly Player Player;
        public readonly Card Card;

        public Draw(ISource source, Player player, Card card)
        {
            Source = source;
            Player = player;
            Card = card;
        }

        public void Execute()
        {
            Player.Move(Player.Deck, Card, Player.Hand);
        }

        public void Undo()
        {
            Player.Move(Player.Hand, Card, Player.Deck);
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.Draw;
            message.Player["args"] = new Array {Card.Serialize()};
            message.Opponent["command"] = GameEvents.OpponentDraw;
            message.Opponent["args"] = new Array {1};
            return message;
        }
       
    }

    public class  Discard : GameEvent, ICommand
    {
        public readonly ISource Source;
        public readonly Player Player;
        public readonly Card Card;

        public Discard(ISource source, Player player, Card card)
        {
            Source = source;
            Player = player;
            Card = card;
        }

        public void Execute()
        {
            Player.Move(Player.Hand, Card, Player.Graveyard);
        }

        public void Undo()
        {
            Player.Move(Player.Graveyard, Card, Player.Hand);
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.Discard;
            message.Player["args"] = new Array{Card.Id};
            message.Opponent["command"] = GameEvents.OpponentDiscard;
            message.Opponent["args"] = new Array{Card.Serialize()};
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

    public class ReadyCard : GameEvent, ICommand
    {
        public readonly Card Card;

        public ReadyCard(Card card)
        {
            Card = card;
        }

        public void Execute()
        {
            Card.Ready = true;
        }

        public void Undo()
        {
            Card.Ready = false;
        }


        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.ReadyCard;
            message.Player["args"] = new Array {Card.Id};
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

    public class UnreadyCard : GameEvent
    {
        public readonly Card Card;

        public UnreadyCard(Card card)
        {
            Card = card;
        }
        
        public void Execute()
        {
            Card.Ready = false;
        }

        public void Undo()
        {
            Card.Ready = true;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.UnreadyCard;
            message.Player["args"] = new Array{Card.Id};
            message.Opponent["command"] = GameEvents.NoOp;
            message.Opponent["args"] = new Array();
            return message;
        }
    }

    public class ReturnToDeck : GameEvent, ICommand
    {
        public readonly ISource Source;
        public readonly Player Player;
        public readonly List<Card> PreviousZone;
        public readonly Card Card;

        public ReturnToDeck(ISource source, Player player, Card card)
        {
            Source = source;
            Player = player;
            Card = card;
            PreviousZone = card.Zone;
        }

        public void Execute()
        {
            Player.Move(PreviousZone, Card, Card.Owner.Deck);
        }

        public void Undo()
        {
            Player.Move(Card.Owner.Deck, Card, PreviousZone);
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.ReturnToDeck;
            message.Player["args"] = new Array{Card.Id};
            message.Opponent["command"] = GameEvents.OpponentReturnedToDeck;
            message.Opponent["args"] = new Array(Card.Serialize());
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