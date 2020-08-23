using System;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Cards.Card3D;
using CardGame.Client.Game.Zones;
using Godot;
using Array = Godot.Collections.Array;

namespace CardGame.Client.Game.Players.Player3D
{
    public class Player3DView: Spatial, IPlayerView
    {
        private Declaration Declare;
        private Spatial _units;
        private Spatial _support;
        private IZoneView _hand;
        private Spatial _discard;
        private IZoneView _deck;
        private Tween _gfx;
        private AudioStreamPlayer _sfx;

        public override void _Ready()
        {
            _units = (Spatial) GetNode("Units");
            _support = (Spatial) GetNode("Support");
            _hand = (IZoneView) GetNode("Hand");
            _discard = (Spatial) GetNode("Discard");
            _deck = (IZoneView) GetNode("Deck");
            _gfx = (Tween) GetNode("GFX");
            _sfx = (AudioStreamPlayer) GetNode("SFX");
        }
        
        public void AddCardToDeck(ICardView cardView)
        {
            _deck.Add(cardView);
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
                // Wrap these calls into the GFX Class so it doesn't rely on Tween
                _gfx.RemoveAll();
                var card3D = (Card3DView) card; // We should look into handling this somehow
                var sHand = (Spatial) _hand;
                var globalPosition = card3D.GlobalTransform.origin;
                //var globalDestination = sHand.GlobalTransform.origin;
                _deck.Remove(card);
                _hand.Add(card); // May wrap these in _hand
                //_hand.Sort();
                var globalDestination = card3D.GlobalTransform.origin;
                _gfx.InterpolateProperty(card3D, "translation", globalPosition, globalDestination, 0.3F);
                _gfx.InterpolateProperty(card3D, "rotation", card3D.Rotation, sHand.Rotation, 0.3F);
                return _gfx;
            }

            Declare(Command);
        }
        
        //tween_target.interpolate_property
        //(self, "translation", self.global_transform.origin, self.global_transform.origin + Vector3(20, 0, 0), 3, Tween.TRANS_LINEAR, Tween.EASE_IN_OUT)
        
        // 1 GetCard
        // 2 GetCard Global Position
        // 3 Remove Card From Deck
        // 4 Add Card To Hand
        // 5 Get Card New Global Position
        // 5 Sort
        // 6 Set Card Global To Deck
        // 7 Interpolate Card Global (Deck -> New Position)
        
        // Note: We could create a new slot, resort the hand and then send the card to that slot?
        // Note: We should also rotate the card somewhere
        // Note: We'll skip sort implementation now, just focus on movement

        
        // Player.Deck.Add(Card); // Card is already in deck (but we may need to move it to top of deck)
        // Card.Position = Player.Deck.Position; // We're using global values here
        // MoveCard(Card, Player.Hand);
        // // TODO: We need to add a modulate from the card back to the card front
        // QueueProperty(Card, "modulate", Colors.Transparent, originalColor, 0.1F, 0.1F);
        // QueueCallback(Sfx, 0.1F, nameof(SoundFx.Draw));
        //     return await Start();
        
        // protected void MoveCard(Card card, Zone destination)
        // {
        //     QueueProperty(card, nameof(Card.Position), card.Position, FuturePosition(destination), 0.1F, 0.1F);
        //     QueueCallback(card.Zone, 0.2F, nameof(Zone.Remove), card);
        //     QueueCallback(destination, 0.2F, nameof(Zone.Add), card);
        // }

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