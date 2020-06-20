using System.Collections.Generic;
using CardGame.Server.Game;
using CardGame.Server.Game.Cards;
using Godot.Collections;

namespace CardGame.Server
{
    public partial class Skill
    {
        protected void SetTargets(List<Card> cards)
        {
            Controller.DeclarePlay(new SetTargets(Card, cards));
        }

        protected void AutoTarget()
        {
            Targeting = true;
            Controller.DeclarePlay(new AutoTarget(Card));
        }
    }
}