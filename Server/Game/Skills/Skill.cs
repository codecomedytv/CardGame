using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Events;
using CardGame.Server.Game.Tags;
using CardGame.Server.Game.Zones;
using Godot;

namespace CardGame.Server.Game.Skills {
	
	public class Skill : Godot.Object
	{
		public enum Types { Manual, Auto, Constant }

		[Signal]
		public delegate void Resolved();

		protected Player Owner => Card.Owner;
		protected Player Controller => Card.Controller;
		protected Player Opponent => Card.Opponent;
		public Card Card;
		public Match Match;
		protected History History => Match.History;
		protected readonly List<GameEvents> Triggers = new List<GameEvents>();
		protected readonly List<Zone> AreaOfEffects = new List<Zone>();
		public Card Target;
		protected readonly List<Card> ValidTargets = new List<Card>();
		public bool Targeting = false;
		
		protected void Bounce(Card bounced)
        {
	        Move(bounced.Controller.Field, bounced, bounced.Owner.Hand);
            History.Add(new Move(GameEvents.Bounce, Card, bounced.Controller.Field, bounced, bounced.Owner.Hand));
        }

        protected void Mill(Card milled)
        {
	        Move(milled.Controller.Deck, milled, milled.Controller.Graveyard);
            History.Add(new Move(GameEvents.Mill, Card, milled.Controller.Deck, milled, milled.Controller.Graveyard));
        }

        protected void Draw()
        {
            
        }

        protected void Discard(Card discarded)
        {
	        Move(discarded.Owner.Hand, discarded, discarded.Owner.Graveyard);
            History.Add(new Move(GameEvents.Discard, Card, discarded.Owner.Hand, discarded, discarded.Owner.Graveyard));
        }

        protected void Destroy(Card destroyed)
        {
            if (destroyed.HasTag(TagIds.CannotBeDestroyedByEffect))
            {
                return;
            }
            Move(destroyed.Controller.Field, destroyed, destroyed.Owner.Graveyard);
            History.Add(new Move(GameEvents.DestroyByEffect, Card, destroyed.Controller.Field, destroyed, destroyed.Owner.Graveyard));
        }

        protected void TopDeck(Card topDecked)
        {
	        var origin = topDecked.Zone;
	        Move(origin, topDecked, topDecked.Owner.Deck);
            History.Add(new Move(GameEvents.TopDeck, Card, origin, topDecked, topDecked.Owner.Deck));
        }
        protected void AddTargets(IEnumerable<Card> cards)
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

        private void Move(Zone origin, Card card, Zone destination)
        {
	        origin.Remove(card);
	        destination.Add(card);
	        card.Zone = destination;
        }


		
	}
	
}
