using System;
using System.Collections.Generic;
using CardGame.Client.Game.Cards;


namespace CardGame.Client.Game.Players
{
    public interface IPlayer 
    {
        // There are some minor differences between player and opponent in certain actions
        // so an interface is likely more suitable than a class
        
        void Connect(Declaration addCommand);
        void LoadDeck(IEnumerable<Card> deck);
        void Draw(Card card);
        void Discard(Card card);
        void Deploy(Card card);
        void Set(Card card);
    }
}