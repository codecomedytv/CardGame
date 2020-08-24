using Godot;

namespace CardGame.Client.Game.Cards
{
    public interface ICardView
    {
        
        Node View { get; }
        Vector3 Position { get; set; }
        Vector3 Scale { get; set; }
        Vector3 Rotation { get; set; }
        bool Visible { get; set; }
        void DisplayPower(int power);
        void FlipFaceUp();
        void FlipFaceDown();
        
    }
}