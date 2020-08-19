namespace CardGame.Client.Game.Cards
{
    public interface ICardView
    {
        void DisplayName(string name);
        void DisplayPower(int power);
        void FlipFaceUp();
        void FlipFaceDown();
    }
}