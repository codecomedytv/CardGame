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
        Command AttackDirectly(Card attacker);
        void LoseLife(int lifeLost, Tween gfx);

        void StopDefending();

    }
}