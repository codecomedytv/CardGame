using CardGame.Client.Library;
using Godot;

namespace CardGame.Client.Game.Cards
{
    public class CardFactory
    {
        private readonly PackedScene CardScene = (PackedScene) GD.Load("res://Client/Game/Cards/Card.tscn");

        public CardFactory() { }
        
        public Card Create(int id, SetCodes setCode)
        {
            var cardInfo = CardLibrary.Cards[setCode];
            var card = (Card) CardScene.Instance();
            card.Id = id;
            card.Title = cardInfo.Title;
            card.Power = cardInfo.Power;
            card.CardType = cardInfo.CardType;
            return card;
        }
    }
}
