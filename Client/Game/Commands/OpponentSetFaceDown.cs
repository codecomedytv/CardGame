﻿using System.Linq;
using CardGame.Client.Assets.Audio;
using CardGame.Client.Game.Players;
using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game
{
    public class OpponentSetFaceDown: Command
    {
        private readonly Player Opponent;

        public OpponentSetFaceDown(Player opponent)
        {
            Opponent = opponent;
        }
        protected override void SetUp(Effects gfx)
        {
            var card = Opponent.Hand.First();
            var origin = card.Translation;

            card.Controller.Hand.Remove(card);
            card.Controller.Support.Add(card);
            
            var destination = card.Controller.View.Support.GetNode<Sprite3D>($"CardSlot{card.ZoneIndex - 1}").GlobalTransform.origin;
            destination += new Vector3(0, 0, 0.05F);
            card.Translation = origin;

            gfx.Play(Audio.SetCard);
            gfx.InterpolateProperty(card, "translation", origin, destination, 0.3F);
            gfx.InterpolateProperty(card, "rotation_degrees", new Vector3(-25, 0, 0), new Vector3(0, 0, 0),
                0.1F);
            gfx.InterpolateCallback(new Sorter(card.Controller.Hand), 0.2F, nameof(Sorter.Sort));

        }

    }
}