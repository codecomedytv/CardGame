namespace CardGame.Client.Game.Cards
{
    public class Card
    {
        private CardModel _model;
        private ICardView _view;

        public Card(CardModel cardModel, ICardView cardView)
        {
            _model = cardModel;
            _view = cardView;
        }
    }
}