using System.Collections.Generic;
using CardGame.Client.Game.Cards;

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
        void Connect(Declaration addCommand);
        void DisplayName(string name);
        void DisplayHealth(int health);
        void AddCardToDeck(Card cardView);
        public void LoadDeck(IEnumerable<Card> deck);
        void Draw(Card card);
        void Discard(Card card);
        void Deploy(Card card);
        void SetFaceDown(Card card);
        void Attack(Card attacker, Card defender);
        void AttackDirectly(Card attacker);
    }
}