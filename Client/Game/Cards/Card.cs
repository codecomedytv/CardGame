namespace CardGame.Client.Game.Cards
{
    public class Card
    {
        private readonly CardModel _model;
        private readonly ICardView _view;

        public Card(CardModel cardModel, ICardView cardView)
        {
            _model = cardModel;
            _view = cardView;
        }
    }
}