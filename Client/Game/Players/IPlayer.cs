using System;
using System.Collections.Generic;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game.Players
{
    public interface IPlayer
    {
        
        Deck Deck { get; }
        Graveyard Graveyard { get;  }
        Hand Hand { get; }
        Units Units { get; }
        Support Support { get; }
        void RegisterCard(Card card);
        void LoadDeck(IEnumerable<Card> deck);
        
        
        // Need To Refactor This?
        // Move To CommandFactory At Least
        Command Attack(Card attacker, Card defender);
        Command AttackDirectly(Card attacker);
        Command Battle(Card attacker, Card defender);
        Command LoseLife(int lifeLost);

    }
}