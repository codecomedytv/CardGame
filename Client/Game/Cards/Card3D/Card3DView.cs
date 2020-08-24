using Godot;

namespace CardGame.Client.Game.Cards.Card3D
{
    public class Card3DView: Spatial, ICardView
    {
        public Vector3 Position
        {
            get => Translation;
            set => Translation = value;
        }

        public new bool Visible
        {
            get => base.Visible;
            set => base.Visible = value;
        }
        
        public Card3DView(int id, CardInfo cardInfo)
        {
            var scene = (PackedScene) GD.Load("res://Client/Game/Cards/Card3D/Card3DView.tscn");
            var instance = scene.Instance() as Spatial;
            AddChild(instance);
        }
        
        
        public void DisplayPower(int power)
        {
            throw new System.NotImplementedException();
        }

        public void FlipFaceUp()
        {
            throw new System.NotImplementedException();
        }

        public void FlipFaceDown()
        {
            throw new System.NotImplementedException();
        }
    }
}