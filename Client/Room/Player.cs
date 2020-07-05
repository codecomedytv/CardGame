using System;
using System.Threading.Tasks;
using CardGame.Client.Library;
using CardGame.Client.Library.Cards;
using Godot;

namespace CardGame.Client.Room
{
    public enum States
    {
        Acting,
        Active,
        Idle,
        Passing,
        Passive,
        Processing
    }
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Player: Control
    
    {
        public States State;
        private int DeckCount = 40;
        public int Health = 8000;
        private Label Damage;
        private Label Deck;
        public PanelContainer Discard;
        private Label DiscardCount;
        public HBoxContainer Units;
        public HBoxContainer Support;
        public HBoxContainer Hand;
        private readonly Tween Gfx = new Tween();
        private float Delay = 0.0F;
        private AnimatedSprite PlayingState;
        public Player Opponent;

        [Signal]
        public delegate void AnimationFinished();
        
        public override void _Ready()
        {
            AddChild(Gfx);
            PlayingState = GetNode<AnimatedSprite>("View/PlayingState");
            Damage = GetNode<Label>("Damage");
            Deck = GetNode<Label>("Deck");
            Discard = GetNode<PanelContainer>("Discard");
            DiscardCount = GetNode<Label>("Discard/Count");
            Units = GetNode<HBoxContainer>("Units");
            Support = GetNode<HBoxContainer>("Support");
            Hand = GetNode<HBoxContainer>("Hand");
        }

        public void Reset()
        {
            Delay = 0.0F;
            Gfx.RemoveAll();
        }

        public void SetState(States state)
        {
            State = state;
            if (state == States.Active || state == States.Idle)
            {
                PlayingState.Visible = true;
                PlayingState.Play();
            }

            else
            {
                PlayingState.Frame = 0;
                PlayingState.Visible = false;
                PlayingState.Stop();
            }
        }

        public async Task<object[]> Execute()
        {
            Gfx.Start();
            return await ToSignal(Gfx, "tween_all_completed");
        }

        public void Draw(Card card)
        {
            card.Player = this;
            DeckCount -= 1;
            Hand.AddChild(card);
            Sort(Hand);
            var destination = card.RectGlobalPosition;
            card.RectGlobalPosition = Deck.RectGlobalPosition;
            var originalColor = card.Modulate;
            card.Modulate = Colors.Transparent;
            QueueCallback(Deck, Delay, "set_text", DeckCount.ToString());
            QueueProperty(card, "RectGlobalPosition", Deck.RectGlobalPosition, destination, 0.2F, AddDelay(0.2F));
            QueueProperty(card, nameof(Modulate), Colors.Transparent, originalColor, 0.1F, Delay);
            QueueCallback(this, AddDelay(0.2F),"Sort", Hand);
        }

        public void Deploy(Card card)
        {
            QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, FuturePosition(Units), 0.3F, Delay);
            QueueCallback(card.GetParent(), AddDelay(0.3F), "remove_child", card);
            QueueCallback(Units, Delay, "add_child", card);
        }

        public void SetFaceDown(Card card, bool isOpponent)
        {
            if (isOpponent)
            {
                card.Free();
                card = (Card) Hand.GetChild(0);
            }
            QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, FuturePosition(Support), 0.3F, Delay);
            QueueCallback(card, Delay, nameof(card.FlipFaceDown));
            QueueCallback(card.GetParent(), AddDelay(0.3F), "remove_child", card);
            QueueCallback(Support, Delay, "add_child", card);
        }
        
        public void Activate(Card card, bool isOpponent = false)
        {
            // if (isOpponent)
            // {
            //     Support.RemoveChild(Support.GetChild(Support.GetChildCount() - 1));
            //     Support.AddChild(card);
            //     card.FlipFaceDown();
            // }
            QueueCallback(card, Delay, nameof(card.FlipFaceUp));
            QueueCallback(card, Delay, nameof(card.AddToChain));
        }

        public void Trigger(Card card)
        {
            QueueCallback(card, AddDelay(0.1F), nameof(card.AddToChain));
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

        public void Sort(Container zone)
        {
            zone.Notification(Container.NotificationSortChildren);
        }

        private Vector2 FuturePosition(Container zone)
        {
            var blank = CheckOut.Fetch(0, SetCodes.NullCard);
            zone.AddChild(blank);
            Sort(zone);
            var nextPosition = blank.RectGlobalPosition;
            zone.RemoveChild(blank);
            return nextPosition;
        }

        public void Battle(Card attacker, Card defender, bool isOpponent)
        { 
            // This is imperfect because it is a synchronization based
            var attackerLocation = attacker.RectPosition;
            var defenderLocation = defender.RectPosition;
            var attackerDestination = new Vector2(attacker.RectPosition.x, attacker.RectPosition.y - 50);
            var defenderDestination = new Vector2(defender.RectPosition.x, defender.RectPosition.y + 50);
            
            QueueProperty(attacker, "RectPosition", attackerLocation, attackerDestination, Delay, Delay);
            QueueProperty(defender, "RectPosition", defenderLocation, defenderDestination, Delay, Delay);
            AddDelay(0.5F);
            Opponent.AddDelay(0.5F);
            QueueProperty(attacker, "RectPosition", attackerDestination, attackerLocation, Delay, Delay);
            QueueProperty(defender, "RectPosition", defenderDestination, defenderLocation, Delay, Delay);
            Opponent.AddDelay(0.5F);
            AddDelay(0.5F);
        }

        public void LoseLife(int lifeLost)
        {
            Damage.Text = $"-{lifeLost}";
            // We reduce values here so we can slot it inside of battle
            QueueCallback(this, Delay - 0.8F, nameof(ShowDamage), true);
            QueueCallback(this, Delay, nameof(ShowDamage), false);
        }

        public void ShowDamage(bool show)
        {
            Damage.Visible = show;
        }

        public void SendCardToZone(Card card, ZoneIds zoneId)
        {
            var zone = GetZone(zoneId);
            QueueCallback(card, Delay, nameof(Card.MoveZone), card.GetParent(), zone);
            QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, zone.RectGlobalPosition, Delay, Delay);
        }

        private Control GetZone(ZoneIds zoneId)
        {
            return zoneId switch
            {
                ZoneIds.Deck => Deck,
                ZoneIds.Graveyard => Discard,
                ZoneIds.Field => Units,
                ZoneIds.Support => Support,
                ZoneIds.Hand => Hand,
                _ => throw new ArgumentOutOfRangeException(nameof(zoneId), zoneId, null)
            };
        }
    }
}