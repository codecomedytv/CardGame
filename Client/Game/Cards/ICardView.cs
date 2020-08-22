namespace CardGame.Client.Game.Cards
{
    public interface ICardView
    {
        void DisplayPower(int power);
        void FlipFaceUp();
        void FlipFaceDown();
        
    }
}