﻿using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game.Players.Player3D
{
    public class Opponent3DView: Spatial, IPlayerView
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
        
        public void Connect(Declaration declaration)
        {
            Declare = declaration;
        }

        public void AddCardToDeck(Card cardView)
        {
            Deck.Add(cardView);
        }

        public void Connect(Func<Tween> addCommand)
        {
            throw new NotImplementedException();
        }

        public void DisplayName(string name)
        {
            throw new System.NotImplementedException();
        }

        public void DisplayHealth(int health)
        {
            throw new System.NotImplementedException();
        }

        public void Draw(Card ignoredCard)
        {
            Tween Command()
            {
                var card = Deck.ElementAt(Deck.Count - 1);
                Deck.Add(card);
                var globalPosition = card.Position;
                Deck.Remove(card);
                Hand.Add(card);
                var globalDestination = card.Position;
                var rotation = new Vector3(60, 0, 0);
                
                // Wrap In GFX Class
                Gfx.InterpolateProperty(card, nameof(card.Visible), false, true, 0.1F);
                Gfx.InterpolateProperty(card, nameof(card.Position), globalPosition, globalDestination, 0.1F);
                Gfx.InterpolateProperty(card, nameof(card.RotationDegrees), card.Rotation, rotation, 0.1F);
                return Gfx;
            };

           Declare(Command);
        }
        
        public void Discard(Card card)
        {
            throw new System.NotImplementedException();
        }

        public void Deploy(Card card)
        {
            throw new System.NotImplementedException();
        }

        public void Set(Card card)
        {
            throw new System.NotImplementedException();
        }

        public void Attack(Card attacker, Card defender)
        {
            throw new System.NotImplementedException();
        }

        public void AttackDirectly(Card attacker)
        {
            throw new System.NotImplementedException();
        }
    }
}