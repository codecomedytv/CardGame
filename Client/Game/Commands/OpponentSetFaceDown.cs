﻿using System.Linq;
using CardGame.Client.Assets.Audio;
using CardGame.Client.Game.Players;
using CardGame.Client.Game.Zones;
using CardGame.Client.Game.Zones.ModelControllers;
using Godot;

namespace CardGame.Client.Game
{
    public class OpponentSetFaceDown: Command
    {
        private readonly Opponent Opponent;

        public OpponentSetFaceDown(Opponent opponent)
        {
            Opponent = opponent;
        }
        protected override void SetUp(Effects gfx)
        {
            var card = Opponent.Hand.First();
            var origin = card.Translation;

            card.Controller.Hand.Remove(card);
            card.Controller.Support.Add(card);
            var destination = card.Translation + new Vector3(0, 0, 0.05F);
            card.Translation = origin;

            gfx.Play(Audio.SetCard);
            gfx.InterpolateProperty(card, nameof(card.Translation), origin, destination, 0.3F);
            gfx.InterpolateProperty(card, nameof(card.RotationDegrees), new Vector3(-25, 0, 0), new Vector3(0, 0, 0),
                0.1F);
            gfx.InterpolateCallback(new Sorter(card.Controller.Hand), 0.2F, nameof(Sorter.Sort));

        }

    }
}