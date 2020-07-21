#nullable enable
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
		protected History History => Card.History;
		protected readonly List<GameEvents> Triggers = new List<GameEvents>();
		protected readonly List<Zone> AreaOfEffects = new List<Zone>();
		public readonly List<Card> ValidTargets = new List<Card>();
		public Card? Target;
		public bool Targeting = false;

		protected void Bounce(Card bounced)
        {
	        Move(bounced.Controller.Field, bounced, bounced.Owner.Hand);
            History.Add(new Bounce(Card, bounced.Owner, bounced));
        }

        protected void Mill(Card milled)
        {
	        Move(milled.Controller.Deck, milled, milled.Controller.Graveyard);
            History.Add(new Mill(Card, milled.Owner, milled));
        }

        protected void Draw(Player player, int count = 1)
        {
	        for (var i = 0; i < count; i++)
	        {
		        var card = player.Deck.Top;
		        Move(player.Deck, card, player.Hand);
		        History.Add(new Draw(Card, player, card));
	        }
        }

        protected void Discard(Card discarded)
        {
	        Move(discarded.Owner.Hand, discarded, discarded.Owner.Graveyard);
            History.Add(new Discard(Card, discarded.Owner, discarded));
        }

        protected void Destroy(Card destroyed)
        {
            if (destroyed.HasTag(TagIds.CannotBeDestroyedByEffect))
            {
                return;
            }
            Move(destroyed.Controller.Field, destroyed, destroyed.Owner.Graveyard);
            History.Add(new DestroyByEffect(Card, destroyed.Owner, destroyed));
            History.Add(new SentToZone(Controller, destroyed, ZoneIds.Graveyard));
        }

        protected void TopDeck(Card topDecked)
        {
	        var origin = topDecked.Zone;
	        Move(origin, topDecked, topDecked.Owner.Deck);
            History.Add(new TopDeck(Card, topDecked.Owner, topDecked));
        }
        protected void AddTargets(IEnumerable<Card> cards)
        {
            ValidTargets.AddRange(cards.Where(card => !card.HasTag(TagIds.CannotBeTargeted)));
        }

        protected void RequestTarget()
        {
	        Controller.State = States.Targeting;
	        Controller.TargetingSkill = this;
	        History.Add(new SelectTarget(Card, Controller, ValidTargets));
	        GD.PushWarning("Requires Resolve To Await");
        }

        protected Unit GetAttackingUnit()
        {
	        return Controller.AttackingWith ?? Opponent.AttackingWith;
        }

        private void Move(Zone origin, Card card, Zone destination)
        {
	        origin.Remove(card);
	        destination.Add(card);
	        card.Zone = destination;
        }


		
	}
	
}
