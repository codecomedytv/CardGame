using Godot;

namespace CardGame.Client.Game.Cards
{
    public interface ICardView
    {
        Vector3 Position { get; set; }
        Vector3 Scale { get; set; }
        bool Visible { get; set; }
        void DisplayPower(int power);
        void FlipFaceUp();
        void FlipFaceDown();
        
    }
}