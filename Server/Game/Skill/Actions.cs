using System.Collections.Generic;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;

namespace CardGame.Server.Game.Skill
{
    public partial class Skill
    {
        protected void Bounce(Card bounced)
        {
            History.Add(new Move(GameEvents.Bounce, Card, bounced, bounced.Owner.Hand));
        }

        protected void Mill(Card milled)
        {
            History.Add(new Move(GameEvents.Mill, Card, milled, milled.Controller.Graveyard));
        }

        protected void Draw()
        {
            
        }

        protected void Discard(Card discarded)
        {
            History.Add(new Move(GameEvents.Discard, Card, discarded, discarded.Owner.Graveyard));
        }

        protected void Destroy(Card destroyed)
        {
            History.Add(new Move(GameEvents.DestroyByEffect, Card, destroyed, destroyed.Owner.Graveyard));
        }

        protected void TopDeck(Card topDecked)
        {
            History.Add(new Move(GameEvents.TopDeck, Card, topDecked, topDecked.Owner.Deck));
        }
        protected void SetTargets(List<Card> cards)
        {
            // TODO: Re-implement This
            // Our previous version was an event, not a command!
            // We also may need to separate Attack Targets and Skill Targets
            // To help sending messages we could probably set it to a godot collection
            // (I don't know if System.Collections works and even if it did, it won't work on the
            // signal end).
        }

        protected void RequestTarget()
        {
            Targeting = true;
            // TODO: Re-implement This
            // The previous version of this was a GameEvent. It is likely that we could possibly hard-code this
            // Although that may cause problems with an animation sync (unless of course the targets don't become
            // valid until we change the client state to a valid state to target)
            // I'm not sure if the targeting bool should exist on this skill or on the player itself.
            
        }
    }
}