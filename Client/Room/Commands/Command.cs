using System;
using System.Threading.Tasks;
using CardGame.Client.Cards;
using Godot;
using Object = Godot.Object;

namespace CardGame.Client.Room.Commands
{
    public abstract class Command: Object
    {
        protected Tween Gfx;
        protected Command()
        {
            
        }

        public async Task<object[]> Execute(Tween gfx)
        {
            Gfx = gfx;
            return await Execute();
        }

        protected abstract Task<object[]> Execute();
        
        protected async Task<object[]> Start()
        {
            Gfx.Start();
            return await ToSignal(Gfx,"tween_all_completed");
        }

            protected void MoveCard(Card card, Zone destination)
        {
            QueueProperty(card, nameof(Card.Position), card.Position, FuturePosition(destination), 0.1F, 0.1F);
            QueueCallback(card.Zone, 0.2F, nameof(Zone.Remove), card);
            QueueCallback(destination, 0.2F, nameof(Zone.Add), card);
        }
        
        protected void QueueProperty(Card obj, string property, object start, object end, float duration, float delay)
        {
            Gfx.InterpolateProperty(obj, property, start, end, duration, Tween.TransitionType.Linear,
                Tween.EaseType.In, delay);
        }

        protected void QueueCallback(Object obj, float delay, string callback, object args1 = null, object args2 = null, object args3 = null, object args4 = null, object args5 = null)
        {
            Gfx.InterpolateCallback(obj, delay, callback, args1, args2, args3, args4, args5);
        }

        protected void Sort(Container zone)
        {
            zone.Notification(Container.NotificationSortChildren);
        }

        protected Vector2 FuturePosition(Zone zone)
        {
            var blank = Library.Fetch(0, SetCodes.NullCard);
            zone.Add(blank);
            zone.Sort();
            var nextPosition = blank.RectGlobalPosition;
            zone.Remove(blank);
            return nextPosition;
        }
        
        protected Zone GetZone(Player player, ZoneIds zoneId)
        {
            return zoneId switch
            {
                ZoneIds.Deck => player.Deck,
                ZoneIds.Graveyard => player.Discard,
                ZoneIds.Field => player.Units,
                ZoneIds.Support => player.Support,
                ZoneIds.Hand => player.Hand,
                _ => throw new ArgumentOutOfRangeException(nameof(zoneId), zoneId, null)
            };
        }
    }
}
