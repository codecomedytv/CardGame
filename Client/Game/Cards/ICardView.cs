using Godot;

namespace CardGame.Client.Game.Cards
{
    public interface ICardView
    {
       // Vector2 Position();

       // Vector3 Position();
        void DisplayPower(int power);
        void FlipFaceUp();
        void FlipFaceDown();
        
    }
}