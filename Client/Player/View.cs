using CardGame.Client.Library;
using CardGame.Client.Library.Cards;
using Godot;

namespace CardGame.Client.Player
{
    public class View: Control
    {
        public int Health = 8000;
        public Label Damage;
        public Label Deck;
        public Label Discard;
        public HBoxContainer Units;
        public HBoxContainer Support;
        public HBoxContainer Hand;
        private readonly Tween Gfx = new Tween();
        private float Delay = 0.0F;

        public override void _Ready()
        {
            AddChild(Gfx);
            Damage = GetNode<Label>("Damage");
            Deck = GetNode<Label>("Deck");
            Discard = GetNode<Label>("Discard");
            Units = GetNode<HBoxContainer>("Units");
            Support = GetNode<HBoxContainer>("Support");
            Hand = GetNode<HBoxContainer>("Hand");
        }

        public async void Execute()
        {
            Gfx.Start();
            await ToSignal(Gfx, "tween_all_completed");
            Delay = 0.0F;
            Gfx.RemoveAll();
        }

        public void Draw(Card card, string deckSize)
        {

            Hand.AddChild(card);
            Sort(Hand);
            var destination = card.RectGlobalPosition;
            card.RectGlobalPosition = Deck.RectGlobalPosition;
            var originalColor = card.Modulate;
            card.Modulate = Colors.Transparent;
            QueueCallback(Deck, Delay, "set_text", deckSize);
            QueueProperty(card, "RectGlobalPosition", Deck.RectGlobalPosition, destination, 0.2F, AddDelay(0.2F));
            QueueProperty(card, nameof(Modulate), Colors.Transparent, originalColor, 0.1F, Delay);
            QueueCallback(this, AddDelay(0.2F),"Sort", Hand);
        }
        
        private float AddDelay(float delay)
        {
            Delay += delay;
            return Delay;
        }

        private void QueueProperty(Object obj, string property, object start, object end, float duration,
            float delay)
        {
            Gfx.InterpolateProperty(obj, property, start, end, duration, Tween.TransitionType.Linear,
                Tween.EaseType.In, delay);
        }

        private void QueueCallback(Object obj, float delay, string callback, object args1 = null, object args2 = null, object args3 = null, object args4 = null, object args5 = null)
        {
            Gfx.InterpolateCallback(obj, delay, callback, args1, args2, args3, args4, args5);
        }
        
        private static void Sort(Container zone)
        {
            zone.Notification(Container.NotificationSortChildren);
        }
    }
}