namespace CardGame.Client.Game.Cards
{
    public class Card
    {
        public readonly CardModel Model;
        public readonly ICardView View;

        public Card(CardModel cardModel, ICardView cardView)
        {
            Model = cardModel;
            View = cardView;
        }
    }
}