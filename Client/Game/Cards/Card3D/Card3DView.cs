using Godot;

namespace CardGame.Client.Game.Cards.Card3D
{
    public class Card3DView: Spatial, ICardView
    {
        public Vector3 Position
        { 
            get => GetNode<Spatial>("3DCardView").Translation;
            set => GetNode<Spatial>("3DCardView").Translation = value;
        }
        
        public Card3DView(int id, CardInfo cardInfo)
        {
            var scene = (PackedScene) GD.Load("res://Client/Game/Cards/Card3D/Card3DView.tscn");
            var instance = (Spatial) scene.Instance();
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