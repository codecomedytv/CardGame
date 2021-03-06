﻿using System;
using System.Linq;
using CardGame.Client.Assets.Audio;
using CardGame.Client.Game.Players;
using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game
{
    public class OpponentDraw: Command
    {
        private readonly Player Opponent;

        public OpponentDraw(Player opponent)
        {
            Opponent = opponent;
        }
        protected override void SetUp(Effects gfx)
        {
            var card = Opponent.Deck.Last();
            var globalPosition = card.Translation;
            Opponent.Deck.Remove(card);
            Opponent.Hand.Add(card);
            
            card.Translation = card.Controller.View.Hand.GlobalTransform.origin;
            var sorter = new Sorter(card.Controller.Hand);
            sorter.Sort();
			
            // Our destination is where the card is POST-SORT, not the hand origin itself
            var globalDestination = card.Translation;

            var rotation = new Vector3(90, 180, 0);
				
            gfx.Play(Audio.Draw);
            gfx.InterpolateProperty(card, "translation", card.Translation, globalPosition, 0.09F);
            gfx.InterpolateProperty(card, "visible", false, true, 0.1F);
            gfx.InterpolateProperty(card, "translation", globalPosition, globalDestination, 0.2F, delay: 0.1F);
            gfx.InterpolateProperty(card, "rotation_degrees", card.Rotation, rotation, 0.2F, delay: 0);

        }
    }
}