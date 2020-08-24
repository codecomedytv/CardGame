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
        private IZoneView Hand;
        private Spatial Graveyard;
        private IZoneView Deck;
        private Tween Gfx;
        private AudioStreamPlayer Sfx;

        public override void _Ready()
        {
            Units = (Spatial) GetNode("Units");
            Support = (Spatial) GetNode("Support");
            Hand = (IZoneView) GetNode("Hand");
            Graveyard = (Spatial) GetNode("Discard");
            Deck = (IZoneView) GetNode("Deck");
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
            Tween Command()
            {
                Gfx.RemoveAll();
                
                card.Visible = false;
                Deck.Add(card);
                var globalPosition = card.Position;
                Deck.Remove(card);
                Hand.Add(card);
                var globalDestination = card.Position;
                var rotation = new Vector3(-25, 180, 0);
                
                // Wrap In GFX Class
                var card3D = (Spatial) card;
                Gfx.InterpolateProperty(card3D, nameof(ICardView.Visible), false, true, 0.1F);
                Gfx.InterpolateProperty(card3D, "rotation_degrees", card3D.Rotation, rotation, 0.4F);
                Gfx.InterpolateProperty(card3D, nameof(ICardView.Position), globalPosition, globalDestination, 0.3F);
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