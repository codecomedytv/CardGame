using System.Collections.Generic;
using CardGame.Server.Room.Cards;
using CardGame.Server.Room.Network.Messages;
using CardGame.Server.States;
using Godot;
using Serialized = System.Collections.Generic.Dictionary<object, object>;

namespace CardGame.Server
{

    public interface ICommand
    {
        void Execute();
        void Undo();
    }
    
    public abstract class GameEvent: Reference
    {
        public Message Message = new Message();

        protected GameEvent()
        {
            
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
            Message = new Room.Network.Messages.LoadDeck(cardsLoaded.Count);
        }
    }

}