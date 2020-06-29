using System;
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

        [Signal]
        public delegate void AnimationFinished();
        
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
            EmitSignal(nameof(AnimationFinished));
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

        public void Deploy(Card card, bool isOpponent)
        {
            if (isOpponent)
            {
                Hand.RemoveChild(Hand.GetChild(0));
                Hand.AddChild(card);
                Sort(Hand);
            }
            QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, FuturePosition(Units), 0.3F, Delay);
            QueueCallback(card.GetParent(), AddDelay(0.3F), "remove_child", card);
            QueueCallback(Units, Delay, "add_child", card);
        }
        
        private float AddDelay(float delay)
        {
            Delay += delay;
            return Delay;
        }

        private void QueueProperty(Godot.Object obj, string property, object start, object end, float duration,
            float delay)
        {
            Gfx.InterpolateProperty(obj, property, start, end, duration, Tween.TransitionType.Linear,
                Tween.EaseType.In, delay);
        }

        private void QueueCallback(Godot.Object obj, float delay, string callback, object args1 = null, object args2 = null, object args3 = null, object args4 = null, object args5 = null)
        {
            Gfx.InterpolateCallback(obj, delay, callback, args1, args2, args3, args4, args5);
        }
        
        private static void Sort(Container zone)
        {
            zone.Notification(Container.NotificationSortChildren);
        }

        private static Vector2 FuturePosition(Container zone)
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