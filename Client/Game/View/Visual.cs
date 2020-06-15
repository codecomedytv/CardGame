using CardGame.Client.Library;
using CardGame.Client.Library.Card;
using Godot;
using Godot.Collections;

namespace CardGame.Client.Match
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
        
        public void Setup(Gfx animate, Sfx sfx)
        {
            Animate = animate;
            Sfx = sfx;
        }

        public float Delay(float timeDelay = 0.0F) => Animate.AddDelay(timeDelay);

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
        
        public void Sort(Container zone)
        {
            zone.Notification(Container.NotificationSortChildren);
        }
    }
}