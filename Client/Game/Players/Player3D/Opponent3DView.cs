﻿using System;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game.Players.Player3D
{
    public class Opponent3DView: Spatial, IPlayerView
    {
        private Spatial _units;
        private Spatial _support;
        private Spatial _hand;
        private Spatial _discard;
        private IZoneView _deck;
        private Tween _gfx;
        private AudioStreamPlayer _sfx;

        public override void _Ready()
        {
            _units = (Spatial) GetNode("Units");
            _support = (Spatial) GetNode("Support");
            _hand = (Spatial) GetNode("Hand");
            _discard = (Spatial) GetNode("Discard");
            _deck = (IZoneView) GetNode("Deck");
            _gfx = (Tween) GetNode("GFX");
            _sfx = (AudioStreamPlayer) GetNode("SFX");
        }

        public void AddCardToDeck(ICardView cardView)
        {
            _deck.Add(cardView);
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
            //throw new System.NotImplementedException();
            Action x = async () =>
            {
                await ToSignal(_gfx, "tween_all_completed");
            };
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