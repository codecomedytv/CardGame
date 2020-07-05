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
        public int DeckCount = 40;
        public int Health = 8000;
        private Label Damage;
        public Label Deck;
        public PanelContainer Discard;
        private Label DiscardCount;
        public HBoxContainer Units;
        public HBoxContainer Support;
        public HBoxContainer Hand;
        private readonly Tween Gfx = new Tween();
        private float Delay = 0.0F;
        private AnimatedSprite PlayingState;
        public Player Opponent;

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
    }

        
}