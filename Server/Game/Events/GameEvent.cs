﻿using System;
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