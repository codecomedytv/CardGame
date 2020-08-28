using System;
using System.Collections.Generic;
using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game.Players
{
    public interface IPlayer
    {
     
        // This is more of a guess than a description of the interface but we should
        // somehow use the View's to plan animations but not to execute them. Instead we could return
        // some form of action, lambda or some prepared action and push it to our command queue then
        // on update/execute etc, we await execute all of those commands
        
        // Ideally we want to queue the animation by resetting the current tween, setting it up, and then returning
        // that setup
        
        // Implemenation Leaking Here. Note for refactoring
        Sprite DefendingIcon { get; set; }
        TextureProgress LifeBar { get; set; }
        Label LifeCount { get; set; }
        Label LifeChange { get; set; }
        void DisplayName(string name);
        void DisplayHealth(int health);
        void AddCardToDeck(Card cardView);
        void LoadDeck(IEnumerable<Card> deck);
        Func<Tween> Draw(Card card);
        Func<Tween> Discard(Card card);
        Func<Tween> Deploy(Card card);
        Func<Tween> SetFaceDown(Card card);
        Func<Tween> Activate(Card card);
        Func<Tween> SendCardToGraveyard(Card card);
        Func<Tween> Attack(Card attacker, Card defender);
        Func<Tween> AttackDirectly(Card attacker);
        Func<Tween> Battle(Card attacker, Card defender);
        Func<Tween> LoseLife(int lifeLost);

        void ClearDirectAttackingDefense();
    }
}