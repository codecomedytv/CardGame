using System.Collections.Generic;
using System.Linq;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Tags;

namespace CardGame.Server.Game.Skills
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
            if (destroyed.HasTag(TagIds.CannotBeDestroyedByEffect))
            {
                return;
            }
            History.Add(new Move(GameEvents.DestroyByEffect, Card, destroyed, destroyed.Owner.Graveyard));
        }

        protected void TopDeck(Card topDecked)
        {
            History.Add(new Move(GameEvents.TopDeck, Card, topDecked, topDecked.Owner.Deck));
        }
        protected void SetTargets(IEnumerable<Card> cards)
        {
            // TODO: Implement A Way To Send This Information To Messenger (ModifyCard?)
            ValidTargets.AddRange(cards.Where(card => !card.HasTag(TagIds.CannotBeTargeted)));
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