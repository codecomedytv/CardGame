using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game.Commands
{
    public class Activate: Command
    {
        // Our Cards Are Activated ClientSide so we always know this is going to be Opponent
        private readonly Opponent Opponent;
        private readonly Card Card;
        private readonly TextureRect ActivationView;

        public Activate(Opponent opponent, Card card, TextureRect activationView)
        {
            Opponent = opponent;
            Card = card;
            ActivationView = activationView;
        }
        
        protected override void SetUp(Effects gfx)
        {
            var fakeCard = Opponent.Support[0];
            Opponent.Support.Remove(fakeCard);
            Opponent.Support.Add(Card);
            Card.Translation = fakeCard.Translation;
            fakeCard.Free();

            var bigScale = ActivationView.RectScale * 1.5F;
            
            ActivationView.Texture = Card.Art;
            gfx.InterpolateProperty(Card, nameof(Card.RotationDegrees), new Vector3(0, 0, 0), new Vector3(0, 180, 0), 0.1F);
            gfx.InterpolateCallback(ActivationView, 0.2F, "set_visible", true);
            gfx.InterpolateProperty(ActivationView, "modulate", Colors.Transparent, ActivationView.Modulate, 0.1F, delay: 0.3F);
            gfx.InterpolateProperty(ActivationView, "rect_scale", bigScale, ActivationView.RectScale, 0.2F, Tween.TransitionType.Linear, Tween.EaseType.In, delay: 0.4F);
            gfx.InterpolateProperty(ActivationView, "modulate", ActivationView.Modulate, Colors.Transparent, 0.1F, delay: 8);
            gfx.InterpolateCallback(ActivationView, 1, "set_visible", false);


            ActivationView.Modulate = Colors.Transparent;
            ActivationView.RectScale = bigScale;
        }
    }
}