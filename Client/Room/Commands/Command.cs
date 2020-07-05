using CardGame.Client.Library;
using Godot;

namespace CardGame.Client.Room.Commands
{
    public abstract class Command
    {
        protected Tween Gfx;
        protected Command()
        {
            
        }

        public abstract void Execute(Tween gfx);
        
        protected void QueueProperty(Object obj, string property, object start, object end, float duration, float delay)
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

        protected Vector2 FuturePosition(Container zone)
        {
            var blank = CheckOut.Fetch(0, SetCodes.NullCard);
            zone.AddChild(blank);
            Sort(zone);
            var nextPosition = blank.RectGlobalPosition;
            zone.RemoveChild(blank);
            return nextPosition;
        }
    }
}