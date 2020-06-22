using System.Collections.Generic;
using CardGame.Server.Room;
using CardGame.Server.Room.Cards;
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
            // This shouldn't be an event. We could likely hard code this via signal
            // TODO: Re-implement This
        }
    }
}