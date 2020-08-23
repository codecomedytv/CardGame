using CardGame.Client.Game.Cards;

namespace CardGame.Client.Game.Players
{
    public interface IPlayerModel
    {
        public void Draw(CardModel card);
    }
}