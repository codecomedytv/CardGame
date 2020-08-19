using CardGame.Client.Game.Cards;

namespace CardGame.Client.Game.Players
{
    public interface IPlayer 
    {
        // There are some minor differences between player and opponent in certain actions
        // so an interface is likely more suitable than a class
        
        void Draw(Card card);
        void Discard(Card card);
        void Deploy(Card card);
        void Set(Card card);
    }
}