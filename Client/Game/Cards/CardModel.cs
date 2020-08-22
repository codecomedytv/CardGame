namespace CardGame.Client.Game.Cards
{
    public class CardModel
    {
        public int Id;
        public string Title;
        public int Power;
        public CardType CardType;
        public CardFace Face = CardFace.FaceDown;

        public CardModel(int id, CardInfo cardInfo)
        {
            Id = id;
            Title = cardInfo.Title;
            Power = cardInfo.Power;
            CardType = cardInfo.CardType;
        }
    }
}