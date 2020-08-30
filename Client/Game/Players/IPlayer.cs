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
        
        void AddCardToDeck(Card cardView);
        void LoadDeck(IEnumerable<Card> deck);
        Command SetFaceDown(Card card);
        Command Activate(Card card);
        Command SendCardToGraveyard(Card card);
        Command Attack(Card attacker, Card defender);
        Command AttackDirectly(Card attacker);
        Command Battle(Card attacker, Card defender);
        Command LoseLife(int lifeLost);

    }
}