using System;
using System.Collections.Generic;
using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game.Players
{
    public interface IPlayer
    {
        void DisplayName(string name);
        void DisplayHealth(int health);
        void AddCardToDeck(Card cardView);
        void LoadDeck(IEnumerable<Card> deck);
        Command Draw(Card card);
        Command Discard(Card card);
        Command Deploy(Card card);
        Command SetFaceDown(Card card);
        Command Activate(Card card);
        Command SendCardToGraveyard(Card card);
        Command Attack(Card attacker, Card defender);
        Command AttackDirectly(Card attacker);
        Command Battle(Card attacker, Card defender);
        Command LoseLife(int lifeLost);

        void ClearDirectAttackingDefense();
    }
}