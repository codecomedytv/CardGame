using System;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Cards.Card3D;
using CardGame.Client.Game.Zones;
using CardGame.Client.Game.Zones.Zones3D;
using Godot;
using Array = Godot.Collections.Array;

namespace CardGame.Client.Game.Players.Player3D
{
    public class Player3DView: Spatial, IPlayerView
    {
        private Declaration Declare;
        private Spatial Units;
        private Spatial Support;
        private Hand3D Hand;
        private Spatial Graveyard;
        private Deck3D Deck;
        private Tween Gfx;
        private AudioStreamPlayer Sfx;

        public override void _Ready()
        {
            Units = (Spatial) GetNode("Units");
            Support = (Spatial) GetNode("Support");
            Hand = (Hand3D) GetNode("Hand");
            Graveyard = (Spatial) GetNode("Discard");
            Deck = (Deck3D) GetNode("Deck");
            Gfx = (Tween) GetNode("GFX");
            Sfx = (AudioStreamPlayer) GetNode("SFX");
        }
        
        public void AddCardToDeck(ICardView cardView)
        {
            Deck.Add(cardView);
        }

        public void Connect(Declaration declaration)
        {
            Declare = declaration;
        }

        public void DisplayName(string name)
        {
            throw new System.NotImplementedException();
        }

        public void DisplayHealth(int health)
        {
            throw new System.NotImplementedException();
        }

        public void Draw(ICardView card)
        {
            GD.Print("Player View Drew Card");

            Tween Command()
            {
                Gfx.RemoveAll();
                
                var card3D = (Card3DView) card;
                card3D.Visible = false;
                
                Deck.AddToTopOfDeck(card3D);
                var globalPosition = card3D.GlobalTransform.origin;
                Deck.Remove(card);
                Hand.Add(card);
                var globalDestination = card3D.GlobalTransform.origin;
                var rotation = new Vector3(-25, 180, 0);
                
                // Wrap In GFX Class
                Gfx.InterpolateProperty(card3D, "visible", false, true, 0.1F);
                Gfx.InterpolateProperty(card3D, "rotation_degrees", card3D.Rotation, rotation, 0.4F);
                    Gfx.InterpolateProperty(card3D, "translation", globalPosition, globalDestination, 0.3F);
                return Gfx;
            }

            Declare(Command);
        }
        
        public void Discard(ICardView card)
        {
            throw new System.NotImplementedException();
        }

        public void Deploy(ICardView card)
        {
            throw new System.NotImplementedException();
        }

        public void Set(ICardView card)
        {
            throw new System.NotImplementedException();
        }

        public void Attack(ICardView attacker, ICardView defender)
        {
            throw new System.NotImplementedException();
        }

        public void AttackDirectly(ICardView attacker)
        {
            throw new System.NotImplementedException();
        }
    }
}