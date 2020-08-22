using System;
using CardGame.Client.Game.Cards.Card3D;
using CardGame.Client.Library;
using Godot;

namespace CardGame.Client.Game.Cards
{
    public class CardFactory
    {
        private readonly Type View;

        public CardFactory()
        {
            View = typeof(Card3DView);
        }
        
        public Card Create(int id, SetCodes setCode)
        {
            var cardInfo = CardLibrary.Cards[setCode];
            var model = new CardModel(id, cardInfo);
            var view = (ICardView) Activator.CreateInstance(View, id, cardInfo);
            return new Card(model, view);
        }
    }
}