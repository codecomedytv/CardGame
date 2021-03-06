﻿using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Skills;
using CardGame.Server.Game.Tags;

namespace CardGame.Server
{
    public class PlayerCannotTakeDamage : Unit
    {
        public PlayerCannotTakeDamage(Player owner)
        {
            Owner = owner;
            Controller = owner;
            Title = "Debug.PlayerCannotTakeDamage";
            Power = 1000;
            SetCode = SetCodes.DebugPlayerCannotTakeDamage;
            Skill = new PlayerCannotTakeBattleDamage(this);
        }

        private class PlayerCannotTakeBattleDamage: Constant
        {
        private readonly Tag Tag = new Tag(TagIds.CannotTakeBattleDamage);
            public PlayerCannotTakeBattleDamage(Card card)
            {
                Card = card;
                AreaOfEffects.Add(Controller.Field);
            }

            protected override void _Apply()
            {
                Tag.UnTagAll();
                if (Controller.Field.Contains(Card))
                {
                    Tag.Add(Controller);
                }
            }
        }
    }
}

