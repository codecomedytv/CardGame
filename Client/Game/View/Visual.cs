using CardGame.Client.Library;
using CardGame.Client.Library.Card;
using Godot;
using Godot.Collections;

namespace CardGameSharp.Client.Game
{
    public class Visual: Control
    {
        protected Label Life;
        public HBoxContainer Hand;
        public HBoxContainer Units;
        public HBoxContainer Support;
        protected Control Discard;
        protected Label Deck;
        protected Label Damage;
        protected Gfx Animate;
        protected int Who;
        protected Sfx Sfx;
        protected History History;
        public Godot.Collections.Dictionary<int, Card> Cards;
        public override void _Ready()
        {
            Life = GetNode("View/Life") as Label;
            Hand = GetNode("Hand") as HBoxContainer;
            Units = GetNode("Units") as HBoxContainer;
            Support = GetNode("Support") as HBoxContainer;
            Discard = GetNode("Discard") as Control;
            Deck = (Label) GetNode("Deck");
            Damage = GetNode("Damage") as Label;
        }
        
        public void Setup(Gfx animate, Sfx sfx, History history, int who)
        {
            Animate = animate;
            Sfx = sfx;
            History = history;
            Who = who;
        }

        public float Delay(object Delay = null)
        {
            if (Delay is float timeDelay)
            {
                return Animate.AddDelay(timeDelay, Who);
            }
            else
            {
                return Animate.TotalDelay(Who);
            }
        }

        public void QueueProperty(Object obj, string property, object start, object end, float duration,
            float Delay = 0.0F)
        {
            Animate.InterpolateProperty(obj, property, start, end, duration, Tween.TransitionType.Linear,
                Tween.EaseType.In, Delay);
        }

        public void QueueCallback(Object obj, float delay, string callback, object args1 = null, object args2 = null, object args3 = null, object args4 = null, object args5 = null)
        {
            Animate.InterpolateCallback(obj, delay, callback, args1, args2, args3, args4, args5);
        }
        
        public Array NextHandPositions(int count)
        {
            var blanks = new Array();
            var positions = new Array();
            for(var i = 0; i < count; i++)
            {
                var blank = Library.Placeholder();
                Hand.AddChild(blank);
                blanks.Add(blank);
            }

            Sort(Hand);
            foreach(Card blank in blanks)
            {
                positions.Add(blank.RectGlobalPosition);
            }
            foreach(Card blank in blanks)
            {
                Hand.RemoveChild(blank);
                blank.Free();
            }
            blanks.Clear();
            return positions;
        }


        public Vector2 FuturePosition(Container zone)
        {
            var blank = Library.Placeholder();
            zone.AddChild(blank);
            Sort(zone);
            var nextPosition = blank.RectGlobalPosition;
            zone.RemoveChild(blank);
            return nextPosition;
        }

        public void Sort(Container zone)
        {
            zone.Notification(Container.NotificationSortChildren);
        }
    }
}